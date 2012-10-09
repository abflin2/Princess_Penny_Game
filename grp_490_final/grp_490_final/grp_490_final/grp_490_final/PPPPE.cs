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
	XNACS1Rectangle levelScreen;
	int tick;
	bool loading;
	bool newLevel;
	XNACS1Rectangle areYouSure;
	XNACS1Rectangle yes;
	XNACS1Rectangle no;
	bool exiting;
	public Hero hero;
    Map map;
	XNACS1Rectangle lostLifeScreen;
	bool win;
	XNACS1Rectangle winScreen;

	//-------------------------------------------------------------------------
	//For difficulty selection menu
	int ticks = 8; //Have to cool down the menu selector -- dumb
	XNACS1Rectangle selected, easy, medium, hard;
	int difficulty;
	//-------------------------------------------------------------------------

	XNACS1Rectangle cover, gameOverText,startUpScreen;

	protected void setupGame() {
		win = false;
		World.SetWorldCoordinate(new Vector2(0f, 0f), 100f);
		totalLevels = 4;
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
		World.SetWorldCoordinate(new Vector2(0, 0), 100f);

		paused = gameStarted = gameOver = false;

		startUpScreen = new XNACS1Rectangle(new Vector2(50f, (9f / 16f) * 50f), 100f, (9f / 16f) * 100f, "newcastlebg");
		selected = new XNACS1Rectangle(new Vector2(50, 30), 42, 10);
		selected.Color = Color.Black;

		easy = new XNACS1Rectangle(new Vector2(50, 45), 40, 8, "newEasy");
		medium = new XNACS1Rectangle(new Vector2(50, 30), 40, 8, "newMedium");
		hard = new XNACS1Rectangle(new Vector2(50, 15), 40, 8, "newHard");

		tick = 120;
		loading = false;
		exiting = false;

	}

    protected override void InitializeWorld() {
		setupGame();
		PlayBackgroundAudio("princess_theme", .3f);
	}

	protected override void UpdateWorld() {

		if (gameStarted && !gameOver && !win) {
			int roomX = (int)theLevel.currentRoom.X;
			int roomY = (int)theLevel.currentRoom.Y;
			if (pauseScreen == null) {
				float x = theLevel.currentOrigin.X + 50f;
				float y = theLevel.currentOrigin.Y + (9f/16f)*50f;
				pauseScreen = new XNACS1Rectangle(new Vector2(x, y), 100f, (9f / 16f)*100f);
                pauseScreen.Color = Color.Black;

                map = new Map(new Vector2(x, y));// betania added this 
                map.Visible = false;
                map.RemoveFromAutoDrawSet();// betania added this
			}

			if (GamePad.ButtonStartClicked() && !exiting)
				paused = paused ? false : true;

			if (theLevel.paused)
				World.Paused = true;

			if (!theLevel.paused && lostLifeScreen != null) {
				lostLifeScreen.RemoveFromAutoDrawSet();
				lostLifeScreen = null;
			}

			if (!theLevel.paused && !paused)
				World.Paused = false;

			if (!paused && !theLevel.paused ) {
				//if (theLevel.paused)
				//	World.Paused = true;

				if (GamePad.ButtonBackClicked() || exiting) {
					exiting = true;
					World.Paused = true;
					float x = theLevel.rooms[roomX, roomY].roomOrigin.X + 50f;
					float y = theLevel.rooms[roomX, roomY].roomOrigin.Y + 27f;
					if (areYouSure == null) {
						areYouSure = new XNACS1Rectangle(new Vector2(x, y), 50, 27f);
						areYouSure.Color = Color.Black;
						areYouSure.Label = "Are you sure you want to quit? All progress will be lost.\n"
							+ "Press A to exit or B or cancel and return to the game";
						areYouSure.LabelColor = Color.White;
						areYouSure.AddToAutoDrawSet();
					}

					if (GamePad.ButtonAClicked()) {
						Exit();
					}

					else if (GamePad.ButtonBClicked()) {
						areYouSure.RemoveFromAutoDrawSet();
						areYouSure = null;
						World.Paused = false;
						exiting = false;

					}
				}
				else {
					if (!loading) {
						newLevel = false;
						newLevel = theLevel.updateLevel(GamePad.Buttons, GamePad.ThumbSticks, hero);
						if (newLevel) {
							loading = true;
							currentLevel++;
						}
					}

					if (loading) {

						if (currentLevel <= totalLevels) {
							float x = theLevel.rooms[roomX, roomY].roomOrigin.X + 50f;
							float y = theLevel.rooms[roomX, roomY].roomOrigin.Y + 28.2f;
							if (levelScreen == null) {
								levelScreen = new XNACS1Rectangle(new Vector2(x, y), 100f, 56f);
								levelScreen.Color = Color.Black;
								levelScreen.Label = "Level " + currentLevel;
								levelScreen.LabelColor = Color.White;
								levelScreen.AddToAutoDrawSet();
							}

							World.Paused = true;
							if (tick > 0) {
								tick--;
							}
							else {
								tick = 120;
								levelScreen.RemoveFromAutoDrawSet();
								levelScreen = null;
								theLevel.unloadLevel();
                                // delete all rooms
                                //map.removeRoom();
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
										win = true;
										break;
								}
								s = new StreamReader(path);
								theLevel = new Level(ref s);
								theLevel.loadInitialRoom(difficulty, hero);
								World.Paused = false;
								loading = false;
							}


						}

						else {
								win = true;
							
						}
					}

					if (pauseScreen.IsInAutoDrawSet()) {
						pauseScreen.RemoveFromAutoDrawSet();
						pauseScreen = null;
                         if (map.IsInAutoDrawSet())
                        {
                            map.RemoveFromAutoDrawSet();
                            map.removeRoom();
                            map.Visible = false;
                           // map = null;
                        }
					}
				}
			}
			else if (paused && !theLevel.paused) {

				pauseScreen.AddToAutoDrawSet();
				if (!map.Visible) {
					map.AddToAutoDrawSet();// betania added this 
					map.drawRoom(map.LowerLeft);// betania added this 

					map.displayMap(theLevel);// betania added

					map.Visible = true;
				}

			}

			else {
				if (lostLifeScreen == null) {
					lostLifeScreen = new XNACS1Rectangle(new Vector2(theLevel.currentOrigin.X + 50f, theLevel.currentOrigin.Y + ((9f / 16f) * 100) / 2), 100f, 56f);
					lostLifeScreen.Label = "Princess Lives Left " + hero.NumTimesCaught;
					lostLifeScreen.Color = Color.Black;
					lostLifeScreen.LabelColor = Color.White;
					lostLifeScreen.AddToAutoDrawSet();
				}
				theLevel.updateLevel(GamePad.Buttons, GamePad.ThumbSticks, hero);
			}
			

			gameOver = hero.lives < 0;
			//String message = "Number of Lives: " + hero.lives + "   Level Number: " + currentLevel;
			//SetTopEchoColor(Color.White);
			//EchoToTopStatus(message);
			theLevel.rooms[(int)theLevel.currentRoom.X, (int)theLevel.currentRoom.Y].wand.UpdateWand(currentLevel);

			//if (w == null)
			//    w = new Wizard(Vector2.One);
			//w.Update(hero, theLevel.rooms[(int)theLevel.currentRoom.X, (int)theLevel.currentRoom.Y].wand);
		}
		else if (gameOver || win) {
			World.SetWorldCoordinate(new Vector2(0, 0), 100f);
			if (gameOver) {
				if (gameOverText == null) {
					cover = new XNACS1Rectangle(new Vector2(50f, ((9f / 16f) * 100f) / 2f), 100, (9f / 16f) * 100f);
					cover.Color = Color.Black;
				}

				if (gameOverText == null) {
					gameOverText = new XNACS1Rectangle(new Vector2(50, 40), 40, 10);
					gameOverText.Label = "GAME OVER \n A TO RESTART \n B TO EXIT";
					gameOverText.Color = Color.Black;
					gameOverText.LabelColor = Color.White;
				}
			}
			else {
				if (winScreen == null) {
					winScreen = new XNACS1Rectangle(new Vector2(50f, ((9f / 16f) * 100f) / 2f), 100, (9f / 16f) * 100f);
					winScreen.Color = Color.Black;
					winScreen.Label = "You've escaped the evil wizard's castle!\n You're free at last!";
					winScreen.LabelColor = Color.White;
				}
			}

			if(theLevel != null)
				theLevel.unloadLevel();
			theLevel = null;

			if(hero != null)
				hero.RemoveFromAutoDrawSet();

			hero = null;

			if (GamePad.ButtonAClicked()) {
				gameOverText.RemoveFromAutoDrawSet();
				gameOverText = null;
				cover.RemoveFromAutoDrawSet();
				cover = null;
				setupGame();
			}

            if (GamePad.ButtonBClicked())
            {
                Exit();
            }
		}
		else if (!gameStarted) {
			if (GamePad.ButtonAClicked()) {
				if (selected.Center == easy.Center)
					difficulty = 1;
				else if (selected.Center == medium.Center)
					difficulty = 2;
				else
					difficulty = 3;
				hero = new Hero(new Vector2(0f, 0f), difficulty);
				theLevel.loadInitialRoom(difficulty, hero);
				//EchoToTopStatus(difficulty.ToString());
				gameStarted = true;
				selected.RemoveFromAutoDrawSet();


				
			}

			if (tick > 0)
				tick--;
			else {
				if (GamePad.ThumbSticks.Left.Y > 0) {
					if (selected.Center != easy.Center)
						if (selected.Center != medium.Center)
							selected.Center = medium.Center;
						else
							selected.Center = easy.Center;
					tick = 8;
				}
				else if (GamePad.ThumbSticks.Left.Y < 0) {
					if (selected.Center != hard.Center)
						if (selected.Center != medium.Center)
							selected.Center = medium.Center;
						else
							selected.Center = hard.Center;
					tick = 8;
				}
			}
		}
	}
}
}
