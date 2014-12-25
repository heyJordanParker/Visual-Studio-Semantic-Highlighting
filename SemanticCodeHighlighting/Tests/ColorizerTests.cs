using NUnit.Framework;
using SemanticCodeHighlighting.Colorization;

namespace SemanticCodeHighlighting.Tests {
	[TestFixture]
	public class ColorizerTests {

		[Test]
		public void ColorizationTest() {
			var text = "_Test";

			var colorization = new TextColor(text);

			Assert.AreEqual(colorization.Length, text.Length);
			Assert.AreEqual(colorization.LowercaseLetters, "est");
			Assert.AreEqual(colorization.Prefix, "_");
			Assert.AreEqual(colorization.UppercaseLetters, "T");
		}


	}
}