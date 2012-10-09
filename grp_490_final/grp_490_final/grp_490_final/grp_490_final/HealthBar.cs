using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XNACS1Lib;

namespace grp_490_final {
	class HealthBar {
		XNACS1Rectangle green;
		XNACS1Rectangle red;

		public HealthBar(Vector2 Center) {
			red = new XNACS1Rectangle(new Vector2(Center.X, Center.Y + 3), 3, 0.5f);
			red.Color = Color.Red;
			green = new XNACS1Rectangle(new Vector2(Center.X, Center.Y + 3), 3, 0.5f);
			green.Color = Color.Green;
		}

		public void Update(Vector2 Center, int health, int maxHealth) {
			if (red != null && green != null) {
				red.RemoveFromAutoDrawSet();
				green.RemoveFromAutoDrawSet();
				red = null;
				green = null;
			}

			if(health >= (maxHealth * .7)) {
				red = new XNACS1Rectangle(new Vector2(Center.X, Center.Y + 3), 3, 0.5f);
				red.Color = Color.Red;
				green = new XNACS1Rectangle(new Vector2(Center.X, Center.Y + 3), 3, 0.5f);
				green.Color = Color.Green;
			}
			else if(health >= (maxHealth * .4)) {
				red = new XNACS1Rectangle(new Vector2(Center.X, Center.Y + 3), 3, 0.5f);
				red.Color = Color.Red;
				green = new XNACS1Rectangle(new Vector2(Center.X - 1, Center.Y + 3), 2, 0.5f);
				green.Color = Color.Green;
			}
			else if(health > 0){
				red = new XNACS1Rectangle(new Vector2(Center.X, Center.Y + 3), 3, 0.5f);
				red.Color = Color.Red;
				green = new XNACS1Rectangle(new Vector2(Center.X - 2, Center.Y + 3), 1, 0.5f);
				green.Color = Color.Green;
			}
			
		}
	}
}
