using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Text;
using System.Diagnostics;

namespace Chess
{
    class Board
    {
        public int[,] grid = new int[8, 8] { 
            {8, 9, 10, 11, 12, 10, 9, 8 },
            {7, 7, 7, 7, 7, 7, 7, 7 },
            {0, 0, 0, 0, 10, 0, 0, 0 },
            {0, 0, 0, 0, 0, 0, 0, 0 },
            {0, 0, 2, 0, 10, 4, 0, 0 },
            {0, 0, 0, 0, 0, 1, 0, 0 },
            {1, 1, 1, 1, 1, 10, 1, 1 },
            {2, 3, 4, 5, 6, 4, 3, 2 },
        };

        public List<int> moves = new List<int>();
        private bool hasChoosenPiece = false;
        private GridProperties choosenPieceProperties;


        private int GetColorById(int pieceId) // 1 = white, 2 = black, 0 = empty
        {
            //Debug.WriteLine(pieceId);
            if (pieceId >= 1 && pieceId <= 6)
            {
                return 1;
            } else if (pieceId > 6 && pieceId <= 12)
            {
                return 2;
            }
            return 0;
        }

        public void ResetBoard()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (grid[i, j] == -100)
                    {
                        grid[i, j] = 0;
                    } else if (grid[i,j] < 0)
                    {
                        grid[i, j] *= -1;
                    }
                }
            }
        }

        public void ChoosePiece(GridProperties gridProperty)
        {
            ResetBoard();

            //Debug.WriteLine(gridProperty.GridIdentifier);
            //Debug.WriteLine(gridProperty.GridCoords.X);

            if (gridProperty.GridIdentifier == 7 || choosenPieceProperties?.GridIdentifier == 7 && hasChoosenPiece)
            {
                PawnMove(gridProperty, 1);
            }
            else if (gridProperty.GridIdentifier == 1 || choosenPieceProperties?.GridIdentifier == 1 && hasChoosenPiece)
            {
                PawnMove(gridProperty, -1);
            } else if (gridProperty.GridIdentifier == 4 || choosenPieceProperties?.GridIdentifier == 4 && hasChoosenPiece)
            {
                GetDiagonalMoves(gridProperty);
            }
            else if (gridProperty.GridIdentifier == 10 || choosenPieceProperties?.GridIdentifier == 10 && hasChoosenPiece)
            {
                GetDiagonalMoves(gridProperty);
            }
            else if (gridProperty.GridIdentifier == 2 || choosenPieceProperties?.GridIdentifier == 2 && hasChoosenPiece)
            {
                GetHorizontalMoves(gridProperty);
                GetVerticalMoves(gridProperty);
            }

            SwitchChoosenPiece(gridProperty);
        }

        private int GetPieceOnGridCoord(IntVector2 gridCoord)
        {
            return grid[gridCoord.X, gridCoord.Y];
        }

        //private void SetDiagonaValue(int first, int second, int currentX, int currentY, int i)
        //{
        //    if (grid[currentX + (i * first), currentY + (i * second)] == 0)
        //    {
        //        grid[currentX + (i * first), currentY + (i * second)] = -1;
        //    }
        //    else
        //    {
        //        grid[currentX + (i * first), currentY + (i * second)] *= -1;
        //    }
        //}

        private void GetDiagonalMoves(GridProperties gridProperties, int[,] customDirectionLogic = null, int maxMoves = -1, bool checkForOpponents = false)
        {
            int currentX = hasChoosenPiece ? choosenPieceProperties.GridCoords.X : gridProperties.GridCoords.X;
            int currentY = hasChoosenPiece ? choosenPieceProperties.GridCoords.Y : gridProperties.GridCoords.Y;

            int[,] diagonalDirectionLogic = customDirectionLogic == null ? new int[4, 2] { { -1, -1 }, { -1, 1 }, { 1, 1 }, { 1, -1 } } : customDirectionLogic;

            for (int i = 0; i < (diagonalDirectionLogic.Length / 2); i++)
            {
                int j = 1;
                int xMove = currentX + (j * diagonalDirectionLogic[i, 0]);
                int yMove = currentY + (j * diagonalDirectionLogic[i, 1]);

                while (xMove <= 7  && xMove >= 0 && yMove <= 7 && yMove >= 0 && (maxMoves != -1 ? j <= maxMoves : true ))
                {
                    if(checkForOpponents)
                    {
                        if((GetColorById(gridProperties.GridIdentifier) == 1 && GetColorById(grid[xMove, yMove]) != 2) ||
                            (GetColorById(gridProperties.GridIdentifier) == 2 && GetColorById(grid[xMove, yMove]) != 1) || GetColorById(gridProperties.GridIdentifier) == 0)
                        {
                            break;
                        }
                    }
                    if (hasChoosenPiece) 
                    {
                        if(xMove == gridProperties.GridCoords.X && yMove == gridProperties.GridCoords.Y)
                        {
                            grid[gridProperties.GridCoords.X, gridProperties.GridCoords.Y] = choosenPieceProperties.GridIdentifier;
                            grid[choosenPieceProperties.GridCoords.X, choosenPieceProperties.GridCoords.Y] = 0;
                            ResetBoard();
                            break;
                        }
                    }
                    else
                    {
                        if (grid[xMove, yMove] == 0)
                        {
                            grid[xMove, yMove] = -100;
                        }
                        else if (grid[xMove, yMove] > 0)
                        {
                            if ((GetColorById(gridProperties.GridIdentifier) == 1 && GetColorById(grid[xMove, yMove]) != 1)
                                || (GetColorById(gridProperties.GridIdentifier) == 2 && GetColorById(grid[xMove, yMove]) != 2))
                            {
                                grid[xMove, yMove] *= -1;
                            }
                            break;
                        }
                    }
                    xMove = currentX + (j * diagonalDirectionLogic[i, 0]);
                    yMove = currentY + (j * diagonalDirectionLogic[i, 1]);
                    j++;
                }
            }

        }
        private void GetHorizontalMoves(GridProperties gridProperties, int[] upOrDown = null, int maxMoves = -1)
        {
            int[] _upOrDown = upOrDown == null ? new int[2] { 1, 1 } : upOrDown;
            int x = hasChoosenPiece ? choosenPieceProperties.GridCoords.X : gridProperties.GridCoords.X;
            int y = hasChoosenPiece ? choosenPieceProperties.GridCoords.Y : gridProperties.GridCoords.Y;
            int color = GetColorById(hasChoosenPiece ? choosenPieceProperties.GridIdentifier : gridProperties.GridIdentifier);

            for (int j = 0; j < 2; j++)
            {
                int i = j == 1 ? _upOrDown[0] : -_upOrDown[1] ;
                
                while (y + i < 8 && y + i >= 0)
                {
                    if (hasChoosenPiece)
                    {

                    }
                    else
                    {
                        if (grid[x, y + i] == 0)
                        {
                            grid[x, y + i] = -100;
                        }
                        else if (color != GetColorById(grid[x, y + i]))
                        {
                            grid[x, y + i] *= -1;
                            break;

                        }
                        else if (color == GetColorById(grid[x, y + i]))
                        {
                            break;
                        }
                    }
                    if(j == 1) { i++; } else { i--; }
                }
            }
        }


        private void GetVerticalMoves(GridProperties gridProperties, int[] upOrDown = null, int maxMoves = -1)
        {
            int[] _upOrDown = upOrDown == null ? new int[2] { 1, 1 } : upOrDown;
            int x = hasChoosenPiece ? choosenPieceProperties.GridCoords.X : gridProperties.GridCoords.X;
            int y = hasChoosenPiece ? choosenPieceProperties.GridCoords.Y : gridProperties.GridCoords.Y;
            int color = GetColorById(hasChoosenPiece ? choosenPieceProperties.GridIdentifier : gridProperties.GridIdentifier);
  
            for (int j = 0; j < _upOrDown.Length; j++)
            {
                if (_upOrDown[j] != 0)
                {
                    int i = j == 0 ? _upOrDown[0] : -_upOrDown[1];
                    int counter = 1;


                    while (x + i < 8 && x + i >= 0 && (hasChoosenPiece ? choosenPieceProperties.GridCoords.Y == gridProperties.GridCoords.Y : true) && (maxMoves != -1 ? counter <= maxMoves : true))
                    {
                        if (hasChoosenPiece)
                        {
                            if (gridProperties.GridCoords.X == x + i)
                            {
                                //Debug.WriteLine("lel");
                                grid[gridProperties.GridCoords.X, gridProperties.GridCoords.Y] = choosenPieceProperties.GridIdentifier;
                                grid[choosenPieceProperties.GridCoords.X, choosenPieceProperties.GridCoords.Y] = 0;
                                ResetBoard();
                                break;
                            }
                        }
                        else
                        {
                            if (grid[x + i, y] == 0)
                            {
                                grid[x + i, y] = -100;
                            }
                            else if (color != GetColorById(grid[x + i, y]) && gridProperties.GridIdentifier != 1 && gridProperties.GridIdentifier != 7)
                            {
                                grid[x + i, y] *= -1;
                                break;
                            }
                            else if (color == GetColorById(grid[x + i, y]))
                            {
                                break;
                            }
                        }
                        if (j == 0) { i++; } else { i--; }
                        counter++;

                    }
                }
            }
        }


        private void SwitchChoosenPiece(GridProperties gridProperties)
        {
            if (hasChoosenPiece)
            {
                hasChoosenPiece = false;
            }
            else
            {
                hasChoosenPiece = true;
                choosenPieceProperties = gridProperties;

            }
        }

        private void PawnMove(GridProperties gridProperty, int whiteorblackMultiplier)
        {
            int color = GetColorById(gridProperty.GridIdentifier);
            int[] verticalGrid = null;
            int[,] customDiagonalDir = null; 

            if (color == 2) { verticalGrid = new int[2] { 1, 0 }; customDiagonalDir = new int[2, 2] { { 1, 1 }, { 1, -1 } };  } 
            else if(color == 1) { verticalGrid = new int[2] { 0, 1 }; customDiagonalDir = new int[2, 2] { { -1, -1 }, { -1, 1 } }; }

            if ((color == 1 && gridProperty.GridCoords.X == 6) || (color == 2 && gridProperty.GridCoords.X == 1) 
                || (hasChoosenPiece && GetColorById(choosenPieceProperties.GridIdentifier) == 1 && choosenPieceProperties.GridCoords.X == 6) 
                || (hasChoosenPiece && GetColorById(choosenPieceProperties.GridIdentifier) == 2 && choosenPieceProperties.GridCoords.X == 1)) {
                GetVerticalMoves(gridProperty, verticalGrid, 2);
                GetDiagonalMoves(gridProperty, customDiagonalDir, 1, true);
            } else
            {
                GetVerticalMoves(gridProperty, verticalGrid, 1);
                GetDiagonalMoves(gridProperty, customDiagonalDir, 1, true);
            }
        }
    }
}
