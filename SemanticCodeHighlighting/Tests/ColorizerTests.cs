using System;
using NUnit.Framework;
using SemanticCodeHighlighting.Colorization;

namespace SemanticCodeHighlighting.Tests {
	[TestFixture]
	public class ColorizerTests {

		[Test]
		public void ColorizationTest() {
			string text = "_Test";

			var colorization = new TextColor(text);

			Assert.AreEqual(colorization.Length, text.Length);
			Assert.AreEqual(colorization.LowercaseLetters, "est");
			Assert.IsTrue(String.Equals(colorization.Prefix.ToString(), "_", StringComparison.InvariantCulture));
			Assert.AreEqual(colorization.UppercaseLetters, "T");

			text = "Test";
			colorization = new TextColor(text);
			Assert.IsTrue(String.IsNullOrEmpty(colorization.Prefix.ToString()));

			text = "ITest";
			colorization = new TextColor(text);
			Assert.IsTrue(String.Equals(colorization.Prefix.ToString(), "I", StringComparison.InvariantCulture));

			text = "Item";
			colorization = new TextColor(text);
			Assert.IsTrue(String.Equals(colorization.Prefix.ToString(), "", StringComparison.InvariantCulture));

		}

		[Test]
		public void PrefixTest() {
			Prefix prefix = new Prefix("_");
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
			prefix = new Prefix("I", "I[A-Z]");
			Assert.IsFalse(Prefix.HasPrefix(text, prefix));

			text = "Item";
			Assert.IsFalse(Prefix.HasPrefix(text, prefix));

			text = "IItem";
			Assert.IsTrue(Prefix.HasPrefix(text, prefix));


		}


	}
}