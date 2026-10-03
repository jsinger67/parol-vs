using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Language.StandardClassification;
using System;
using System.Collections.Generic;

namespace parol_vs
{
    /// <summary>
    /// Classifier that tokenizes parol source text and assigns token-level classifications.
    /// </summary>
    internal class ParolEditorClassifier : IClassifier
    {
        private static readonly HashSet<string> Keywords = new HashSet<string>(StringComparer.Ordinal)
        {
            "start",
            "title",
            "line_comment",
            "block_comment",
            "comment",
            "scanner",
            "sc",
            "skip",
            "allow_unmatched",
            "push",
            "pop",
            "auto_newline_off",
            "auto_ws_off",
            "on",
            "enter",
            "user_type",
            "nt_type",
            "grammar_type",
            "t_type",
        };

        private readonly ITextBuffer textBuffer;
        private readonly IClassificationType commentType;
        private readonly IClassificationType stringType;
        private readonly IClassificationType regexType;
        private readonly IClassificationType keywordType;
        private readonly IClassificationType operatorType;
        private readonly IClassificationType identifierType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParolEditorClassifier"/> class.
        /// </summary>
        /// <param name="registry">Classification registry.</param>
        /// <param name="textBuffer">Text buffer for change notifications.</param>
        internal ParolEditorClassifier(IClassificationTypeRegistryService registry, ITextBuffer textBuffer)
        {
            this.textBuffer = textBuffer;
            this.commentType = registry.GetClassificationType(PredefinedClassificationTypeNames.Comment);
            this.stringType = registry.GetClassificationType(PredefinedClassificationTypeNames.String);
            this.regexType = this.stringType;
            this.keywordType = registry.GetClassificationType(PredefinedClassificationTypeNames.Keyword);
            this.operatorType = registry.GetClassificationType(PredefinedClassificationTypeNames.Operator);
            this.identifierType = registry.GetClassificationType(PredefinedClassificationTypeNames.Identifier);
            this.textBuffer.Changed += this.OnBufferChanged;
        }

        #region IClassifier

        public event EventHandler<ClassificationChangedEventArgs>? ClassificationChanged;

        /// <summary>
        /// Gets all the <see cref="ClassificationSpan"/> objects that intersect with the given range of text.
        /// </summary>
        /// <param name="span">The span currently being classified.</param>
        /// <returns>A list of ClassificationSpans that represent spans identified to be of this classification.</returns>
        public IList<ClassificationSpan> GetClassificationSpans(SnapshotSpan span)
        {
            string text = span.Snapshot.GetText();
            int requestedStart = span.Start.Position;
            int requestedEnd = span.End.Position;
            var result = new List<ClassificationSpan>();

            foreach (Token token in Tokenize(text))
            {
                int tokenEnd = token.Start + token.Length;
                if (tokenEnd <= requestedStart || token.Start >= requestedEnd)
                {
                    continue;
                }

                var tokenSpan = new SnapshotSpan(span.Snapshot, new Span(token.Start, token.Length));
                result.Add(new ClassificationSpan(tokenSpan, token.ClassificationType));
            }

            return result;
        }

        #endregion

        private void OnBufferChanged(object sender, TextContentChangedEventArgs e)
        {
            var changedSpan = new SnapshotSpan(e.After, 0, e.After.Length);
            this.ClassificationChanged?.Invoke(this, new ClassificationChangedEventArgs(changedSpan));
        }

        private IEnumerable<Token> Tokenize(string text)
        {
            int i = 0;
            while (i < text.Length)
            {
                char c = text[i];

                if (c == '/' && i + 1 < text.Length)
                {
                    if (text[i + 1] == '/')
                    {
                        int start = i;
                        i += 2;
                        while (i < text.Length && text[i] != '\r' && text[i] != '\n')
                        {
                            i++;
                        }

                        yield return new Token(start, i - start, this.commentType);
                        continue;
                    }

                    if (text[i + 1] == '*')
                    {
                        int start = i;
                        i += 2;
                        while (i + 1 < text.Length && !(text[i] == '*' && text[i + 1] == '/'))
                        {
                            i++;
                        }

                        i = i + 1 < text.Length ? i + 2 : text.Length;
                        yield return new Token(start, i - start, this.commentType);
                        continue;
                    }

                    int regexStart = i;
                    i++;
                    bool escaped = false;
                    bool closed = false;
                    while (i < text.Length)
                    {
                        char rc = text[i];
                        if (escaped)
                        {
                            escaped = false;
                            i++;
                            continue;
                        }

                        if (rc == '\\')
                        {
                            escaped = true;
                            i++;
                            continue;
                        }

                        if (rc == '/')
                        {
                            i++;
                            closed = true;
                            break;
                        }

                        if (rc == '\r' || rc == '\n')
                        {
                            break;
                        }

                        i++;
                    }

                    if (closed && i - regexStart > 2)
                    {
                        yield return new Token(regexStart, i - regexStart, this.regexType);
                        continue;
                    }

                    i = regexStart + 1;
                }

                if (c == '"' || c == '\'')
                {
                    char quote = c;
                    int start = i;
                    i++;
                    bool escaped = false;

                    while (i < text.Length)
                    {
                        char sc = text[i];
                        if (escaped)
                        {
                            escaped = false;
                            i++;
                            continue;
                        }

                        if (sc == '\\')
                        {
                            escaped = true;
                            i++;
                            continue;
                        }

                        if (sc == quote)
                        {
                            i++;
                            break;
                        }

                        if (sc == '\r' || sc == '\n')
                        {
                            break;
                        }

                        i++;
                    }

                    yield return new Token(start, i - start, this.stringType);
                    continue;
                }

                if (c == '%' && i + 1 < text.Length && text[i + 1] == '%')
                {
                    yield return new Token(i, 2, this.operatorType);
                    i += 2;
                    continue;
                }

                if (c == '?' && i + 1 < text.Length && (text[i + 1] == '!' || text[i + 1] == '='))
                {
                    yield return new Token(i, 2, this.operatorType);
                    i += 2;
                    continue;
                }

                if (c == '%' && i + 1 < text.Length && IsIdentifierStart(text[i + 1]))
                {
                    int start = i;
                    i += 2;
                    while (i < text.Length && IsIdentifierPart(text[i]))
                    {
                        i++;
                    }

                    string directive = text.Substring(start + 1, i - (start + 1));
                    IClassificationType type = Keywords.Contains(directive) ? this.keywordType : this.identifierType;
                    yield return new Token(start, i - start, type);
                    continue;
                }

                if (IsOperator(c))
                {
                    yield return new Token(i, 1, this.operatorType);
                    i++;
                    continue;
                }

                if (IsIdentifierStart(c))
                {
                    int start = i;
                    i++;
                    while (i < text.Length && IsIdentifierPart(text[i]))
                    {
                        i++;
                    }

                    yield return new Token(start, i - start, this.identifierType);
                    continue;
                }

                i++;
            }
        }

        private static bool IsIdentifierStart(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '_';
        }

        private static bool IsIdentifierPart(char c)
        {
            return IsIdentifierStart(c) || (c >= '0' && c <= '9');
        }

        private static bool IsOperator(char c)
        {
            return c == '|' || c == ':' || c == ';' || c == '^' || c == '=' || c == '@';
        }

        private readonly struct Token
        {
            internal Token(int start, int length, IClassificationType classificationType)
            {
                this.Start = start;
                this.Length = length;
                this.ClassificationType = classificationType;
            }

            internal int Start { get; }

            internal int Length { get; }

            internal IClassificationType ClassificationType { get; }
        }
    }
}
