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
	class Guard : Enemy {

		private const int HEIGHT = 5;
		private const int WIDTH = 3;

		public Guard(Vector2 center): base(center) {
			Width = WIDTH;
			Height = HEIGHT;
			InitialPos = center;
			Texture = "Guard";
			CurrentState = EnemyState.Patrol;
			ChaserCount = 500;
			Xchange = .2f;
			Ychange = .2f;
			lives = 3;

		}

	}
}