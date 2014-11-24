using System;
using System.Collections.Generic;
using System.Windows.Media;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;

namespace SemanticCodeHighlighting {

	/// <summary>
	/// Classifier that classifies all text as an instance of the OrinaryClassifierType
	/// </summary>
	class Classifier : IClassifier {
		private IClassificationType _classificationType;
		private ITextBuffer _textBuffer;

		internal Classifier(ITextBuffer textBuffer, IClassificationTypeRegistryService registry) {
			_textBuffer = textBuffer;
			_classificationType = registry.GetClassificationType("SemanticCodeHighlighting");
		}

		/// <summary>
		/// This method scans the given SnapshotSpan for potential matches for this classification.
		/// In this instance, it classifies everything and returns each span as a new ClassificationSpan.
		/// </summary>
		/// <param name="trackingSpan">The span currently being classified</param>
		/// <returns>A list of ClassificationSpans that represent spans identified to be of this classification</returns>
		public IList<ClassificationSpan> GetClassificationSpans(SnapshotSpan span) {
			//create a list to hold the results

			var classifications = new List<ClassificationSpan>();
			classifications.Add(new ClassificationSpan(new SnapshotSpan(span.Snapshot, new Span(span.Start, span.Length)),
														   _classificationType));
			return classifications;
		}

#pragma warning disable 67
		// This event gets raised if a non-text change would affect the classification in some way,
		// for example typing /* would cause the classification to change in C# without directly
		// affecting the span.
		public event EventHandler<ClassificationChangedEventArgs> ClassificationChanged;
#pragma warning restore 67
	}
}
