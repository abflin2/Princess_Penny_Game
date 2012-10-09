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


	int totalLevels;
	Level[] levelSet;

	int currentLevel;

    protected override void InitializeWorld()
    {
        //XNACS1Base.SetAppWindowPixelDimension(false, 1250, 300);
        World.SetWorldCoordinate(new Vector2(0f, 0f), 600f);
		string path = @"C:\Users\A\Documents\Dropbox\CSS 490 Projects\490_final\grp_490_final\world1.txt";
		try {
			StreamReader s = new StreamReader(path);
			String line;
			if ((line = s.ReadLine()) != null)
				totalLevels = Convert.ToInt32(line);

			levelSet = new Level[totalLevels];
			for (int i = 0; i < totalLevels; i++)
				levelSet[i] = new Level(ref s);

		}
		catch (Exception e) {
			// Let the user know what went wrong.
			Console.WriteLine("The file could not be read:");
			Console.WriteLine(e.Message);
		}

		currentLevel = 0;
		levelSet[currentLevel].loadInitialRoom();
    }


    protected override void UpdateWorld()
    {
		if (GamePad.ButtonBackClicked())
			Exit();
		bool newLevel = false;
		newLevel = levelSet[currentLevel].updateLevel(GamePad.Buttons, GamePad.ThumbSticks);

		if (newLevel)
			//levelSet[++currentLevel].loadLevel();
			levelSet[currentLevel].loadInitialRoom();
    }
}
}
