using Microsoft.VisualStudio.Text;

namespace SemanticCodeHighlighting.Colorization {
	public struct Colorization {

		//_lower _count
		//m_ m_count or m_Count
		//mUpper mCount
		//IUpper IShip vs Internationalization 
		private static readonly string[] Prefixes = { "_", "m_", "I" };

		// Those classifiers will be prioritized and text colorization will be based on all of them. 
		// Higher priority classifiers will have greater effect than lower priority ones

		// pick from a list of prefixes { m_, _, I etc } if not found - use the first letter
		public string Prefix { get; set; }

		// Uppercase Letters all capital letters extracted, keeping the order intact
		public string UppercaseLetters { get; set; }

		// Lowercase Letters all capital letters extracted, keeping the order intact
		public string LowercaseLetters { get; set; }

		// Length of the text
		public int Length { get; set; }

		// The text itself
		public string Text { get; set; }


		//Dictionary key. The comparer compares just the text
	}
}