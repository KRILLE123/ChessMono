using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chess
{
    class EventHandler
    {
        GameProperties gameProperties;

        public EventHandler(GameProperties gameProperties)
        {
            this.gameProperties = gameProperties;
        }

        public string GetCurrentEvent(MouseState newState)
        {
            bool insideBoardGrid = gameProperties.InsideBoardGrid(newState.X, newState.Y);

            if (insideBoardGrid)
            {
                IntVector2 gridCoord = gameProperties.GetGridFromCoords(newState.X, newState.Y);
                int gridInfo = gameProperties.grid[gridCoord.X, gridCoord.Y];


                if (gridInfo != 0)
                {
                    gameProperties.currentGridPropery = new GridProperties(gridCoord, gridInfo);
                }
                //To-do HUD event etc.


            }
            return "";
        }
    }
}
