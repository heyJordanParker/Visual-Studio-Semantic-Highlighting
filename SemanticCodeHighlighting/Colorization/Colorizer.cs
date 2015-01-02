using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using NUnit.Framework;

namespace SemanticCodeHighlighting.Colorization {
	public class Colorizer {
		private static readonly Prefix[] DefaultPrefixes = {
			new Prefix("_Member", "_"),   
			new Prefix("m_Member", "m_"), 
			new Prefix("mMember", "m[A-Z]"), 
			new Prefix("Verbatim", "@"),
			new Prefix("Interface", "I[A-Z]"),
			new Prefix("lowercase", "[a-z]"),
			new Prefix("Uppercase", "[A-Z]"),
		};

		private const double BaseLighting = 0.4;
		private readonly IClassificationTypeRegistryService _typeRegistry;
		private readonly IClassificationFormatMap _formatMap;

		private readonly Dictionary<string, ColorizedIdentifier> _colorizerCache;
		private readonly IClassificationType _baseClassification;

		private readonly Random _random;

		private bool _updating;

		public Colorizer(IClassificationTypeRegistryService typeRegistry, IClassificationFormatMap formatMap) {
			_colorizerCache = new Dictionary<string, ColorizedIdentifier>();
			_typeRegistry = typeRegistry;
			_formatMap = formatMap;
			_baseClassification = _typeRegistry.GetClassificationType("identifier");
			_random = new Random();
		}

		public void GenerateColors(params string[] colorizationStrings) {
			foreach(var text in colorizationStrings) {

				if(_colorizerCache.ContainsKey(text)) continue;

				var identifier = new ColorizedIdentifier(text);
				Prefix prefix = GetPrefix(identifier.Text);
				Assert.NotNull(prefix);
				identifier.Prefix = prefix;

				identifier.Color = CreateColor(identifier);

				var classificationName = '_' + identifier.Text + "Classification";
				var classification = _typeRegistry.GetClassificationType(classificationName) ??
									 _typeRegistry.CreateClassificationType(classificationName, new[] { _baseClassification });

				identifier.Classification = classification;
				_colorizerCache.Add(text, identifier);

			}
		}

		private Prefix GetPrefix(string text) {
			return DefaultPrefixes.FirstOrDefault(prefix => Prefix.HasPrefix(text, prefix));
		}

		public void UpdateClassifications() {
			try {
				_updating = true;
				foreach(var identifier in _colorizerCache.Values.Where(i => i.IsDirty)) {
					var textProperties = _formatMap.GetTextProperties(identifier.Classification);
					textProperties = textProperties.SetForeground(identifier.Color.ToColor());
					_formatMap.SetTextProperties(identifier.Classification, textProperties);
					identifier.IsDirty = false;
				}
			} finally {
				_updating = false;
			}
		}


		private ColorHCL CreateColor(ColorizedIdentifier identifier) {
			return new ColorHCL(_random.Next(360), 25, 61);
			// take into account prefixes, prioritize capital letters when parsing
			// a prefix, a lowercase first letter or a higher case first letter could introduce variation to the saturation and lightness
		}

		//TODO use format map watcher combo
		public IClassificationType GetClassification(string text) {
			ColorizedIdentifier identifier;
			return _colorizerCache.TryGetValue(text, out identifier) ? identifier.Classification : null;
		}
	}
}