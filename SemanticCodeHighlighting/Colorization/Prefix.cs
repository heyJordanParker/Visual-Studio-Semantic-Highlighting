using System.Text.RegularExpressions;

namespace SemanticCodeHighlighting.Colorization {
	public struct Prefix {
		public static readonly Prefix None = new Prefix("");

		private readonly string _prefix;
		private readonly string _condition;

		public string Condition { get { return _condition; } }

		public Prefix(string prefix, string condition = "") {
			_prefix = prefix;
			_condition = string.IsNullOrEmpty(condition) ? _prefix : condition;
			if(!_condition.StartsWith("^")) {
				_condition = '^' + _condition;
			}
		}

		public static bool HasPrefix(string text, Prefix prefix) {
			return Regex.IsMatch(text, prefix.Condition);
		}

		public override string ToString() {
			return _prefix;
		}

		public override bool Equals(object obj) {
			if(ReferenceEquals(null, obj)) return false;
			return obj is Prefix && Equals((Prefix) obj);
		}

		public bool Equals(Prefix other) {
			return string.Equals(_prefix, other._prefix) && string.Equals(_condition, other._condition);
		}

		public override int GetHashCode() {
			unchecked {
				return ((_prefix != null ? _prefix.GetHashCode() : 0)*397) ^ (_condition != null ? _condition.GetHashCode() : 0);
			}
		}
	}
}