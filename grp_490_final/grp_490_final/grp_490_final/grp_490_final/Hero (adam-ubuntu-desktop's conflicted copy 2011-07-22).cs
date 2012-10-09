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
	class Hero : XNACS1Rectangle {

		public int lives = 4;
		Vector2 InitialPos;

		public Hero(Vector2 center, int difficulty) {
			Center = center;
			Width = 2f;
			Height = 5f;
			InitialPos = center;
			Texture = "pennyRight";

			switch (difficulty) {
				case 1:
					lives = 5;
					break;
				case 2:
					lives = 4;
					break;
				case 3:
					lives = 3;
					break;
				default:
					lives = 4;
					break;
			}

		}
		public void UpdateHero(Vector2 move) {
			Center += move;

			if (move.X > 0)
				Texture = "pennyRight";
			else if (move.X < 0)
				Texture = "pennyLeft";
		}

		public void UpdateCollisionWithWall(XNACS1Rectangle wall) {
			Vector2 mVectorV;
			Vector2 TangentVector;

			float DistOnTangent;
			Vector2 PtOnTangentLine;
			Vector2 NormalVector;
			float DistOnNormal;
			Vector2 mPtOnNormal;
			float mDistOnTangern;

			if (Collided(wall)) {

				mVectorV = Center - wall.Center;    // V vector(vector from hero center to wall center)

				TangentVector = wall.FrontDirection;  //  tangent vector 
				TangentVector.Normalize();
				DistOnTangent = Vector2.Dot(mVectorV, TangentVector);
				PtOnTangentLine = wall.Center + (DistOnTangent * TangentVector);
				NormalVector = mVectorV - (DistOnTangent * TangentVector);
				NormalVector.Normalize();
				DistOnNormal = Vector2.Dot(mVectorV, NormalVector);


				mPtOnNormal = wall.Center + (DistOnNormal * NormalVector);
				mDistOnTangern = Vector2.Dot(mVectorV, TangentVector);

				if (Math.Abs(DistOnTangent) < wall.Width / 2f) // collided with top and bottom 
				{
					float dOnN = Height / 2 + (wall.Height / 2f);
					if (Math.Abs(DistOnNormal) < dOnN)
						Center = PtOnTangentLine + Math.Sign(DistOnNormal) * (dOnN * NormalVector);
				}
				else {// collide with left and right 

					if (Math.Abs(mDistOnTangern) < Width / 2 + (wall.Width / 2f))
						Center = mPtOnNormal + Math.Sign(mDistOnTangern) * (Width / 2 + wall.Width / 2f) * TangentVector;

				}
			}

		}

		public void Caught() { lives--; }

		public int NumTimesCaught {
			get {
				if (lives < 0)
					lives = 0;
				return lives;
			}

		}

		public Vector2 HeroInitialPos {
			set {
				InitialPos = value;
			}
			get {
				return InitialPos;
			}

		}

	}
}
