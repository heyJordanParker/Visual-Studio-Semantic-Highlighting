using System;
using Colorspace;
using NUnit.Framework;
using SemanticCodeHighlighting.Colorization;

namespace SemanticCodeHighlighting.Tests {
	[TestFixture]
	public class ColorHCLTests {

		public const double Epsilon = 0.01;

		[Test]
		public void GenerateFromLabTest() {

			//#4BC8E6
			ColorLAB lab = new ColorLAB(1, 75, -25, -25);
			var hcl = new ColorHCL(lab);

			Console.WriteLine(lab);
			Console.WriteLine(hcl);


			Assert.Less(lab.Alpha - hcl.Alpha, Epsilon);
			Assert.Less(hcl.H - 225, Epsilon);
			Assert.Less(hcl.C - 35.355, Epsilon);
			Assert.Less(hcl.L - 75, Epsilon);

		}

		[Test]
		public void LabToHCLToLab() {
			var lab = new ColorLAB(1, 50, 10, 10);
			var hcl = new ColorHCL(lab);
			var hclToLab = hcl.ToLab();
			Assert.Less(lab.Alpha - hclToLab.Alpha, Epsilon);
			Assert.Less(lab.L - hclToLab.L, Epsilon);
			Assert.Less(lab.A - hclToLab.A, Epsilon);
			Assert.Less(lab.B - hclToLab.B, Epsilon);
		}

	}
}