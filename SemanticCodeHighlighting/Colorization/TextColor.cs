using System.Text.RegularExpressions;
using Microsoft.VisualStudio.Text;

namespace SemanticCodeHighlighting.Colorization {
	public struct TextColor {

		// Those classifiers will be prioritized and text colorization will be based on all of them. 
		// Higher priority classifiers will have greater effect than lower priority ones


		//_lower _count
		//m_ m_count or m_Count
		//mUpper mCount
		//IUpper IShip vs Internationalization 
		private static readonly string[] Prefixes = { "_", "m_", "I" };
		private readonly string _prefix;
		private readonly string _uppercaseLetters;
		private readonly string _lowercaseLetters;
		private readonly int _length;
		private readonly string _text;

		// pick from a list of prefixes { m_, _, I etc } if not found - use the first letter
		public string Prefix { get { return _prefix; } }

		// Uppercase Letters all capital letters extracted, keeping the order intact
		public string UppercaseLetters { get { return _uppercaseLetters; } }

		// Lowercase Letters all capital letters extracted, keeping the order intact
		public string LowercaseLetters { get { return _lowercaseLetters; } }

		// Length of the text
		public int Length { get { return _length; } }

		// The text itself
		public string Text { get { return _text; } }

		public TextColor(string text) {
			_text = text;

			_prefix = "";
			foreach(var prefix in Prefixes) {
				if(!Regex.IsMatch(text, '^' + prefix)) continue;
				_prefix = prefix;
				break;
			}

			_uppercaseLetters = Regex.Replace(text, "[^A-Z]", "");
			_lowercaseLetters = Regex.Replace(text, "[^a-z]", "");

			_length = _text.Length;

			//PARSE
		}


		//Dictionary key. The comparer compares just the text
	}
}