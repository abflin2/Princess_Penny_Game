﻿#region Reference to system libraries
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
	class Doors : XNACS1Rectangle {
		protected enum DoorsState {
			Open,
			Closed
		}

		Vector2 roomMax;
		Vector2 roomMin;
		Vector2 nextRoom;
		Vector2 heroNextPos;
		DoorsState CurrentState;
		public Doors(Vector2 origin, Vector2 c, string type) {
			roomMin = origin;
			roomMax = new Vector2(origin.X + 100f, (9f / 16f) * 100f + origin.Y);
			Vector2 roomCenter = (roomMax + roomMin) / 2.0f;

			switch (type) {
				case "top":
					// top door
					Center = new Vector2(roomCenter.X, roomMax.Y - 5.5f);
					Width = 7f;
					Height = 2.5f;
					Color = Color.Brown;
					nextRoom = new Vector2(c.X, c.Y + 1f);
					heroNextPos = new Vector2(roomCenter.X, roomMax.Y + 6f);
					break;
				case "bottom":
					//bottom door
					Center = new Vector2(roomCenter.X, roomMin.Y + 1.25f);
					Width = 7f;
					Height = 2.5f;
					Color = Color.Brown;
					nextRoom = new Vector2(c.X, c.Y - 1f);
					heroNextPos = new Vector2(roomCenter.X, roomMin.Y - 12f);
					break;
				case "left":
					// left door
					Center = new Vector2(roomMin.X +1.25f, roomCenter.Y - 2.5f);
					Width = 2.5f;
					Height = 7f;
					Color = Color.Brown;
					nextRoom = new Vector2(c.X - 1f, c.Y);
					heroNextPos = new Vector2(roomMin.X - 4.5f, Center.Y);
					break;
				case "right":
					// right door
					Center = new Vector2(roomMax.X - 1.25f, roomCenter.Y - 2.5f);
					Width = 2.5f;
					Height = 7f;
					Color = Color.Brown;
					nextRoom = new Vector2(c.X +1f, c.Y);
					heroNextPos = new Vector2(roomMax.X + 4.5f, Center.Y);
					break;
				default:
					break;
			}

			CurrentState = DoorsState.Closed;
		}

		public Vector2 UpdateDoor(Hero hero) {
			Vector2 bad = new Vector2(-1f, -1f);
			if (CurrentState == DoorsState.Open) {
				if (Collided(hero)) {
					hero.Center = heroNextPos;
					hero.HeroInitialPos = hero.Center;
					return nextRoom;
				}
			}

			return bad;

			//if (Collided(hero) && CurrentState == DoorsState.Open) {
			//    CurrentState = DoorsState.Open;
			//    Color = Color.White;
			//}
			//else {
			//    CurrentState = DoorsState.Closed;
			//    Color = Color.Brown;
			//}
		}

		public void openDoor() {
			CurrentState = DoorsState.Open;
			Color = Color.White;
		}

	}
}
