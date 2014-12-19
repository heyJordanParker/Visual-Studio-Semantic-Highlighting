using System.Windows.Media;
using Colorspace;

namespace SemanticCodeHighlighting {
	public class Colorizer {

		private void CreateUniqueClassificationTypeForColor(string variableName) {
			//			_typeRegistry.CreateTransientClassificationType(_baseClassificationType)
			//			set the color of the classification type
			//			link Classification Type and variableName


			ColorLAB lab = new ColorLAB(1, 70, -53, 32);
			
			//r = 61.91
			//-58.8794102 

			
			// take into account prefixes, prioritize capital letters when parsing
			// a prefix, a lowercase first letter or a higher case first letter could introduce variation to the saturation and lightness
		}

	}
}