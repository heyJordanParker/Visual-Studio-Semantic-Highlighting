namespace SemanticCodeHighlighting {
	public class ColoredIdentifier {

		// Those classifiers will be prioritized and text colorization will be based on all of them. 
		// Higher priority classifiers will have greater effect than lower priority ones
		#region Classifiers

		// pick from a list of prefixes { m_, _, I etc } if not found - use the first letter
		public string Prefix { get; set; }

		// Uppercase Letters all capital letters extracted, keeping the order intact
		public string UppercaseLetters { get; set; }

		// Lowercase Letters all capital letters extracted, keeping the order intact
		public string LowercaseLetters { get; set; }

		// Length of the text
		public int Length { get; set; }

		#endregion

		// The text itself
		public string Text { get; set; }

	}
}