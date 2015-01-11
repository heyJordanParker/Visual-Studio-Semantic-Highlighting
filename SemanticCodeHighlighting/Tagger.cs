using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Text.Tagging;
using NUnit.Framework;
using SemanticCodeHighlighting.Colorization;

namespace SemanticCodeHighlighting {

	public class Tagger : ITagger<ClassificationTag> {
		private readonly Colorizer _colorizer;
		private readonly IClassificationFormatMap _formatMap;
		private readonly IClassifier _classifier;

		public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

		internal Tagger(IClassificationTypeRegistryService typeRegistry, ITextView textView, IClassificationFormatMap formatMap, IClassifier classifier) {
			_classifier = classifier;
			_colorizer = textView.Properties.GetOrCreateSingletonProperty(() => new Colorizer(typeRegistry));
			_formatMap = formatMap;
			textView.LayoutChanged += OnLayoutChangedEvent;
		}

		private void OnLayoutChangedEvent(object sender, TextViewLayoutChangedEventArgs textViewLayoutChangedEventArgs) {
			//TODO:FIX when editing an  OnLayoutChanged event is raised with all of the colorized fields in it. this causes the text to blink
			if(textViewLayoutChangedEventArgs.NewSnapshot.Length == textViewLayoutChangedEventArgs.OldSnapshot.Length) {
				_colorizer.UpdateClassifications(_formatMap);
			}
		}

		public IEnumerable<ITagSpan<ClassificationTag>> GetTags(NormalizedSnapshotSpanCollection spans) {
			
			if(spans.Count == 0) {
				yield break;
			}

			foreach(var snapshotSpan in spans) {
				var classifications = _classifier.GetClassificationSpans(snapshotSpan);
				foreach(var classificationSpan in classifications) {
					if(!classificationSpan.ClassificationType.Classification.Equals(Config.BaseClassification)) continue;

					var colorization = _colorizer.GetColorization(classificationSpan.Span.GetText());
					yield return new TagSpan<ClassificationTag>(classificationSpan.Span, colorization.ClassificationTag);
				}
			}
		}
	}
}