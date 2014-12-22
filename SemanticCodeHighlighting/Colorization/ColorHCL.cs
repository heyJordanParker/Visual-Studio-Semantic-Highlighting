using System;
using System.Globalization;
using Colorspace;

namespace SemanticCodeHighlighting.Colorization {
	public struct ColorHCL {

		public const double PI = Math.PI;

		private readonly double _alpha;
		private readonly double _h;
		private readonly double _c;
		private readonly double _l;

		public double Alpha { get { return _alpha; } }
		public double H { get { return _h; } }
		public double C { get { return _c; } }
		public double L { get { return _l; } }

		public ColorHCL(double h, double c, double l, double alpha = 1.0) {
			_h = h;
			_c = c;
			_l = l;
			_alpha = alpha;
		}

		public ColorHCL(ColorLAB labColor) {
			var tan = Math.Atan2(labColor.B, labColor.A);
			_h = tan > 0 ? (tan/PI)*180 : 360 - (Math.Abs(tan)/PI)*180;
			_c = Math.Sqrt(labColor.A*labColor.A + labColor.B*labColor.B);
			_l = labColor.L;
			_alpha = labColor.Alpha;
		}


		public override string ToString() {
			return string.Format(CultureInfo.InvariantCulture, "{0}({1:0.0##},{2:0.0##},{3:0.0##},{4:0.0##})", (object)GetType().Name, (object)_alpha, (object)_h, (object)_c, (object)_l);
		}
	}
}