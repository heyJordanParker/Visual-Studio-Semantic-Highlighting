using System.Collections.Generic;
using Microsoft.VisualStudio.Text.Tagging;

namespace SemanticCodeHighlighting.Colorization {
	public class Colorizer {
		private const double BaseLighting = 0.4;

		//cache TextColor instances to speed up further lookups

		private readonly Dictionary<string, TextColor> _colorizerCache;

		public TextColor this[string key] {
			get { return _colorizerCache[key]; }
		}

		public Colorizer() {
			_colorizerCache = new Dictionary<string, TextColor>();
		}

		//rename this method. Logically integrate it
		public IEnumerable<TextColor> Parse(params string[] colorizationStrings) {
			var result = new List<TextColor>();
			foreach(var text in colorizationStrings) {
				if(!_colorizerCache.ContainsKey(text)) {
					TextColor textColor = new TextColor(text);
					_colorizerCache.Add(text, textColor);
				}
				result.Add(_colorizerCache[text]);
			}
			return result;
		}


		private void CreateUniqueClassificationTypeForColor(string variableName) {
			//			_typeRegistry.CreateTransientClassificationType(_baseClassificationType)
			//			set the color of the classification type
			//			link Classification Type and variableName


			

			
			// take into account prefixes, prioritize capital letters when parsing
			// a prefix, a lowercase first letter or a higher case first letter could introduce variation to the saturation and lightness
		}

		public ColorizedSpan[] GetColorizedSpans(IMappingTagSpan<IClassificationTag>[] tags) {
			return null;
		}
	}
}