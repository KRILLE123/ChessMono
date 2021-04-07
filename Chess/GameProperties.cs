using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Chess
{
    class GameProperties : Board
    {
        public int WindowWidth { get; set; }
        public int WindowHeight { get; set; }
        public int CenterPosY { get; set; }
        public int CenterPosX { get; set; }
        public GridProperties currentGridPropery { get; set; }
        public Texture2D[] pieces = new Texture2D[13];
        public List<Texture2D> textures = new List<Texture2D>();

        public GameProperties(int windowWidth, int windowHeight)
        {
            WindowHeight = windowHeight;
            WindowWidth = windowWidth;
            CenterPosX = (WindowWidth - 800) / 2;
            CenterPosY = (windowHeight - 800) / 2;
        }

        public bool InsideBoardGrid(int x, int y)
        {
           return x >= CenterPosX && x <= WindowWidth - CenterPosX && y >= CenterPosY &&y <= WindowHeight - CenterPosY;
        }

        public IntVector2 GetGridFromCoords(int x, int y)
        {
            IntVector2 boardCoords = new IntVector2(x - CenterPosX, y - CenterPosY);


            //if(boardCoords.X < 100 && boardCoords.Y < 100) {
            //    return new IntVector2(0,0);
            //} 
            //else {
            boardCoords.X /= 100;
            boardCoords.Y /= 100;
            
            return new IntVector2(boardCoords.Y, boardCoords.X);
            //}
        }

        private bool boardDrawBool = true;

        public void Draw(SpriteBatch spr)
        {
            for (int i = 0; i < 8; i++)
            {
                if (i % 2 == 0)
                {
                    boardDrawBool = true;
                }
                else
                {
                    boardDrawBool = false;
                }
                for (int j = 0; j < 8; j++)
                {
                    if (boardDrawBool)
                    {
                        boardDrawBool = false;
                        spr.Draw(textures[0], new Vector2(CenterPosX + (j * 100), CenterPosY + (i * 100)), new Rectangle(0, 0, 100, 100), new Color(51, 51, 51), 0.0f, new Vector2(0.5f), new Vector2(1f), SpriteEffects.None, 1.0f);
                    }
                    else
                    {
                        boardDrawBool = true;
                        spr.Draw(textures[0], new Vector2(CenterPosX + (j * 100), CenterPosY + (i * 100)), new Rectangle(0, 0, 100, 100), new Color(230, 230, 230), 0.0f, new Vector2(0.5f), new Vector2(1f), SpriteEffects.None, 1.0f);
                    }

                    if (grid[i, j] > 0)
                    {
                        spr.Draw(pieces[grid[i, j]], new Vector2(CenterPosX + (j * 100), CenterPosY + (i * 100)), null, Color.White, 0.0f, new Vector2(1f), new Vector2(5 / 3f), SpriteEffects.None, 1.0f);
                    } else if (grid[i, j] < 0)
                    {
                        spr.Draw(textures[0], new Vector2(CenterPosX + (j * 100), CenterPosY + (i * 100)), new Rectangle(0, 0, 100, 100), new Color(230, 9, 9 ), 0.0f, new Vector2(0.5f), new Vector2(1f), SpriteEffects.None, 1.0f);
                        if (grid[i, j] != -100)
                        {
                            spr.Draw(pieces[grid[i, j] * -1], new Vector2(CenterPosX + (j * 100), CenterPosY + (i * 100)), null, Color.White, 0.0f, new Vector2(1f), new Vector2(5 / 3f), SpriteEffects.None, 1.0f);
                        }
                    }
                }
            }
        }
    }
}
