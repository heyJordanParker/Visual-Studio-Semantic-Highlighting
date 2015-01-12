using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace SemanticCodeHighlighting {
	// ReSharper disable once ClassNeverInstantiated.Global
	public class ClassifierProvider {

		[Export(typeof(IViewTaggerProvider))]
		[ContentType("CSharp")]
		[TagType(typeof(ClassificationTag))]
		public class KeywordClassifierProvider : IViewTaggerProvider {
			[Import]
			internal IClassificationTypeRegistryService classificationRegistry;

			[Import]
			internal IClassificationFormatMapService formatMap;

			[Import]
			internal IClassifierAggregatorService classifierAggregator;

			public ITagger<T> CreateTagger<T>(ITextView textView, ITextBuffer textBuffer) where T : ITag {
				return textView.Properties.GetOrCreateSingletonProperty(
					() =>
						new Tagger(
							classificationRegistry, 
							textView,
							formatMap.GetClassificationFormatMap(textView),
							classifierAggregator.GetClassifier(textBuffer)
							) as ITagger<T>);
			}
		} 
	}
}