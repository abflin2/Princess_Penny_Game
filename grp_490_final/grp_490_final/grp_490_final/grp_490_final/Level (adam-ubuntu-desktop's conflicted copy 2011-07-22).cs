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
		enum loadState { notLoading, loading, finished };
		enum loadDirection { top, bottom, left, right, noDirection };
		loadState loads;
		loadDirection currentDirection;
		int levelNum; //this level's number
		public Room[,] rooms; //total number of rooms on this level
		int cellNum = 6;
		string line;
		public Vector2 currentRoom;
		public Vector2 lastVisitedRoom;
		Vector2 initialRoom; //coordinates first room princess enters in a level
		public Hero hero;
		Vector2 currentOrigin;
		Vector2 expectedOrigin;

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

		public void unloadLevel(){
			for (int i = 0; i < cellNum; i++) {
				for (int j = 0; j < cellNum; j++ ) {
					if (rooms[i, j] != null) {
						rooms[i, j].deleteRoom();
					}
				}
			}
			hero.RemoveFromAutoDrawSet();
			hero = null;
		}

		public void loadInitialRoom(int difficulty) {
			XNACS1Base.World.SetWorldCoordinate
				(new Vector2(initialRoom.X * 100f, initialRoom.Y * (9f/16f)*100f), 100f);
            //XNACS1Base.World.SetBackgroundTexture("bg1");

			hero = new Hero(new Vector2(rooms[(int)initialRoom.X, (int)initialRoom.Y].roomOrig().X
				+ 50f, rooms[(int)initialRoom.X, (int)initialRoom.Y].roomOrig().Y + (9f / 16f) * 50f), difficulty);
			rooms[(int)initialRoom.X, (int)initialRoom.Y].loadRoom();
			currentRoom = initialRoom;
			lastVisitedRoom = currentRoom;
			currentOrigin = new Vector2(currentRoom.X * 100f, currentRoom.Y * (9f / 16f) * 100f);
			loads = loadState.notLoading;
			currentDirection = loadDirection.noDirection;
			
		}

		public bool updateLevel(XNACS1Lib.XNACS1Lib.GamePadSupport.AllButtonsOnGamePad pad, 
			XNACS1Lib.XNACS1Lib.GamePadSupport.ThumbSticksOnGamePad thumbs) {
				Vector2 newRoom = new Vector2(-1, -1);
			if(loads == loadState.notLoading){
				hero.UpdateHero(thumbs.Left);
				newRoom = rooms[(int)currentRoom.X, (int)currentRoom.Y].updateRoom(hero);

			}

			if (newRoom.X > -1) {
				loads = loadState.loading;
				if (newRoom.X > currentRoom.X)
					currentDirection = loadDirection.right;

				else if (newRoom.X < currentRoom.X)
					currentDirection = loadDirection.left;

				else if (newRoom.Y > currentRoom.Y)
					currentDirection = loadDirection.top;

				else
					currentDirection = loadDirection.bottom;

				lastVisitedRoom = currentRoom;
				currentRoom = newRoom;
				loadRoom();
                
				expectedOrigin = new Vector2(currentRoom.X * 100f, currentRoom.Y * ((9f/16f)* 100f));
				
			}

			if (loads == loadState.loading) {
				hero.Visible = false;
				moveRoom();
			}

			if(loads == loadState.finished){
				hero.Visible = true;
				unLoadRoom();
				loads = loadState.notLoading;
				currentDirection = loadDirection.noDirection;
				currentOrigin = expectedOrigin;
             
			}

			
            //XNACS1Rectangle Screen;
            //if (hero.NumTimesCaught == 0)
            //{
            //    Screen = new XNACS1Rectangle(new Vector2(currentOrigin.X + 50f, currentOrigin.Y +25f), 100f, 54f);
            //    Screen.Label = "GAME OVER";
            //    Screen.LabelColor = Color.White;
            //    }
			//XNACS1Base.EchoToTopStatus("Hero has " + hero.NumTimesCaught + " live(s)");

			if(rooms[(int)currentRoom.X, (int)currentRoom.Y].stairsInRoom()){
				if (hero.Collided(rooms[(int)currentRoom.X, (int)currentRoom.Y].stairs))
					return true;
			}
			return false;
		}

		public void loadRoom() {
			//XNACS1Base.World.SetWorldCoordinate
				//(new Vector2(currentRoom.X * 100f, currentRoom.Y * (9f / 16f) * 100f), 100f);
			rooms[(int)currentRoom.X, (int)currentRoom.Y].loadRoom();
          
		}

		public void unLoadRoom() {
			rooms[(int)lastVisitedRoom.X, (int)lastVisitedRoom.Y].unloadRoom();
           
		}

		public void moveRoom() {

			switch (currentDirection) {
				case loadDirection.right:
					if (currentOrigin.X < expectedOrigin.X -.5f) {
						currentOrigin.X += .75f;
						XNACS1Base.World.SetWorldCoordinate
						(new Vector2(currentOrigin.X, currentOrigin.Y), 100f);
                        
					}
					else {
						XNACS1Base.World.SetWorldCoordinate
						(new Vector2(expectedOrigin.X, expectedOrigin.Y), 100f);
						loads = loadState.finished;

					}

					break;

				case loadDirection.left:
					if (currentOrigin.X > expectedOrigin.X + .5f) {
						currentOrigin.X -= .75f;
						XNACS1Base.World.SetWorldCoordinate
							(new Vector2(currentOrigin.X, currentOrigin.Y), 100f);
					}

					else {
						loads = loadState.finished;
						XNACS1Base.World.SetWorldCoordinate
							(new Vector2(expectedOrigin.X, expectedOrigin.Y), 100f);
					}

					break;

				case loadDirection.top:
					if (currentOrigin.Y < expectedOrigin.Y - .5f) {
						currentOrigin.Y += .75f;
						XNACS1Base.World.SetWorldCoordinate
							(new Vector2(currentOrigin.X, currentOrigin.Y), 100f);
					}

					else {
						loads = loadState.finished;
						XNACS1Base.World.SetWorldCoordinate
							(new Vector2(expectedOrigin.X, expectedOrigin.Y), 100f);
					}

					break;

				case loadDirection.bottom:
					if (currentOrigin.Y > expectedOrigin.Y + .5f) {
						currentOrigin.Y -= .75f;
						XNACS1Base.World.SetWorldCoordinate
							(new Vector2(currentOrigin.X, currentOrigin.Y), 100f);
					}

					else {
						loads = loadState.finished;
						XNACS1Base.World.SetWorldCoordinate
							(new Vector2(expectedOrigin.X, expectedOrigin.Y), 100f);
					}

					break;

				default:
					loads = loadState.finished;
					break;
			}
		}
	}
}
