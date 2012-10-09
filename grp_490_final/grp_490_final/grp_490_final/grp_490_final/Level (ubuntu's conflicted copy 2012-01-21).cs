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
	class Level{

		int levelNum; //this level's number
		Room[,] rooms; //total number of rooms on this level
		int cellNum = 6;
		string line;
		Vector2 currentRoom;
		Vector2 initialRoom; //coordinates first room princess enters in a level
		Hero hero;


		public Level(ref StreamReader s) {
			if ((line = s.ReadLine()) != null)
				levelNum = Convert.ToInt32(line);

			rooms = new Room[cellNum, cellNum];
			int numRooms = Convert.ToInt32(s.ReadLine());
			initialRoom = new Vector2(Convert.ToInt64(s.ReadLine()), Convert.ToInt64(s.ReadLine())); 

			for (int i = 0; i < cellNum; i++) {
				for (int j = 0; j < cellNum; j++)
					rooms[i, j] = null;
			}

			while(numRooms > 0){
				int x = Convert.ToInt32(s.ReadLine());
				int y = Convert.ToInt32(s.ReadLine());
				rooms[x,y] = new Room(ref s, x, y);
				numRooms--;
			}


		}


		public void loadInitialRoom() {
			XNACS1Base.World.SetWorldCoordinate
				(new Vector2(initialRoom.X * 100f, initialRoom.Y * (9f/16f)*100f), 100f);
			hero = new Hero(new Vector2(rooms[(int)initialRoom.X, (int)initialRoom.Y].roomOrig().X
				+ 50f, rooms[(int)initialRoom.X, (int)initialRoom.Y].roomOrig().Y + (9f / 16f) * 50f));
			rooms[(int)initialRoom.X, (int)initialRoom.Y].loadRoom();
			currentRoom = initialRoom;
		}

		public bool updateLevel(XNACS1Lib.XNACS1Lib.GamePadSupport.AllButtonsOnGamePad pad, 
			XNACS1Lib.XNACS1Lib.GamePadSupport.ThumbSticksOnGamePad thumbs) {
			hero.UpdateHero(thumbs.Left);
			Vector2 newRoom = rooms[(int)currentRoom.X, (int)currentRoom.Y].updateRoom(hero);



			if (newRoom.X > -1) {
				unLoadRoom();
				currentRoom = newRoom;
				loadRoom();
			}


			XNACS1Base.EchoToTopStatus("Hero has " + hero.NumTimesCaught + " live(s)");
			return false;
		}

		public void loadRoom() {
			XNACS1Base.World.SetWorldCoordinate
				(new Vector2(currentRoom.X * 100f, currentRoom.Y * (9f / 16f) * 100f), 100f);
			rooms[(int)currentRoom.X, (int)currentRoom.Y].loadRoom();
		}

		public void unLoadRoom() {
			rooms[(int)currentRoom.X, (int)currentRoom.Y].unloadRoom();
		}
	}
}
