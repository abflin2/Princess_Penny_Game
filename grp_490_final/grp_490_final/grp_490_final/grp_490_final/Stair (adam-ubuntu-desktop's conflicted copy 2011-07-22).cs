using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XNACS1Lib;

namespace grp_490_final {
	class Stair:XNACS1Rectangle {

        public Stair(Vector2 center) {
            Center = center;
			CenterX -= 1.5f;
			CenterY += 3;
            Height = 12f;
            Width = 6f;
			this.Texture = "stairs";
		}
	}
}