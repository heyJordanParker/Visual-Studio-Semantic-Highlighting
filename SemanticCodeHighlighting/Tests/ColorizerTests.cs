using System;
using Colorspace;
using NUnit.Framework;
using SemanticCodeHighlighting.Colorization;

namespace SemanticCodeHighlighting.Tests {
	[TestFixture]
	public class ColorizerTests {

		[Test]
		public void ColorizationTest() {

			var workingspace = new RGBWorkingSpaces();

			var hclColor = new ColorHCL(320, 3, 1);
			var lab = hclColor.ToLab();

			ColorRGB rgb = new ColorRGB(new ColorXYZ(lab, workingspace.SRGB_D65_Degree2), workingspace.SRGB_D65_Degree2);
			Console.WriteLine(rgb.ToString());
		}

		[Test]
		public void FilterTest() {
			Filter filter = new Filter("_", "_");
			string text = "text";

			Assert.IsFalse(Filter.HasPrefix(text, filter));

			text = "_Text";
			Assert.IsTrue(Filter.HasPrefix(text, filter));

			//with start of string regex
			filter = new Filter("I", "^I[A-Z]");
			Assert.IsFalse(Filter.HasPrefix(text, filter));

			text = "Item";
			Assert.IsFalse(Filter.HasPrefix(text, filter));

			text = "IItem";
			Assert.IsTrue(Filter.HasPrefix(text, filter));

			//skipped start of string regex. Should be auto added
			text = "_Text";
			filter = new Filter("m", "m[A-Z]");
			Assert.IsFalse(Filter.HasPrefix(text, filter));

			text = "Item";
			Assert.IsFalse(Filter.HasPrefix(text, filter));

			text = "mItem";
			Assert.IsTrue(Filter.HasPrefix(text, filter));


		}


	}
}