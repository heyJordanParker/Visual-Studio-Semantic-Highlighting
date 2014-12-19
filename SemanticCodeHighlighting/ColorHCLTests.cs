using System;
using Colorspace;
using NUnit.Framework;

namespace SemanticCodeHighlighting {
	[TestFixture]
	public class ColorHCLTests {

		public static double PI = Math.PI;

		[Test]
		public void GenerateFromLabTest() {

			//hcl(239, 19.40%, 79.14%)
			//lab(79, -10, -16)
			RGBWorkingSpaces rgbWorkingSpaces = new RGBWorkingSpaces();
			var workingSpace = rgbWorkingSpaces.SRGB_D65_Degree2;
			ColorLAB labColor = new ColorLAB(new ColorXYZ(new ColorRGB(new ColorRGB32Bit(155, 203, 226)), workingSpace),
				workingSpace);
			Console.WriteLine(labColor);

			var tan = Math.Atan2(labColor.B, labColor.A);
			if(tan > 0) tan = (tan/PI)*180;
			else tan = 360 - (Math.Abs(tan)/PI)*180;

			Console.WriteLine("H : {0:f3}, C : {1:f3}, L : {2:f3}", tan, Math.Sqrt(labColor.A * labColor.A + labColor.B * labColor.B), labColor.L);
		}

	}
}