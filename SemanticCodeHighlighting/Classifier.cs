using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using NUnit.Framework;
using SemanticCodeHighlighting.Colorization;

namespace SemanticCodeHighlighting {

	public class Classifier : ITagger<ClassificationTag> {
		private readonly Colorizer _colorizer;
		private readonly IClassifier _classifier;
		private readonly Dictionary<IClassificationType, ClassificationTag> _classificationTagsCache;

		public event EventHandler<SnapshotSpanEventArgs> TagsChanged = delegate { };

		internal Classifier(IClassificationTypeRegistryService typeRegistry, ITextView textView, IClassificationFormatMap formatMap, IClassifier classifier) {
			_classifier = classifier;
			_colorizer = textView.Properties.GetOrCreateSingletonProperty(() => new Colorizer(typeRegistry, formatMap));
			textView.LayoutChanged += OnChangedEvent;
			textView.GotAggregateFocus += OnFirstFocus;
			_classificationTagsCache = new Dictionary<IClassificationType, ClassificationTag>();
		}

		private void OnChangedEvent(object sender, TextViewLayoutChangedEventArgs eventArgs) {
			_colorizer.UpdateClassifications();
			
		}
		private void OnFirstFocus(object textView, EventArgs e) {
			((ITextView)textView).GotAggregateFocus -= OnFirstFocus;
			//on first focus; called just once
		}

		public IEnumerable<ITagSpan<ClassificationTag>> GetTags(NormalizedSnapshotSpanCollection spans) {
			
			if(spans.Count == 0) {
				yield break;
			}

			foreach(var snapshotSpan in spans) {
				var classifications = _classifier.GetClassificationSpans(snapshotSpan);
				foreach(var classificationSpan in classifications.Where(span => span.ClassificationType.Classification.Equals(Config.BaseClassification))) {
					var classificationType = _colorizer.GenerateClassification(classificationSpan.Span.GetText());

					if(!_classificationTagsCache.ContainsKey(classificationType))
						_classificationTagsCache.Add(classificationType, new ClassificationTag(classificationType));

					yield return new TagSpan<ClassificationTag>(classificationSpan.Span, _classificationTagsCache[classificationType]);
				}
			}
		}
	}
}