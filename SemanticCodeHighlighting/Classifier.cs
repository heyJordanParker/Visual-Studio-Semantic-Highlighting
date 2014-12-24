using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using SemanticCodeHighlighting.Colorization;

namespace SemanticCodeHighlighting {

	public class Classifier : ITagger<ClassificationTag> {

		private readonly IClassificationType _classification;
		private readonly ITagAggregator<IClassificationTag> _aggregator;
		private Colorizer _colorizer;
		private Parser _parser;

#pragma warning disable 67
		public event EventHandler<SnapshotSpanEventArgs> TagsChanged;
#pragma warning restore 67

		internal Classifier(IClassificationTypeRegistryService registry, ITagAggregator<IClassificationTag> aggregator, ITextView textView) {
			_classification = registry.GetClassificationType(Config.ClassificationName);
			_aggregator = aggregator;
			_colorizer = textView.Properties.GetOrCreateSingletonProperty(() => new Colorizer());
			_parser = textView.Properties.GetOrCreateSingletonProperty(() => new Parser());
		}

		public IEnumerable<ITagSpan<ClassificationTag>> GetTags(NormalizedSnapshotSpanCollection spans) {
			if(spans.Count == 0) {
				yield break;
			}

			ITextSnapshot snapshot = spans[0].Snapshot;
			//var contentType = snapshot.TextBuffer.ContentType; //C#


			// using this we can find the already classified variables and simply count them and add classification tags
			var tags = _aggregator.GetTags(spans).Where((span) => span.Tag.ClassificationType.Classification.Equals("identifier")).ToArray();

//			ColorizedSpan[] colorizations = _colorizer.GetColorizedSpans(tags);
//
//			foreach(var colorizedSpan in colorizations) {
//				yield return new TagSpan<ClassificationTag>(colorizedSpan.Span, new ClassificationTag(colorizedSpan.Classification));
//			}

			foreach(var classifiedSpan in tags) {

				foreach(SnapshotSpan span in classifiedSpan.Span.GetSpans(snapshot)) {
					yield return new TagSpan<ClassificationTag>(span, new ClassificationTag(_classification));
				}
			}
		}
	}
}