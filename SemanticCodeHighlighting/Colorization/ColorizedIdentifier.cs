using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;

namespace SemanticCodeHighlighting.Colorization {
	public class ColorizedIdentifier {
		private readonly string _text;
		private IClassificationType _classification;
		private ClassificationTag _classificationTag;
		private bool _isDirty;

		public Filter Filter { get; set; }

		public ColorHCL Color { get; set; }

		public IClassificationType Classification {
			get { return _classification; }
			set {
				if(value != null && _classification != value) {
					_classification = value;
					_classificationTag = new ClassificationTag(_classification);
					_isDirty = true;
				}
			}
		}
		public ClassificationTag ClassificationTag { get { return _classificationTag; } }

		public string Text { get { return _text; } }

		public bool IsDirty { get { return _isDirty; } set { _isDirty = value; } }

		public ColorizedIdentifier(string text) {
			_text = text;
		}


		//Dictionary key. The comparer compares just the text
	}
}