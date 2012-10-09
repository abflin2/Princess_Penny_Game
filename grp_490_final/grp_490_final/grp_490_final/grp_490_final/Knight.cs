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
namespace grp_490_final
{
    class Knight : Enemy
    {
		protected const int HEIGHT = 6;
        protected const int WIDTH = 4;
        

        public Knight(Vector2 center): base (center)
        {
            Width = WIDTH;
            Height = HEIGHT;
            Xchange = .1f;
            Ychange = .1f;
            ChaserCount = 660;
			lives = 7;
            Texture = "Knight";

        }
    }
}
