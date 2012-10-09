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
	class Enemy : XNACS1Rectangle {
		bool flagX = false;
		bool flagY = false;
		protected float Xchange;
		protected float Ychange;
		protected float ChaserCount;

		public float difficulty = 1.0f;
		protected int lives;
		public int initialLives;
		public HealthBar health;

		protected const float kDistToBeginChase = 15f;
		protected EnemyState CurrentState;
		protected Vector2 TargetPosition;
        protected Vector2 InitialPos;
		protected enum EnemyState {
			Patrol,
			Chaser,
			Bunny
		}

		public Enemy(Vector2 center) {
			Center = center;
            InitialPos = center;
			CurrentState = EnemyState.Patrol;
			initialLives = 1;
			health = new HealthBar(this.Center);
		}
        
		public  bool UpdateEnemy(Hero hero, WallSet wall) {
			if (CurrentState != EnemyState.Bunny) {
				DetectHero(hero);
				Vector2 toTarget = TargetPosition - Center;
				float distToTarget = toTarget.Length();
				toTarget.Normalize();

				if (CurrentState == EnemyState.Chaser) {
					ChaserCount--;
					if (UpdateEnemyChaseState(hero, distToTarget)) {
						return true;
					}
				}

				else if (CurrentState == EnemyState.Patrol)
					UpdateEnemyPatrolState(wall);

				if (ChaserCount == 0 && distToTarget > kDistToBeginChase) {
					CurrentState = EnemyState.Patrol;
					ChaserCount = 500;
				}
			}

			if (lives > initialLives)
				initialLives = lives;
			health.Update(this.Center, lives, initialLives);

			return false;
		}

		protected bool UpdateEnemyChaseState(Hero hero, float distToHero) {
			bool caught = false;

			caught = Collided(hero);

			if (caught) {
				hero.Caught();
				hero.Center = hero.HeroInitialPos;
              
			}

			TargetPosition = hero.Center;
            if (caught)
            {
                Center = EnemyInitialPos;
                CurrentState = EnemyState.Patrol;
				return true;
            }
            else
            {
                Center += .05f * (hero.Center - Center) * difficulty;
            }

			return false;
		}



		protected void DetectHero(Hero hero) {
			Vector2 toHero = hero.Center - Center;
			if (toHero.Length() < kDistToBeginChase) {

				CurrentState = EnemyState.Chaser;
				TargetPosition = hero.Center;
			}
		}

		public void UpdateEnemyPatrolState(WallSet wall) {
			float ran = XNACS1Base.RandomFloat(-20f, 20.0f);
			if (ran < 5) {
				UpdateEnemyLeftRightDir(wall);
			}
			else {
				UpdateEnemyUpDownDir(wall);
			}
		}

		public void UpdateEnemyLeftRightDir(WallSet wall) {

			bool CollidWithLeftWall = Collided(wall.RoomWall[0]);// collided with left wall 
			bool CollidWithRightWall = Collided(wall.RoomWall[2]);// collided with right wall 

			if (CollidWithRightWall)
				flagX = false;
			if (CollidWithLeftWall)
				flagX = true;

			if (flagX)// going right 
				CenterX = (CenterX + Xchange);
			else
				CenterX = (CenterX - Xchange);
		}

		public void UpdateEnemyUpDownDir(WallSet wall) {
			bool CollidWithUpWall = Collided(wall.RoomWall[1]);// collided with up wall 
			bool CollidWithBottomWall = Collided(wall.RoomWall[3]);// collided with bottom wall 

			if (CollidWithUpWall)
				flagY = false;
			if (CollidWithBottomWall)
				flagY = true;

			if (flagY)// going up
				CenterY = (CenterY + Ychange);
			else
				CenterY = (CenterY - Ychange);// going down 

		}



		public void UpdateCollisionWithEnemy(Enemy enemy) {
			Vector2 mVectorV;
			Vector2 TangentVector;

			float DistOnTangent;
			Vector2 PtOnTangentLine;
			Vector2 NormalVector;
			float DistOnNormal;
			Vector2 mPtOnNormal;
			float mDistOnTangern;

			if (Collided(enemy)) {

				mVectorV = Center - enemy.Center;    // V vector(vector from hero center to wall center)

				TangentVector = enemy.FrontDirection;  //  tangent vector 
				TangentVector.Normalize();
				DistOnTangent = Vector2.Dot(mVectorV, TangentVector);
				PtOnTangentLine = enemy.Center + (DistOnTangent * TangentVector);
				NormalVector = mVectorV - (DistOnTangent * TangentVector);
				NormalVector.Normalize();
				DistOnNormal = Vector2.Dot(mVectorV, NormalVector);


				mPtOnNormal = enemy.Center + (DistOnNormal * NormalVector);
				mDistOnTangern = Vector2.Dot(mVectorV, TangentVector);

				if (Math.Abs(DistOnTangent) < enemy.Width / 2f) // collided with top and bottom 
                {
					float dOnN = Height / 2 + (enemy.Height / 2f);
					if (Math.Abs(DistOnNormal) < dOnN)
						Center = PtOnTangentLine + Math.Sign(DistOnNormal) * (dOnN * NormalVector);
				}
				else {// collide with left and right 

					if (Math.Abs(mDistOnTangern) < Width / 2 + (enemy.Width / 2f))
						Center = mPtOnNormal + Math.Sign(mDistOnTangern) * (Width / 2 + enemy.Width / 2f) * TangentVector;

				}



			}

		}
		public void UpdateCollisionWithObject(XNACS1Rectangle obj) {
			Vector2 mVectorV;
			Vector2 TangentVector;

			float DistOnTangent;
			Vector2 PtOnTangentLine;
			Vector2 NormalVector;
			float DistOnNormal;
			Vector2 mPtOnNormal;
			float mDistOnTangern;

			if (Collided(obj)) {

				mVectorV = Center - obj.Center;    // V vector(vector from hero center to wall center)

				TangentVector = obj.FrontDirection;  //  tangent vector 
				TangentVector.Normalize();
				DistOnTangent = Vector2.Dot(mVectorV, TangentVector);
				PtOnTangentLine = obj.Center + (DistOnTangent * TangentVector);
				NormalVector = mVectorV - (DistOnTangent * TangentVector);
				NormalVector.Normalize();
				DistOnNormal = Vector2.Dot(mVectorV, NormalVector);


				mPtOnNormal = obj.Center + (DistOnNormal * NormalVector);
				mDistOnTangern = Vector2.Dot(mVectorV, TangentVector);

				if (Math.Abs(DistOnTangent) < obj.Width / 2f) // collided with top and bottom 
                {
					float dOnN = Height / 2 + (obj.Height / 2f);
					if (Math.Abs(DistOnNormal) < dOnN)
						Center = PtOnTangentLine + Math.Sign(DistOnNormal) * (dOnN * NormalVector);
				}
				else {// collide with left and right 

					if (Math.Abs(mDistOnTangern) < Width / 2 + (obj.Width / 2f))
						Center = mPtOnNormal + Math.Sign(mDistOnTangern) * (Width / 2 + obj.Width / 2f) * TangentVector;

				}
			}
		}

		public Vector2 TurnEnemyToBunny() {
			if (CurrentState != EnemyState.Bunny) {
				CurrentState = EnemyState.Bunny;
				Width = 4.7f;
				Height = 2.5f;
				SetTextureSpriteSheet("bunnySpriteSheet", 11, 2, 0);
				UseSpriteSheet = true;
				SetTextureSpriteAnimationFrames(0, 0, 10, 1, 5, SpriteSheetAnimationMode.AnimateForward);
				UseSpriteSheetAnimation = true;

				string[] cues = { "transform1", "transform2", "transform3", "transform4" };
				Random r = new Random();
				XNACS1Base.PlayACue(cues[r.Next(0,4)]);
			}
			
			return Center;
		}

		public bool isBunny() {
			return CurrentState == EnemyState.Bunny;
		}

        public Vector2 EnemyInitialPos
        {
            set
            {
                InitialPos = value;
            }
            get
            {
                return InitialPos;
            }

        }


		public void Caught() { 
			lives--;
			
		}

		public int NumLivesLeft {
			get {
				return lives;
			}

		}
	}
}