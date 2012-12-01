using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
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
		//Knight knight;
		//float knightX;
		//float knightY;
		bool triggerPressed;
		int numDoors;
		int numEnemies;
		XNACS1PrimitiveSet roomObjects;
		Doors[] myDoors;
		Enemy[] myEnemies;
		public int hasStairs;
		public Stair stairs;
		int x;		//location of the room in x
		int y;		//location of the room in y
		WallSet walls;
		public Vector2 coord;
		public Vector2 roomOrigin;
		public Wand wand;
		int activeEnemies;
		Key key;
		Vector2 dropPos;
		XNACS1Rectangle backGround;
		//bool allDone;
		Wizard badGuy;
		bool hasWon;
		bool isAlive;
		int numKnights;
		int numGuards;
		public bool heroCaught;

		public Room(XmlNode xml)
		{
			//Todo: Write constructor that takes in xmlnode and creates room.
			//x = Convert.ToInt32(xml.ChildNodes.
		}
		public Room(ref StreamReader m, int xCoord, int yCoord) {

			x = xCoord;
			y = yCoord;

			triggerPressed = false;
			roomOrigin = new Vector2((float)xCoord * 100f, (float)yCoord * ((9f / 16f) * 100f));
			backGround = new XNACS1Rectangle(new Vector2(roomOrigin.X + 50f, roomOrigin.Y + 50 * (9f / 16f)), 100f, 100f * (9f / 16f), "dungeonFloor");
			walls = new WallSet(roomOrigin);

			hasStairs = Convert.ToInt32(m.ReadLine());
			numDoors = Convert.ToInt32(m.ReadLine());
			myDoors = new Doors[numDoors];
			numEnemies = Convert.ToInt32(m.ReadLine());
			numKnights = Convert.ToInt32(m.ReadLine());
			roomObjects = new XNACS1PrimitiveSet();
			isAlive = false;
			heroCaught = false;

			if (hasStairs == 1) {
				float stairX = (float)Convert.ToDouble(m.ReadLine());
				float stairY = (float)Convert.ToDouble(m.ReadLine());
				stairs = new Stair(new Vector2(stairX + roomOrigin.X, stairY + roomOrigin.Y));
				roomObjects.AddToSet(stairs);

				float badGuyX = (float)Convert.ToDouble(m.ReadLine());
				float badGuyY = (float)Convert.ToDouble(m.ReadLine());
				badGuy = new Wizard(new Vector2(badGuyX + roomOrigin.X, badGuyY + roomOrigin.Y));

				roomObjects.AddToSet(badGuy);
				hasWon = false;
			}

			activeEnemies = numEnemies;
			myEnemies = new Enemy[numEnemies];
			numGuards = numEnemies - numKnights;
			coord = new Vector2((float)x, (float)y);


			for (int i = 0; i < numGuards; i++) {
				float posX = (float)Convert.ToDouble(m.ReadLine()) + roomOrigin.X;
				string line = m.ReadLine();
				float posY = (float)Convert.ToDouble(line) + roomOrigin.Y;
				myEnemies[i] = new Guard(new Vector2(posX, posY));
				roomObjects.AddToSet(myEnemies[i]);
			}

			for (int i = numGuards; i < numEnemies; i++) {
				float posX = (float)Convert.ToDouble(m.ReadLine()) + roomOrigin.X;
				string line = m.ReadLine();
				float posY = (float)Convert.ToDouble(line) + roomOrigin.Y;
				myEnemies[i] = new Knight(new Vector2(posX, posY));
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

		public void loadRoom(Hero hero, int levelNum) {
			walls.loadWalls();
			roomObjects.AddAllToAutoDraw();
			if (hasStairs == 1 && !stairs.available)
				stairs.RemoveFromAutoDrawSet();
			if (numEnemies == 0 && !isAlive)
				openAllDoors();
			walls.updateStatus(hero, levelNum);
		}

		public void unloadRoom() {
			roomObjects.RemoveAllFromAutoDrawSet();
			walls.unloadWalls();
		}

		public void deleteRoom() {
			roomObjects.RemoveAllFromSet();
			myDoors = null;
			myEnemies = null;
			walls = null;
			stairs = null;
			key = null;
			if (backGround != null)
				backGround.RemoveFromAutoDrawSet();
			backGround = null;

			badGuy = null;
		}

		public Vector2 roomOrig() {
			return roomOrigin;
		}

		public bool stairsInRoom() {
			if (hasStairs == 1)
				return true;
			else return false;
		}

		public Vector2 updateRoom(Hero hero, int levelNum) {
			heroCaught = false;
			walls.UpdateWallCollisionWithHero(hero);

			for (int i = 0; i < myEnemies.Length; i++) {

				bool temp = myEnemies[i].UpdateEnemy(hero, walls);
				if (temp)
					heroCaught = temp;

				walls.UpdateWallCollisionWithEnemy(myEnemies[i]);
			}

			walls.updateStatus(hero, levelNum);
			if ((XNACS1Base.GamePad.Triggers.Right > 0 && !triggerPressed) || XNACS1Base.GamePad.ButtonXClicked()) {
				wand.Shoot(hero);
				triggerPressed = true;
			}

			if (XNACS1Base.GamePad.Triggers.Right == 0) {
				triggerPressed = false;
			}

			dropPos = wand.Update(myEnemies, walls);
			if (dropPos.X > -1)
				reduceActiveEnemies();

			//allDone = true;
			//for (int i = 0; i < myEnemies.Length; i++) {
			//    if (!myEnemies[i].isBunny())
			//        allDone = false;

			//}

			if (hasStairs == 1 && badGuy != null) {
				walls.updateWizardStats(badGuy);
				hasWon = badGuy.Update(hero, wand);


				if (hasWon) {
					stairs.AddToAutoDrawSet();
					stairs.available = true;
					openAllDoors();
				}

				if (badGuy.heroHit) {
					heroCaught = true;
				}

			}

			if (key != null) {
				if (hero.Collided(key)) {
					openAllDoors();
					XNACS1Base.PlayACue("door_open");
					key.RemoveFromAutoDrawSet();
				}

				if (myDoors[0].isOpen() && key != null) {
					key.RemoveFromAutoDrawSet();
					key = null;
				}
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

			if (activeEnemies < 1)
				dropKey();
		}

		void dropKey() {
			if (key == null)
				key = new Key(new Vector2(dropPos.X + 3f, dropPos.Y), 1f, 1f);

		}

		public void openAllDoors() {
			for (int i = 0; i < numDoors; i++) {
				myDoors[i].openDoor(myDoors[i].pos);
			}
		}
		public void closedDoors() {
			for (int i = 0; i < numDoors; i++) {
				myDoors[i].closedDoor(myDoors[i].pos);
			}
		}
	}
}
