using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace SemanticCodeHighlighting {
	public class ClassifierProvider {

		[Export(typeof(IViewTaggerProvider))]
		[ContentType("code")]
		[TagType(typeof(ClassificationTag))]
		public class KeywordClassifierProvider : IViewTaggerProvider {
			[Import]
			internal IClassificationTypeRegistryService classificationRegistry;

			[Import]
			internal IBufferTagAggregatorFactoryService aggregator;

			public ITagger<T> CreateTagger<T>(ITextView textView, ITextBuffer buffer) where T : ITag {
				return new Classifier(classificationRegistry, aggregator.CreateTagAggregator<IClassificationTag>(buffer), textView) as ITagger<T>;
			}
		} 
	}
}