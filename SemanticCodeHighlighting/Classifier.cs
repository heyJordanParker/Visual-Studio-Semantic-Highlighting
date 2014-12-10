using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace SemanticCodeHighlighting {
	public class Classifier : ITagger<ClassificationTag> {
		private IClassificationType _classification;
		private ITagAggregator<ClassificationTag> _aggregator;

#pragma warning disable 67
		public event EventHandler<SnapshotSpanEventArgs> TagsChanged;
#pragma warning restore 67

		internal Classifier(
			IClassificationTypeRegistryService registry,
			ITagAggregator<ClassificationTag> aggregator) {
			_classification = registry.GetClassificationType(Config.ClassificationName);
			_aggregator = aggregator;
		}

		public IEnumerable<ITagSpan<ClassificationTag>> GetTags(NormalizedSnapshotSpanCollection spans) {
			if(spans.Count == 0) {
				yield break;
			}

			ITextSnapshot snapshot = spans[0].Snapshot;

//			ILanguageKeywords keywords = GetKeywordsByContentType(snapshot.TextBuffer.ContentType);
//			if (keywords == null)
//			{
//				yield break;
//			}

			// using this we can find the already classified variables and simply count them and add classification tags
			foreach(var classifiedSpan in from cs in _aggregator.GetTags(spans)
				let name = cs.Tag.ClassificationType.Classification.ToLowerInvariant()
				where name.Contains("variable")
				let classifiedSpans = cs.Span.GetSpans(snapshot)
				where classifiedSpans.Count > 0
				select classifiedSpans[0]) {
//				string text = classifiedSpan.GetText();
//				if (keywords.ControlFlow.Contains(text))
				yield return new TagSpan<ClassificationTag>(classifiedSpan, new ClassificationTag(_classification));
			}
		}
	}
}