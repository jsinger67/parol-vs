using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;
using System.Windows.Media;

namespace parol_vs
{
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = ParolEditorClassifierClassificationDefinition.CommentTypeName)]
    [Name("Parol Comment")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class ParolCommentFormat : ClassificationFormatDefinition
    {
        public ParolCommentFormat()
        {
            this.DisplayName = "Parol Comment";
            this.ForegroundColor = Colors.ForestGreen;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = ParolEditorClassifierClassificationDefinition.StringTypeName)]
    [Name("Parol String")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class ParolStringFormat : ClassificationFormatDefinition
    {
        public ParolStringFormat()
        {
            this.DisplayName = "Parol String";
            this.ForegroundColor = Colors.Brown;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = ParolEditorClassifierClassificationDefinition.RegexTypeName)]
    [Name("Parol Regex")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class ParolRegexFormat : ClassificationFormatDefinition
    {
        public ParolRegexFormat()
        {
            this.DisplayName = "Parol Regex";
            this.ForegroundColor = Colors.DarkOrange;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = ParolEditorClassifierClassificationDefinition.KeywordTypeName)]
    [Name("Parol Keyword")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class ParolKeywordFormat : ClassificationFormatDefinition
    {
        public ParolKeywordFormat()
        {
            this.DisplayName = "Parol Keyword";
            this.ForegroundColor = Colors.MediumBlue;
            this.IsBold = true;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = ParolEditorClassifierClassificationDefinition.OperatorTypeName)]
    [Name("Parol Operator")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class ParolOperatorFormat : ClassificationFormatDefinition
    {
        public ParolOperatorFormat()
        {
            this.DisplayName = "Parol Operator";
            this.ForegroundColor = Colors.DarkCyan;
        }
    }

    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = ParolEditorClassifierClassificationDefinition.IdentifierTypeName)]
    [Name("Parol Identifier")]
    [UserVisible(true)]
    [Order(Before = Priority.Default)]
    internal sealed class ParolIdentifierFormat : ClassificationFormatDefinition
    {
        public ParolIdentifierFormat()
        {
            this.DisplayName = "Parol Identifier";
            this.ForegroundColor = Colors.SlateBlue;
        }
    }
}
