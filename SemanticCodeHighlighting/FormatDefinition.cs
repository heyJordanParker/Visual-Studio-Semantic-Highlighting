using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace SemanticCodeHighlighting {
	#region Format definition
	/// <summary>
	/// Defines an editor format for the SemanticCodeHighlighting type that has a purple background
	/// and is underlined.
	/// </summary>
	[Export(typeof(EditorFormatDefinition))]
	[ClassificationType(ClassificationTypeNames = "SemanticCodeHighlighting")]
	[Name("SemanticCodeHighlighting")]
	[UserVisible(true)] //this should be visible to the end user
	[Order(Before = Priority.Default)] //set the priority to be after the default classifiers
	internal sealed class FormatDefinition : ClassificationFormatDefinition {
		/// <summary>
		/// Defines the visual format for the "SemanticCodeHighlighting" classification type
		/// </summary>
		public FormatDefinition() {
			DisplayName = "SemanticCodeHighlighting"; //human readable version of the name
			BackgroundColor = Colors.BlueViolet;
			TextDecorations = System.Windows.TextDecorations.Underline;
		}
	}
	#endregion //Format definition
}
