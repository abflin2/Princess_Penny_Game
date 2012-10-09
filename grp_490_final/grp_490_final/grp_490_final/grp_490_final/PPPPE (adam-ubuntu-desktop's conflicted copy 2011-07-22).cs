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

namespace grp_490_final
{

public class PPPPE : XNACS1Base
{

	int totalLevels, currentLevel;
	bool paused;
	XNACS1Rectangle pauseScreen;
	bool gameStarted;
	bool gameOver;
	StreamReader s;
	Level theLevel;
	string path;

	//-------------------------------------------------------------------------
	//For difficulty selection menu
	int ticks = 8; //Have to cool down the menu selector -- dumb
	XNACS1Rectangle selected, easy, medium, hard;
	int difficulty;
	//-------------------------------------------------------------------------

	XNACS1Rectangle cover, gameOverText;

    protected override void InitializeWorld()
    {
        //XNACS1Base.SetAppWindowPixelDimension(false, 1250, 300);
        World.SetWorldCoordinate(new Vector2(0f, 0f), 100f);
		totalLevels = 1;
		path = @"level0.txt";	//this works if world1.txt is in the bin/x86/debug folder
		try {
			s = new StreamReader(path);
				

			theLevel = new Level(ref s);

		}
		catch (Exception e) {
			// Let the user know what went wrong.
			Console.WriteLine("The file could not be read:");
			Console.WriteLine(e.Message);
		}

		currentLevel = 0;
		//levelSet[currentLevel].loadInitialRoom(); -- Moved this down to where the game starts after menu
		World.SetWorldCoordinate(new Vector2(0, 0), 100f);

		paused = gameStarted = gameOver = false;

		selected = new XNACS1Rectangle(new Vector2(50, 30), 42, 10);
		selected.Color = Color.Red;

		easy = new XNACS1Rectangle(new Vector2(50, 45), 40, 8, "easy");
		medium = new XNACS1Rectangle(new Vector2(50, 30), 40, 8, "medium");
		hard = new XNACS1Rectangle(new Vector2(50, 15), 40, 8, "hard");
    }

	protected override void UpdateWorld() {
		if (gameStarted && !gameOver) {
			int roomX = (int)theLevel.currentRoom.X;
			int roomY = (int)theLevel.currentRoom.Y;
			if (pauseScreen == null) {
				float x = theLevel.rooms[roomX, roomY].roomOrigin.X + 50f;
				float y = theLevel.rooms[roomX, roomY].roomOrigin.Y + 27f;
				pauseScreen = new XNACS1Rectangle(new Vector2(x, y), 100f, 54f);
				pauseScreen.Color = Color.Black;
			}

			if (GamePad.ButtonStartClicked())
				paused = paused ? false : true;

			if (!paused) {
				if (GamePad.ButtonBackClicked())
					Exit();

				bool newLevel = false;
				newLevel = theLevel.updateLevel(GamePad.Buttons, GamePad.ThumbSticks);

				if (newLevel) {
					if (currentLevel < totalLevels) {
						currentLevel++;
						theLevel.unloadLevel();

						switch (currentLevel) {
							case 1:
								path = @"level1.txt";
								break;
							case 2:
								path = @"level2.txt";
								break;
							case 3:
								path = @"level3.txt";
								break;
							case 4:
								path = @"level4.txt";
								break;
							default:
								break;
						}
						s = new StreamReader(path);
						theLevel = new Level(ref s);
						theLevel.loadInitialRoom(difficulty);
					}

					else {
						gameOver = true;
					}
				}

				if (pauseScreen.IsInAutoDrawSet()) {
					pauseScreen.RemoveFromAutoDrawSet();
					pauseScreen = null;
				}
			}
			else {
				pauseScreen.AddToAutoDrawSet();
				pauseScreen.Label = "PAUSED";
				pauseScreen.LabelColor = Color.White;
			}
			gameOver = theLevel.hero.lives < 0;
			String message = "Number of Lives: " + theLevel.hero.lives + "   Level Number: " + currentLevel;
			SetTopEchoColor(Color.White);
			EchoToTopStatus(message);
		}
		else if (gameOver) {
			World.SetWorldCoordinate(new Vector2(0, 0), 100f);
			cover = new XNACS1Rectangle(new Vector2(0, 0), 200, 200);
			cover.Color = Color.Black;
			gameOverText = new XNACS1Rectangle(new Vector2(50, 40), 40, 10);
			gameOverText.Label = "GAME OVER \n X TO RESTART";
			gameOverText.Color = Color.Black;
			gameOverText.LabelColor = Color.White;

			if (GamePad.ButtonXClicked()) {
				gameOverText.RemoveFromAutoDrawSet();
				gameOverText = null;
				cover.RemoveFromAutoDrawSet();
				cover = null;
				this.InitializeWorld();
			}
		}
		else if (!gameStarted) {
			if (GamePad.ButtonXClicked()) {
				if (selected.Center == easy.Center)
					difficulty = 1;
				else if (selected.Center == medium.Center)
					difficulty = 2;
				else
					difficulty = 3;

				theLevel.loadInitialRoom(difficulty);
				EchoToTopStatus(difficulty.ToString());
				gameStarted = true;
			}

			if (ticks > 0)
				ticks--;
			else {
				if (GamePad.ThumbSticks.Right.Y > 0) {
					if (selected.Center != easy.Center)
						if (selected.Center != medium.Center)
							selected.Center = medium.Center;
						else
							selected.Center = easy.Center;
					ticks = 8;
				}
				else if (GamePad.ThumbSticks.Right.Y < 0) {
					if (selected.Center != hard.Center)
						if (selected.Center != medium.Center)
							selected.Center = medium.Center;
						else
							selected.Center = hard.Center;
					ticks = 8;
				}
			}
		}
	}
}
}
