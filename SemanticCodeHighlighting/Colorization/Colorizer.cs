using System.Collections.Generic;
using Microsoft.VisualStudio.Text.Tagging;

namespace SemanticCodeHighlighting.Colorization {
	public class Colorizer {
		private const double BaseLighting = 0.4;

		//cache Colorization instances to speed up further lookups

		private readonly Dictionary<string, Colorization> _colorizerCache;

		public Colorization this[string key] {
			get { return _colorizerCache[key]; }
		}

		public Colorizer() {
			_colorizerCache = new Dictionary<string, Colorization>();
		}

		//rename this method. Logically integrate it
		public IEnumerable<Colorization> Parse(params string[] colorizationStrings) {
			var result = new List<Colorization>();
			foreach(var text in colorizationStrings) {
				if(!_colorizerCache.ContainsKey(text)) {
					Colorization colorization = new Colorization(text);
					_colorizerCache.Add(text, colorization);
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