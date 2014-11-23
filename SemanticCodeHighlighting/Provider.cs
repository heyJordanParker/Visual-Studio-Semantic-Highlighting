using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace SemanticCodeHighlighting {
	/// <summary>
	/// This class causes a classifier to be added to the set of classifiers. Since 
	/// the content type is set to "text", this classifier applies to all text files
	/// </summary>
	[Export(typeof(IClassifierProvider))]
	[ContentType("text")]
	internal class Provider : IClassifierProvider {
		/// <summary>
		/// Import the classification registry to be used for getting a reference
		/// to the custom classification type later.
		/// </summary>
		[Import]
		internal IClassificationTypeRegistryService classificationRegistry = null; // Set via MEF

		public IClassifier GetClassifier(ITextBuffer buffer) {
			return buffer.Properties.GetOrCreateSingletonProperty<Classifier>(
				() => new Classifier(classificationRegistry));
		}
	}
}