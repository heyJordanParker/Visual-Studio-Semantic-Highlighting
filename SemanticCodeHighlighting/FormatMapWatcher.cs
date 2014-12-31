using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using SemanticCodeHighlighting.Colorization;

namespace SemanticCodeHighlighting {

	class FormatMapWatcher {
		public class Couple {
			public IClassificationType classification;
			public ColorHCL color;

			public Couple(IClassificationType classification, ColorHCL color) {
				this.classification = classification;
				this.color = color;
			}

		}

		private readonly IClassificationFormatMap _formatMap;
		private IClassificationType _baseClassificationType;
		private IClassificationTypeRegistryService _typeRegistry;
		private Colorizer _colorizer;
		private bool _updating;

		private static Queue<Couple> _actions = new Queue<Couple>(); 

		public FormatMapWatcher(IWpfTextView textView, IClassificationFormatMap formatMap, IClassificationTypeRegistryService typeRegistry) {
			_formatMap = formatMap;
			_typeRegistry = typeRegistry;
			_baseClassificationType = typeRegistry.GetClassificationType(Config.ClassificationName);
//			_colorizer = textView.Properties.GetOrCreateSingletonProperty(() => new Colorization.Colorizer(typeRegistry, formatMap));

			

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

		public static void AddItem(Couple action) {
			_actions.Enqueue(action);
		}

		//TODO move to Colorizer
		private void Colorize() {
			try {
				_updating = true;


				while(_actions.Count > 0) {
					var couple = _actions.Dequeue();
					var textProperties = _formatMap.GetTextProperties(couple.classification);
					textProperties.SetForeground(couple.color.ToColor());
					_formatMap.SetTextProperties(couple.classification, textProperties);
				}
//				var classificationTypes = _formatMap.CurrentPriorityOrder;
//				var allFormats = classificationTypes.Aggregate("", (cur, next) => cur + next + ',');
//				var classificationFiler = classificationTypes.Where(c => c != null && c.Classification.ToLowerInvariant().Contains("identifier"));
//				foreach(var classification in classificationFiler) {
//					Bold(classification);
//				}
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
