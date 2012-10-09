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
	class WallSet {
		XNACS1Rectangle DisplayWall;
		public XNACS1Rectangle[] RoomWalls;
		XNACS1PrimitiveSet allWalls;
		Vector2 roomMin;
		Vector2 roomMax;
		XNACS1Rectangle left, ctr, right;
		public WallSet( Vector2 origin) {
			RoomWalls = new XNACS1Rectangle[4];

			roomMin = origin;

			roomMax = new Vector2(origin.X  + 100f, origin.Y + (9f/16f)*100f);
			allWalls = new XNACS1PrimitiveSet();
			InitializeWalls();
			allWalls.RemoveAllFromAutoDrawSet();
		}

		public void InitializeWalls() {
			
			Vector2 RoomCenter = new Vector2(roomMin.X + 100f/2f, roomMin.Y + ((9f/16f)*100f - 4.5f)/2f);
            // left wall 
            // left wall 
            RoomWalls[0] = new XNACS1Rectangle(new Vector2(roomMin.X + 1.25f, RoomCenter.Y), 2.5f, roomMax.Y - roomMin.Y - 9f);
            allWalls.AddToSet(RoomWalls[0]);
            RoomWalls[0].Texture = "left_gray_wall";
            // top wall 
            RoomWalls[1] = new XNACS1Rectangle(new Vector2(RoomCenter.X, roomMax.Y - 4.25f - 1.25f), roomMax.X - roomMin.X, 2.5f);
            allWalls.AddToSet(RoomWalls[1]);
            RoomWalls[1].Texture = "long_gray_wall_top";
            // right wall 
            RoomWalls[2] = new XNACS1Rectangle(new Vector2(roomMax.X - 1.5f, RoomCenter.Y), 2.5f, roomMax.Y - roomMin.Y - 9f);
            allWalls.AddToSet(RoomWalls[2]);
            RoomWalls[2].Texture = "right_gray_wall";
            // bottom wall 
            RoomWalls[3] = new XNACS1Rectangle(new Vector2(RoomCenter.X, roomMin.Y + 1.25f), roomMax.X - roomMin.X, 2.5f);
            allWalls.AddToSet(RoomWalls[3]);
            RoomWalls[3].Texture = "long_gray_wall_bottom";



			DisplayWall = new XNACS1Rectangle(new Vector2(RoomCenter.X, roomMax.Y - 1.9f), roomMax.X-roomMin.X, 4.5f);
			DisplayWall.TopOfAutoDrawSet();
			DisplayWall.Color = Color.Black;
			allWalls.AddToSet(DisplayWall);
			left = new XNACS1Rectangle(new Vector2(roomMin.X + 15f, roomMax.Y - 2f), 30f, 4f);
			left.Color = Color.Black;

			allWalls.AddToSet(left);
			ctr = new XNACS1Rectangle(new Vector2(roomMin.X + 45f, roomMax.Y - 2f), 30f, 4f);
			ctr.Color = Color.Black;

			allWalls.AddToSet(ctr);
			right = new XNACS1Rectangle(new Vector2(roomMin.X + 85f, roomMax.Y - 2f), 30f, 4f);
			right.Color = Color.Black;

			allWalls.AddToSet(right);
		}

		public void updateStatus(Hero hero, int levelNum) {
            left.Label = "Princess Lives Left: " + hero.lives;
			left.LabelColor = Color.White;

			ctr.Label = "Level: " + levelNum;
			ctr.LabelColor = Color.White;
		}


		public void updateWizardStats(Wizard wiz) {
			right.Label = "Wizard Health: " + wiz.healthRemaining;
			right.LabelColor = Color.White;
		}

		public void removeWizardStats() {
			right.Label = "";
		}
		public void UpdateWallCollisionWithHero(Hero hero) {
			for (int i = 0; i < RoomWalls.Length; i++)
				hero.UpdateCollisionWithObject(RoomWalls[i]);
		}
		public void UpdateWallCollisionWithEnemy(Enemy enemy) {
			for (int i = 0; i < RoomWalls.Length; i++)
				enemy.UpdateCollisionWithObject(RoomWalls[i]);
		}

		public void loadWalls() {
			allWalls.AddAllToAutoDraw();
		}

		public void unloadWalls() {
			allWalls.RemoveAllFromAutoDrawSet();
		}

		public XNACS1Rectangle[] RoomWall { get { return RoomWalls; } }




	}
}
