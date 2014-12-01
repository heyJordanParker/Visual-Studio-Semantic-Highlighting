using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;

namespace SemanticCodeHighlighting {

	class FormatMapWatcher {
		private readonly IClassificationFormatMap _formatMap;
		private IClassificationType _baseClassificationType;
		private IClassificationTypeRegistryService _typeRegistry;
		private Colorizer _colorizer;
		private bool _updating;

		public FormatMapWatcher(IWpfTextView textView, IClassificationFormatMap formatMap, IClassificationTypeRegistryService typeRegistry) {
			_formatMap = formatMap;
			_typeRegistry = typeRegistry;
			_baseClassificationType = typeRegistry.GetClassificationType("SemanticCodeHighlighting");
			_colorizer = textView.Properties.GetOrCreateSingletonProperty(() => new Colorizer());

			_formatMap.ClassificationFormatMappingChanged += FormatMapChanged;
			textView.GotAggregateFocus += OnFirstFocus;
		}

		private void OnFirstFocus(object textView, EventArgs e) {
			((ITextView) textView).GotAggregateFocus -= OnFirstFocus;

			Colorize();
		}

		private void FormatMapChanged(object sender, EventArgs eventArgs) {
			if(!_updating) {
				Colorize();
			}

		}

		//TODO move to Colorizer
		private void Colorize() {
			try {
				_updating = true;

				var classificationFiler = _formatMap.CurrentPriorityOrder.Where(c => c != null && c.Classification.ToLowerInvariant().Contains("variable"));
				foreach(var classification in classificationFiler) {
					Bold(classification);
				}
			} finally {
				_updating = false;
			}
		}

		private void Bold(IClassificationType classification) {
			var textFormat = _formatMap.GetTextProperties(_typeRegistry.GetClassificationType("text"));
			var properties = _formatMap.GetTextProperties(classification);
			var typeface = properties.Typeface;

			var boldedTypeface = new Typeface(typeface.FontFamily, typeface.Style, FontWeights.Bold, typeface.Stretch);
			var biggerSize = textFormat.FontRenderingEmSize + 2;

			properties = properties.SetTypeface(boldedTypeface);
			properties = properties.SetFontRenderingEmSize(biggerSize);

			_formatMap.SetTextProperties(classification, properties);
		}
	}
}
