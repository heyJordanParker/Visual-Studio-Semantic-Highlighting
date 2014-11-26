using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace SemanticCodeHighlighting {
	internal static class ClassificationDefinition {
		/// <summary>
		/// Defines the "SemanticCodeHighlighting" classification type.
		/// </summary>
		[Export(typeof(ClassificationTypeDefinition))]
		[Name("SemanticCodeHighlighting")]
		internal static ClassificationTypeDefinition classificationTypeDefinition = null;
	}
}
