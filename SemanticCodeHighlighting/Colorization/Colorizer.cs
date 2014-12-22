using Microsoft.VisualStudio.Text.Tagging;

namespace SemanticCodeHighlighting.Colorization {
	public class Colorizer {
		private const double BaseLighting = 0.4;

		//cache Colorization instances to speed up further lookups

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