using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using NUnit.Framework;

namespace SemanticCodeHighlighting.Colorization {
	public class Colorizer {
		private static readonly Filter[] DefaultFilters = {
			new Filter("_Member", "^_"),   
			new Filter("m_Member", "^m_"), 
			new Filter("mMember", "^m[A-Z]"), 
			new Filter("Verbatim", "^@"),
			new Filter("Interface", "^I[A-Z]"),
			new Filter("lowercase", "^[a-z]"),
			new Filter("Uppercase", "^[A-Z]"),
		};

		public ColorizedIdentifier this[string text] {
			get {
				if(!_colorizerCache.ContainsKey(text))
					_colorizerCache.Add(text, GenerateIdentifier(text));
				return _colorizerCache[text];
			}
		}
	
		public const string ClassificationPrefix = "SemanticCodeHighlighting.ColorizerClassification.";

		private readonly IClassificationTypeRegistryService _typeRegistry;
		private readonly Dictionary<string, ColorizedIdentifier> _colorizerCache;
		private readonly IClassificationType _baseClassification;

		private readonly Random _random;

		private bool _updating;

		public Colorizer(IClassificationTypeRegistryService typeRegistry) {
			_colorizerCache = new Dictionary<string, ColorizedIdentifier>();
			_typeRegistry = typeRegistry;
			_baseClassification = _typeRegistry.GetClassificationType("identifier");
			_random = new Random();
		}

		private ColorizedIdentifier GenerateIdentifier(string text) {
			var identifier = new ColorizedIdentifier(text);
			Filter filter = GetPrefix(identifier.Text);

			//TODO Set default Filter
			Assert.NotNull(filter);
			identifier.Filter = filter;

			identifier.Color = CreateColor(identifier);

			var classificationName = ClassificationPrefix + identifier.Text;
			IClassificationType classification;
			if(_typeRegistry.GetClassificationType(classificationName) != null) {
				classification = _typeRegistry.GetClassificationType(classificationName);
				identifier.IsDirty = false;
			} else {
				classification = _typeRegistry.CreateClassificationType(classificationName, new[] { _baseClassification });
				identifier.IsDirty = true;
			}

			identifier.Classification = classification;
			return identifier;
		}

		private Filter GetPrefix(string text) {
			return DefaultFilters.FirstOrDefault(prefix => Filter.HasPrefix(text, prefix));
		}

		public void UpdateClassifications(IClassificationFormatMap formatMap) {
			if(_updating)
				return;
			try {
				_updating = true;
//				formatMap.BeginBatchUpdate();
				foreach(var identifier in _colorizerCache.Values.Where(i => i.IsDirty)) {
					var textProperties = formatMap.GetTextProperties(identifier.Classification);
					textProperties = textProperties.SetForeground(identifier.Color.ToColor());
					formatMap.SetTextProperties(identifier.Classification, textProperties);
					identifier.IsDirty = false;
				}
			} finally {
//				formatMap.EndBatchUpdate();
				_updating = false;
			}
		}


		private ColorHCL CreateColor(ColorizedIdentifier identifier) {
			//TODO color generation
			return new ColorHCL(_random.Next(360), 25, 61);
			// take into account prefixes, prioritize capital letters when generating
			// a Filter, a lowercase first letter or a higher case first letter could introduce variation to the saturation and lightness
		}

		public ColorizedIdentifier GetColorization(string text) {
			return this[text];
		}
	}
}