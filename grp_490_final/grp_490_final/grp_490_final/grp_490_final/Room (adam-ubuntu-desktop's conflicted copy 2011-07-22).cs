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
		public int hasStairs;
		public Stair stairs;
		int x;		//location of the room in x
		int y;		//location of the room in y
		WallSet walls;
		public Vector2 coord;
		public Vector2 roomOrigin;
		Wand wand;
		int activeEnemies;
		Key key;
		Vector2 dropPos;
        XNACS1Rectangle backGround;
		bool allDone;
		public Room(ref StreamReader m, int xCoord, int yCoord) {


			x = xCoord;
			y = yCoord;

			roomOrigin = new Vector2((float)xCoord * 100f, (float)yCoord * ((9f / 16f) * 100f));
			backGround = new XNACS1Rectangle(new Vector2(roomOrigin.X + 50f, roomOrigin.Y + 50 * (9f / 16f)), 100f, 100f * (9f / 16f), "bg1");
			walls = new WallSet(roomOrigin);

			hasStairs = Convert.ToInt32(m.ReadLine());
			numDoors = Convert.ToInt32(m.ReadLine());
			myDoors = new Doors[numDoors];
			numEnemies = Convert.ToInt32(m.ReadLine());

			if (hasStairs == 1) {
				float stairX = (float)Convert.ToDouble(m.ReadLine());
				float stairY = (float)Convert.ToDouble(m.ReadLine());
				stairs = new Stair(new Vector2(stairX + roomOrigin.X, stairY + roomOrigin.Y));
			}

			activeEnemies = numEnemies;
			myEnemies = new Enemy[numEnemies];
			roomObjects = new XNACS1PrimitiveSet();

			coord = new Vector2((float)x, (float)y);

			for (int i = 0; i < numEnemies; i++) {
				float posX = (float)Convert.ToDouble(m.ReadLine()) + roomOrigin.X;
				string line = m.ReadLine();
				float posY = (float)Convert.ToDouble(line) + roomOrigin.Y;
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
			if(numEnemies == 0)
				openAllDoors();

		}

		public void unloadRoom() {
			roomObjects.RemoveAllFromAutoDrawSet();
			walls.unloadWalls();
		}

		public void deleteRoom(){
			roomObjects.RemoveAllFromSet();
			myDoors = null;
			myEnemies = null;
			walls = null;
			stairs = null;
			key = null;
			backGround.RemoveFromAutoDrawSet();
			backGround = null;
		}

		public Vector2 roomOrig(){
			return roomOrigin;
		}

		public bool stairsInRoom() {
			if (hasStairs == 1)
				return true;
			else return false;
		}

		public Vector2 updateRoom(Hero hero) {
			walls.UpdateWallCollisionWithHero(hero);
           XNACS1Base.EchoToTopStatus("num of enemy" + activeEnemies );
			for (int i = 0; i < myEnemies.Length; i++) {
				myEnemies[i].UpdateEnemy(hero, walls);
				walls.UpdateWallCollisionWithEnemy(myEnemies[i]);
			}
			//note to self: work on trigger
			if (XNACS1Base.GamePad.ButtonXClicked())
				wand.Shoot(hero);


			dropPos = wand.Update(myEnemies);
				if(dropPos.X > -1)
					reduceActiveEnemies();

			allDone = true;
			for (int i = 0; i < myEnemies.Length; i++) {
				if (!myEnemies[i].isBunny())
					allDone = false;

			}


            if (key != null)
            {
                if (hero.Collided(key))
                {
                    openAllDoors();
                    key.RemoveFromAutoDrawSet();      
                }

				if(myDoors[0].isOpen() && key != null)
					key.RemoveFromAutoDrawSet();
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
			if(key == null)
				key = new Key(new Vector2(dropPos.X + 3f, dropPos.Y), 2f, 1f);
           
		}

		public void openAllDoors() {
			for (int i = 0; i < numDoors; i++) {
				myDoors[i].openDoor();
			}
		}
        public void closedDoors()
        {
            for (int i = 0; i < numDoors; i++)
            {
                myDoors[i].closedDoor();
            }
        }
	}
}
