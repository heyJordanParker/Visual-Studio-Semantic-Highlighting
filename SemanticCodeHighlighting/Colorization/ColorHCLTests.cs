using System;
using Colorspace;
using NUnit.Framework;

namespace SemanticCodeHighlighting.Colorization {
	[TestFixture]
	public class ColorHCLTests {

		[Test]
		public void GenerateFromLabTest() {

			//hcl(239, 19.40%, 79.14%)
			//lab(79, -10, -16)
			RGBWorkingSpaces rgbWorkingSpaces = new RGBWorkingSpaces();
			var workingSpace = rgbWorkingSpaces.SRGB_D65_Degree2;
			ColorLAB labColor = new ColorLAB(new ColorXYZ(new ColorRGB(new ColorRGB32Bit(155, 203, 226)), workingSpace),
				workingSpace);
			Console.WriteLine(labColor);

			var colorHCL = new ColorHCL(labColor);
			Console.WriteLine(colorHCL);
		}

	}
}