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
	class Wall : XNACS1Rectangle
	{

		public Wall(Vector2 origin, float width, float height)
		{

			Center = origin;
			Width = width;
			Height = height;
            
		}

	}
}
