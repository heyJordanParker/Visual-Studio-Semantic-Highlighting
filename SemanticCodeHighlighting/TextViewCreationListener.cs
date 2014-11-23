using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace SemanticCodeHighlighting {
	[Export(typeof(IWpfTextViewCreationListener))]
	[ContentType("code")]
	[TextViewRole(PredefinedTextViewRoles.Editable)] //Editable for now
	public class TextViewCreationListener : IWpfTextViewCreationListener {

		public void TextViewCreated(IWpfTextView textView) {
			


		}
	}
}