using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;

namespace parol_vs
{
    /// <summary>
    /// Classification type definitions for parol tokens.
    /// </summary>
    internal static class ParolEditorClassifierClassificationDefinition
    {
        internal const string CommentTypeName = "Parol.Comment";
        internal const string StringTypeName = "Parol.String";
        internal const string RegexTypeName = "Parol.Regex";
        internal const string KeywordTypeName = "Parol.Keyword";
        internal const string OperatorTypeName = "Parol.Operator";
        internal const string IdentifierTypeName = "Parol.Identifier";

        // Disable "field is never assigned/used" warnings. Justification: fields are used by MEF.
#pragma warning disable 169
#pragma warning disable 649

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(CommentTypeName)]
        private static ClassificationTypeDefinition commentTypeDefinition;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(StringTypeName)]
        private static ClassificationTypeDefinition stringTypeDefinition;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(RegexTypeName)]
        private static ClassificationTypeDefinition regexTypeDefinition;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(KeywordTypeName)]
        private static ClassificationTypeDefinition keywordTypeDefinition;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(OperatorTypeName)]
        private static ClassificationTypeDefinition operatorTypeDefinition;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(IdentifierTypeName)]
        private static ClassificationTypeDefinition identifierTypeDefinition;

#pragma warning restore 169
#pragma warning restore 649
    }
}
