using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using NUnit.Framework;

namespace SemanticCodeHighlighting.Colorization {
	public class Colorizer{
		private static readonly Prefix[] DefaultPrefixes = {
			new Prefix("_", "^_"),   
			new Prefix("m_", "^m_"), 
			new Prefix("m", "^m[A-Z]"), 
			new Prefix("", "^[a-z]"),
			new Prefix("I", "^I[A-Z]"),
			new Prefix("", "^[A-Z]"),
			new Prefix("@", "^@"),
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

		private bool _updating;

		public Colorizer(IClassificationTypeRegistryService typeRegistry) {
			_colorizerCache = new Dictionary<string, ColorizedIdentifier>();
			_typeRegistry = typeRegistry;
			_baseClassification = _typeRegistry.GetClassificationType("identifier");
		}

		private ColorizedIdentifier GenerateIdentifier(string text) {
			var identifier = new ColorizedIdentifier(text);
			Prefix prefix = GetPrefix(identifier.Text);

			identifier.Prefix = prefix;

			identifier.Color = GenerateColor(identifier);

			var classificationName = ClassificationPrefix + identifier.Text;
			IClassificationType classification;
			if(_typeRegistry.GetClassificationType(classificationName) != null) {
				classification = _typeRegistry.GetClassificationType(classificationName);
			} else {
				classification = _typeRegistry.CreateClassificationType(classificationName, new[] { _baseClassification });
				identifier.IsDirty = true;
			}

			identifier.Classification = classification;
			return identifier;
		}

		internal Prefix GetPrefix(string text) {
			return DefaultPrefixes.FirstOrDefault(prefix => Prefix.HasPrefix(text, prefix));
		}

		public void UpdateClassifications(IClassificationFormatMap formatMap) {
			if(_updating || formatMap.IsInBatchUpdate)
				return;
			try {
				_updating = true;
				foreach(var identifier in _colorizerCache.Values) {
					if(!identifier.IsDirty) continue;
					if(!formatMap.IsInBatchUpdate)
						formatMap.BeginBatchUpdate();

					var textProperties = formatMap.GetTextProperties(identifier.Classification);
					textProperties = textProperties.SetForeground(identifier.Color.ToColor());
					formatMap.SetTextProperties(identifier.Classification, textProperties);
					identifier.IsDirty = false;
				}
			} finally {
				if(formatMap.IsInBatchUpdate)
					formatMap.EndBatchUpdate();
				_updating = false;
			}
		}


		private ColorHCL GenerateColor(ColorizedIdentifier identifier) {
			var text = identifier.Text;
			text = Regex.Replace(text, identifier.Prefix.ToString(), "");

			int chroma = 25;
			int luminance = 50;
			int[] hueLimit = { 0, 360 };

			var prefixIndex = Array.IndexOf(DefaultPrefixes, identifier.Prefix);
			switch(prefixIndex) {
				case 4: //interface
					hueLimit[0] = 100;
					hueLimit[1] = 150;
					break;
				case 5: //public
					hueLimit[0] = 150;
					hueLimit[1] = 280;
					if(String.IsNullOrEmpty(Regex.Replace(text, "[^a-z]", ""))) 
						Console.WriteLine("ALL CAPS");
					break;
				case 6: //@
					hueLimit[0] = 320;
					hueLimit[1] = 340;
					break;
				default: //private
					chroma = 20;
					luminance = 66;
					break;
			}

			var firstLetter = text[0];
			text = text.Remove(0, 1);

			const int lowercaseCharacters = 4;
			const int uppercaseCharacters = 3;

			var uppercase = Regex.Replace(text, "[^A-Z]","");
			var lowercase = Regex.Replace(text, "[^a-z]","");

			if(lowercase.Length < lowercaseCharacters) {
				lowercase = String.Concat(lowercase, new string('a', lowercaseCharacters - lowercase.Length));
			}
			if(uppercase.Length < uppercaseCharacters) {
				uppercase = String.Concat(uppercase, new string('A', uppercaseCharacters - uppercase.Length));
			}

			double huePercentage = 0;
			double hueStep = 16.67; //6 steps

			if(!Char.IsLower(firstLetter))
				huePercentage += hueStep;
			huePercentage += hueStep*2*LetterToPercentage(firstLetter);

			double lowercasePercentage = 0;
			for(int i = 0; i < lowercaseCharacters; ++i) {
				lowercasePercentage += LetterToPercentage(lowercase[i]);
			}
			lowercasePercentage = lowercasePercentage/lowercaseCharacters;

			huePercentage += hueStep*2*lowercasePercentage;

			double uppercasePercentage = 0;
			for(int i = 0; i < uppercaseCharacters; ++i) {
				uppercasePercentage += LetterToPercentage(uppercase[i]);
			}
			uppercasePercentage = uppercasePercentage / uppercaseCharacters;

			huePercentage += hueStep*uppercasePercentage;

			if(huePercentage > 100) huePercentage = 100;
			huePercentage /= 100.0;

			return new ColorHCL(hueLimit[0] + huePercentage * (hueLimit[1] - hueLimit[0]), chroma, luminance);
			// take into account prefixes, prioritize capital letters when generating
			// a Prefix, a lowercase first letter or a higher case first letter could introduce variation to the saturation and lightness
		}

		[DebuggerStepThrough]
		private double LetterToPercentage(char letter) {
			const byte a = (byte) 'a';
			const byte z = (byte) 'z';
			const byte A = (byte) 'A';
			const byte Z = (byte) 'Z';

			byte letterByte = (byte) letter;

			if(letter >= a && letter <= z)
				return (letter - a) / 25.0;
			if(letter >= A && letter <= Z)
				return (letter - A) / 25.0;
			return 0;
		}

		public ColorizedIdentifier GetColorization(string text) {
			return this[text];
		}
	}
}