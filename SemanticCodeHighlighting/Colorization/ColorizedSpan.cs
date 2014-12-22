using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;

namespace SemanticCodeHighlighting.Colorization {
	public struct ColorizedSpan {
		public SnapshotSpan Span { get; set; }
		public IClassificationType Classification { get; set; }
	}
}