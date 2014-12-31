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
		public void PrefixTest() {
			Prefix prefix = new Prefix("_", "_");
			string text = "text";

			Assert.IsFalse(Prefix.HasPrefix(text, prefix));

			text = "_Text";
			Assert.IsTrue(Prefix.HasPrefix(text, prefix));

			//with start of string regex
			prefix = new Prefix("I", "^I[A-Z]");
			Assert.IsFalse(Prefix.HasPrefix(text, prefix));

			text = "Item";
			Assert.IsFalse(Prefix.HasPrefix(text, prefix));

			text = "IItem";
			Assert.IsTrue(Prefix.HasPrefix(text, prefix));

			//skipped start of string regex. Should be auto added
			text = "_Text";
			prefix = new Prefix("m", "m[A-Z]");
			Assert.IsFalse(Prefix.HasPrefix(text, prefix));

			text = "Item";
			Assert.IsFalse(Prefix.HasPrefix(text, prefix));

			text = "mItem";
			Assert.IsTrue(Prefix.HasPrefix(text, prefix));


		}


	}
}