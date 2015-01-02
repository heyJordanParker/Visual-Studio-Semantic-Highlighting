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

#pragma warning disable 67
		public event EventHandler<SnapshotSpanEventArgs> TagsChanged;
#pragma warning restore 67

		internal Classifier(IClassificationTypeRegistryService typeRegistry, ITagAggregator<IClassificationTag> aggregator, ITextView textView, IClassificationFormatMap formatMap) {
			_classification = typeRegistry.GetClassificationType(Config.ClassificationName);
			_aggregator = aggregator;
			_colorizer = textView.Properties.GetOrCreateSingletonProperty(() => new Colorizer(typeRegistry, formatMap));
			textView.LayoutChanged += OnChangedEvent;
			textView.TextBuffer.Changed += OnChangedEvent;
			formatMap.ClassificationFormatMappingChanged += OnChangedEvent;
		}

		private void OnChangedEvent(object sender, EventArgs eventArgs) {
			_colorizer.UpdateClassifications();
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

			List<string> classifiedTexts = new List<string>();
			foreach(var classifiedSpan in tags) {
				classifiedTexts.AddRange(classifiedSpan.Span.GetSpans(snapshot).Select(span => span.GetText()));
			}
			_colorizer.GenerateColors(classifiedTexts.ToArray());

			foreach(var classifiedSpan in tags) {

				foreach(SnapshotSpan span in classifiedSpan.Span.GetSpans(snapshot)) {
					var classificationType = _colorizer.GetClassification(span.GetText());
					if(classificationType == null) continue;
					yield return new TagSpan<ClassificationTag>(span, new ClassificationTag(classificationType));
				}
			}
		}
	}
}