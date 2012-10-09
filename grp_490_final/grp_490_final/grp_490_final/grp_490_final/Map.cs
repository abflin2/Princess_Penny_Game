using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XNACS1Lib;

namespace grp_490_final
{
    class Map : XNACS1Rectangle
    {
        public XNACS1Rectangle[,] RoomMap;
        XNACS1Rectangle youAreHere;
        public Map(Vector2 center)
        {
            Center = center;
            Height = 40f;
            Width = 40f;
			Texture = "old_map";
            RoomMap = new XNACS1Rectangle[6, 6];
            
        }
        public Map()
        {
        }
        public void drawRoom(Vector2 LowerLeft)
        {
			LowerLeft.X += 5f;
			LowerLeft.Y += 5f;
            Vector2 temp = LowerLeft;

            for (int j = 0; j < 6; j++)
            {

                for (int i = 0; i < 6; i++)
                {

                    RoomMap[j, i] = new XNACS1Rectangle(new Vector2(LowerLeft.X + 2.5f, LowerLeft.Y + 2.5f), 4.5f, 4.5f);

					RoomMap[j, i].Texture = "clear_box";
                    LowerLeft.X += 5f;


                }
                temp.Y += 5;
                LowerLeft = temp;
             
                
            }


        }
        //public XNACS1Rectangle[,]  GetRoomMap
        //{
        //    set
        //    {
        //        RoomMap = value;
        //    }
        //    get
        //    {
        //        return RoomMap;
        //    }

        //}
        public void removeRoom()
        {
            

            for (int j = 0; j < 6; j++)
            {

                for (int i = 0; i < 6; i++)
                {
                    RoomMap[j, i].RemoveFromAutoDrawSet();
                    RoomMap[j, i] = null;
                }
            }
            youAreHere.RemoveFromAutoDrawSet();
            youAreHere = null;

        }
        public void displayMap(Level theLevel)
        {

            Vector2 currentRoom = theLevel.GetCurrentRoom;
            Vector2 currentRoomOrigin = theLevel.GetcurrentOrigin;

         // new Vector2(LowerLeft.X + 2.5f, LowerLeft.Y + 2.5f)


            //RoomMap[(int)currentRoom.Y, (int)currentRoom.X].Color = Color.Yellow;

            youAreHere = new XNACS1Rectangle(new Vector2((currentRoom.X *5f + LowerLeft.X + 7.5f), (currentRoom.Y*5 + LowerLeft.Y + 7.5f)), 2f, 2f);
            youAreHere.Texture = "princess_stick";
               

                            //RoomMap[4,3].Color = Color.Yellow;

            for (int j = 0; j < 6; j++)
            {
                for (int i = 0; i < 6; i++)
                {
					if (theLevel.getCurrentRoomState(j, i))
						RoomMap[i, j].Texture = "pencil_square";
              


                }
            }
          }
    }
}