using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XNACS1Lib;

namespace grp_490_final {
	class Stair:XNACS1Rectangle {
		public bool available;
        public Stair(Vector2 center) {
            Center = center;
			CenterX -= 1.5f;
			CenterY += 3;
            Height = 4f;
            Width = 4f;
			this.Texture = "newStair";
			available = false;

		}


	}
}