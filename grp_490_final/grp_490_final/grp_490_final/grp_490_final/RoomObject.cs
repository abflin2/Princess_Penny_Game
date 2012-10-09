#region Reference to system libraries
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

#endregion
using XNACS1Lib;
namespace grp_490_final {

	class RoomObject : XNACS1Rectangle {

		public RoomObject(float height, float width, Vector2 center, string texture) {
			Height = height;
			Width = width;
			Texture = texture;
			Center = center;
		}

		public void UpdateWallCollisionWithHero(Hero hero) {
			hero.UpdateCollisionWithObject(this);
		}
		public void UpdateWallCollisionWithEnemy(Enemy enemy) {
			enemy.UpdateCollisionWithObject(this);
		}
	}
}
