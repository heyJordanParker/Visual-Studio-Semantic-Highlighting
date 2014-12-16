using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace SemanticCodeHighlighting {
	public class Classifier : ITagger<ClassificationTag> {
		private IClassificationType _classification;
		private ITagAggregator<IClassificationTag> _aggregator;

#pragma warning disable 67
		public event EventHandler<SnapshotSpanEventArgs> TagsChanged;
#pragma warning restore 67

		internal Classifier(
			IClassificationTypeRegistryService registry,
			ITagAggregator<IClassificationTag> aggregator) {
			_classification = registry.GetClassificationType(Config.ClassificationName);
			_aggregator = aggregator;
		}

		public IEnumerable<ITagSpan<ClassificationTag>> GetTags(NormalizedSnapshotSpanCollection spans) {
			if(spans.Count == 0) {
				yield break;
			}

			ITextSnapshot snapshot = spans[0].Snapshot;
			var contentType = snapshot.TextBuffer.ContentType;
			


			// try the IClassifier thingie
			// using this we can find the already classified variables and simply count them and add classification tags
			var tags = _aggregator.GetTags(spans).ToArray();
			var classifications = tags.Select(s => s.Tag.ClassificationType);

			foreach(var classifiedSpan in tags) {

				var classification = classifiedSpan.Tag.ClassificationType.Classification;
				if(!classification.ToLowerInvariant().Equals("identifier".ToLowerInvariant()))
					continue;

				foreach(SnapshotSpan span in classifiedSpan.Span.GetSpans(snapshot)) {
					yield return new TagSpan<ClassificationTag>(span, new ClassificationTag(_classification));
				}
			}
		}
	}
}