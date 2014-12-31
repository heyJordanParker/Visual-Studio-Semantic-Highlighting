using System;
using System.Collections;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;

namespace SemanticCodeHighlighting.Colorization {
	internal class ColorizedIdentifier {
		private readonly string _text;
		public Prefix Prefix { get; set; }

		public ColorHCL Color { get; set; }

		public IClassificationType Classification { get; set; }

		public string Text { get { return _text; } }

		public ColorizedIdentifier(string text) {
			_text = text;
		}


		//Dictionary key. The comparer compares just the text
	}
}