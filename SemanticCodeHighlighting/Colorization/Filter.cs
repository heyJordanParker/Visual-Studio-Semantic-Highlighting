using System.Text.RegularExpressions;

namespace SemanticCodeHighlighting.Colorization {
	public class Filter {
		//TODO filters should have associated formatting rules accompanying them
		private readonly string _prefix;
		private readonly string _condition;

		public string Condition { get { return _condition; } }

		public Filter(string prefix, string condition) {
			_prefix = prefix;
			_condition = condition;
		}

		public static bool HasPrefix(string text, Filter filter) {
			return Regex.IsMatch(text, filter.Condition);
		}

		public override string ToString() {
			return _prefix;
		}

		public override bool Equals(object obj) {
			if(ReferenceEquals(null, obj)) return false;
			return obj is Filter && Equals((Filter) obj);
		}

		public bool Equals(Filter other) {
			return string.Equals(_prefix, other._prefix) && string.Equals(_condition, other._condition);
		}

		public override int GetHashCode() {
			unchecked {
				return ((_prefix != null ? _prefix.GetHashCode() : 0)*397) ^ (_condition != null ? _condition.GetHashCode() : 0);
			}
		}
	}
}