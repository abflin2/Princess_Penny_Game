using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XNACS1Lib;

namespace grp_490_final {
	class Key:XNACS1Rectangle {

        public Key(Vector2 center, float width, float height)
        {
			Center = center;
            Height = height;
            Width = width;
           Texture = "key2";
		}

	}
}
