using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using XNACS1Lib;

namespace grp_490_final {
	class Room {
		int numDoors;
		int numEnemies;
		XNACS1PrimitiveSet roomObjects;
		Doors[] myDoors;
		Enemy[] myEnemies;
		//Array stuff;
		//bool hasStairs;
		//bool isVisited;
		Color aColor = Color.Gray;
		int x;		//location of the room in x
		int y;		//location of the room in y
		WallSet walls;
		Vector2 coord;
		Vector2 roomOrigin;
		Wand wand;
		int activeEnemies;
		XNACS1Rectangle key;
		Vector2 dropPos;

		public Room(ref StreamReader m, int xCoord, int yCoord) {


			x = xCoord;
			y = yCoord;

			roomOrigin = new Vector2((float)xCoord * 100f, (float)yCoord * ((9f / 16f) * 100f));
			walls = new WallSet(roomOrigin);


			numDoors = Convert.ToInt32(m.ReadLine());
			myDoors = new Doors[numDoors];

			numEnemies = Convert.ToInt32(m.ReadLine());
			activeEnemies = numEnemies;
			myEnemies = new Enemy[numEnemies];
			//line = m.ReadLine();
			//isVisited = Convert.ToBoolean(line);

			//line = m.ReadLine();
			//hasStairs = Convert.ToBoolean(line);
			roomObjects = new XNACS1PrimitiveSet();

			coord = new Vector2((float)x, (float)y);

			for (int i = 0; i < numEnemies; i++) {
				float posX = Convert.ToInt64(m.ReadLine()) + roomOrigin.X;
				float posY = Convert.ToInt64(m.ReadLine()) + roomOrigin.Y;
				myEnemies[i] = new Enemy(new Vector2(posX, posY), "guard");
				roomObjects.AddToSet(myEnemies[i]);
			}

			for (int i = 0; i < numDoors; i++) {
				//takes in the origin of the room and the door type
				myDoors[i] = new Doors(roomOrigin, coord, m.ReadLine());
				roomObjects.AddToSet(myDoors[i]);
			}

			roomObjects.RemoveAllFromAutoDrawSet();

			wand = new Wand();
		}

		public void loadRoom() {
			walls.loadWalls();
			roomObjects.AddAllToAutoDraw();

		}

		public void unloadRoom() {
			roomObjects.RemoveAllFromAutoDrawSet();
			walls.unloadWalls();
		}

		public Vector2 roomOrig() {
			return roomOrigin;
		}

		public Vector2 updateRoom(Hero hero) {
			walls.UpdateWallCollisionWithHero(hero);

			for (int i = 0; i < myEnemies.Length; i++) {
				myEnemies[i].UpdateEnemy(hero, walls);
				walls.UpdateWallCollisionWithEnemy(myEnemies[i]);
			}
			//note to self: work on trigger
			if (XNACS1Base.GamePad.ButtonXClicked())
				wand.Shoot(hero);

			for (int i = 0; i < numEnemies; i++) {
				dropPos = wand.Update(myEnemies[i]);
				if(dropPos.X > -1)
					reduceActiveEnemies();

			}

			if (key != null)
				if (hero.Collided(key)) {
					openAllDoors();
					key.RemoveFromAutoDrawSet();
					key = null;
				}
			Vector2 newRoom = new Vector2(-1f, -1f);
			for (int i = 0; i < numDoors; i++) {

				newRoom = myDoors[i].UpdateDoor(hero);

				if (newRoom.X > 0f)
					return newRoom;
			}

			return newRoom;
		}

		public void reduceActiveEnemies() {
			if (activeEnemies > 0)
				activeEnemies--;

			if(activeEnemies < 1)
				dropKey();
		}

		void dropKey() {
			key = new XNACS1Rectangle(new Vector2(dropPos.X + 2f, dropPos.Y), 1f, 1f);
		}

		public void openAllDoors() {
			for (int i = 0; i < numDoors; i++) {
				myDoors[i].openDoor();
			}
		}
	}
}
