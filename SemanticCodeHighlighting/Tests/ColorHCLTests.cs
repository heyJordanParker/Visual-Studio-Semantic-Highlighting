using System;
using Colorspace;
using NUnit.Framework;
using SemanticCodeHighlighting.Colorization;

namespace SemanticCodeHighlighting.Tests {
	[TestFixture]
	public class ColorHCLTests {

		[Test]
		public void RgbConversionTest() {
			RGBWorkingSpaces workingSpaces = new RGBWorkingSpaces();
			ColorRGB rgb = new ColorRGB(0.741176470588235D, 0.513725490196078D, 0.580392156862745D);
			ColorLAB lab = new ColorLAB(new ColorXYZ(rgb, workingSpaces.SRGB_D65_Degree2), workingSpaces.SRGB_D65_Degree2);
			ColorHCL hcl = new ColorHCL(lab);
			Console.WriteLine(hcl);
		}


		[Test]
		public void GenerateFromLabTest() {
			const double epsilon = 0.01;

			//#4BC8E6
			ColorLAB lab = new ColorLAB(1, 75, -25, -25);
			var hcl = new ColorHCL(lab);

			Console.WriteLine(lab);
			Console.WriteLine(hcl);


			Assert.Less(lab.Alpha - hcl.Alpha, epsilon);
			Assert.Less(hcl.H - 225, epsilon);
			Assert.Less(hcl.C - 35.355, epsilon);
			Assert.Less(hcl.L - 75, epsilon);

		}

		[Test]
		public void LabToHCLToLab() {
			const double epsilon = 0.0001;

			var lab = new ColorLAB(1, 50, 10, 10);
			var hcl = new ColorHCL(lab);
			var hclToLab = hcl.ToLab();
			Assert.Less(lab.Alpha - hclToLab.Alpha, epsilon);
			Assert.Less(lab.L - hclToLab.L, epsilon);
			Assert.Less(lab.A - hclToLab.A, epsilon);
			Assert.Less(lab.B - hclToLab.B, epsilon);
		}

		[Test]
		public void ColorToHCLToColor() {
			var ws = new RGBWorkingSpaces();
			ColorRGB rgb = new ColorRGB(1, 0, 0);
			ColorXYZ xyz = new ColorXYZ(rgb, ws.Adobe_D65_Degree2);
			ColorLAB lab = new ColorLAB(xyz, ws.Adobe_D65_Degree2);
			ColorHCL hcl = new ColorHCL(lab);
			Console.WriteLine(hcl.ToColor());
		}

	}
}