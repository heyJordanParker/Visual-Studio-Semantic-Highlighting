using System;
using System.Collections;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;

namespace SemanticCodeHighlighting.Colorization {
	internal class ColorizedIdentifier {
		private readonly string _text;
		private IClassificationType _classification;
		private bool _isDirty = false;

		public Prefix Prefix { get; set; }

		public ColorHCL Color { get; set; }

		public IClassificationType Classification {
			get { return _classification; }
			set {
				if(value != null && _classification != value) {
					_classification = value;
					_isDirty = true;
				}
			}
		}

		public string Text { get { return _text; } }

		public bool IsDirty { get { return _isDirty; } set { _isDirty = value; } }

		public ColorizedIdentifier(string text) {
			_text = text;
		}


		//Dictionary key. The comparer compares just the text
	}
}