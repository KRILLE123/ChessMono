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
            {0, 0, 0, 0, 10, 4, 0, 0 },
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

            if (gridProperty.GridIdentifier == 7 || (choosenPieceProperties?.GridIdentifier == 7 && hasChoosenPiece))
            {
                PawnMove(gridProperty, 1);  
            }
            else if(gridProperty.GridIdentifier == 1 || choosenPieceProperties?.GridIdentifier == 1 && hasChoosenPiece)
            {
                PawnMove(gridProperty, -1);
            } else if(gridProperty.GridIdentifier == 4 || choosenPieceProperties?.GridIdentifier == 4 && hasChoosenPiece)
            {
                GetDiagonalMoves(gridProperty);
            }
            else if (gridProperty.GridIdentifier == 10 || choosenPieceProperties?.GridIdentifier == 10 && hasChoosenPiece)
            {
                GetDiagonalMoves(gridProperty);
            }

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
            int currentX = gridProperties.GridCoords.X;
            int currentY = gridProperties.GridCoords.Y;

            int[,] diagonalDirectionLogic = customDirectionLogic == null ? new int[4, 2] { { -1, -1 }, { -1, 1 }, { 1, 1 }, { 1, -1 } } : diagonalDirectionLogic = customDirectionLogic;

            for (int i = 0; i < diagonalDirectionLogic.Length / 2; i++)
            {
                int j = 1;
                int xMove = currentX + (j * diagonalDirectionLogic[i, 0]);
                int yMove = currentY + (j * diagonalDirectionLogic[i, 1]);

                while (xMove <= 7  && xMove >= 0 && yMove <= 7 && yMove >= 0 && (maxMoves != -1 ? j <= maxMoves : true ))
                {
                    if(checkForOpponents)
                    {
                        if((GetColorById(gridProperties.GridIdentifier) == 1 && GetColorById(grid[xMove, yMove]) != 2) ||
                            (GetColorById(gridProperties.GridIdentifier) == 2 && GetColorById(grid[xMove, yMove]) != 1))
                        {
                            break;
                        }
                    }
                    if (grid[xMove, yMove] == 0)
                    {
                        grid[xMove, yMove] = -100;
                    }
                    else if (grid[xMove, yMove] > 0)
                    {
                        if ((GetColorById(gridProperties.GridIdentifier) == 1 && GetColorById(grid[xMove, yMove]) != 1)
                            || (GetColorById(gridProperties.GridIdentifier) == 2 && GetColorById(grid[xMove, yMove]) != 2) )
                        {
                            grid[xMove, yMove] *= -1;
                        }
                        break;
                    }
                    xMove = currentX + (j * diagonalDirectionLogic[i, 0]);
                    yMove = currentY + (j * diagonalDirectionLogic[i, 1]);
                    j++;
                }
            }
        }

            //Debug.WriteLine(currentX + " current x");

            //Debug.WriteLine(currentY + " current y");

            //if (currentX > 0)
            //{
            //    if (currentX == 7)
            //    {
            //        for (int i = 1; i <= currentY; i++)
            //        {
            //            Debug.WriteLine(i);

            //            SetDiagonaValue(-1, -1, currentX, currentY, i);
            //        }
            //    } else
            //    {
            //        for (int i = 1; i <= currentY; i++)
            //        {
            //            Debug.WriteLine(i);

            //            SetDiagonaValue(-1, -1, currentX, currentY, i);
            //        }
            //    }

            //    //for (int i = 1; i <= 7-currentY; i++)
            //    //{
            //    //    SetDiagonaValue(-1, 1, currentX, currentY, i);
            //    //}
            //}


            //if (currentX < 7)
            //{
            //    for (int i = 1; i <= 7 - currentY; i++)
            //    {
            //        SetDiagonaValue(1, 1, currentX, currentY, i);

            //        if (currentX == 1 || currentX == 6) { break; }

            //    }
            //    if (currentX == 0)
            //    {
            //        for (int i = 1; i <= currentY; i++)
            //        {
            //            Debug.WriteLine(i);
            //            SetDiagonaValue(1, -1, currentX, currentY, i);

            //            if (currentX == 1 || currentX == 6) { break; }

            //        }
            //    } else
            //    {
            //        for (int i = 1; i <= 7 - currentY; i++)
            //        {
            //            Debug.WriteLine(i);
            //            SetDiagonaValue(1, -1, currentX, currentY, i);

            //            if (currentX == 1 || currentX == 6) { break; }

            //        }
            //    }
            //}
        //}


        private void GetVerticalMoves(GridProperties gridProperties, int maxMoves = -1)
        {
            if (hasChoosenPiece)
            {
                GridProperties g = gridProperties;
                gridProperties = choosenPieceProperties;
                choosenPieceProperties = g;
            }

            int x = gridProperties.GridCoords.X;
            int y = gridProperties.GridCoords.Y;
            int color = GetColorById(gridProperties.GridIdentifier);
            int multiplier = 1;
            int i = 1;

            if(color == 1 ) { multiplier = -1; } else if(color == 2) { multiplier = 1; }


            while(x + (i*multiplier) < 8 && x + (i * multiplier) > 0 && maxMoves != -1 ? i <= maxMoves : true)
            {
                Debug.WriteLine(hasChoosenPiece);
                if (hasChoosenPiece)
                {
                    if (choosenPieceProperties.GridCoords.X == x + (i * multiplier) && choosenPieceProperties.GridCoords.Y == y)
                    {
                        Debug.WriteLine("lel");
                        grid[x + (i * multiplier), y] = gridProperties.GridIdentifier;
                        grid[gridProperties.GridCoords.X, gridProperties.GridCoords.Y] = 0;
                        hasChoosenPiece = false;
                    }
                }
                else
                {
                    if (grid[x + (i * multiplier), y] == 0)
                    {
                        grid[x + (i * multiplier), y] = -100;
                    }
                    else if ((color == 2 && GetColorById(grid[x + (i * multiplier), y]) != 2) || (color == 1 && GetColorById(grid[x + (i * multiplier), y]) != 1) && ((gridProperties.GridIdentifier == 1 || gridProperties.GridIdentifier == 7) && color == 1 && GetColorById(grid[x + (i * multiplier), y]) != 2 && color == 2 && GetColorById(grid[x + (i * multiplier), y]) != 1))
                    {
                        grid[x + (i * multiplier), y] *= -1;
                    }
                    i++;
                }
            }
            if (hasChoosenPiece)
            {
                hasChoosenPiece = false;
            } else {
                hasChoosenPiece = true;
                choosenPieceProperties = gridProperties;

            }
        }

        private void PawnMove(GridProperties gridProperty, int whiteorblackMultiplier)
        {
            if ((GetColorById(gridProperty.GridIdentifier) == 1 && gridProperty.GridCoords.X == 6) || (GetColorById(gridProperty.GridIdentifier) == 7 && gridProperty.GridCoords.X == 1)) {
                GetVerticalMoves(gridProperty, 2);
                GetDiagonalMoves(gridProperty, new int[2, 2] { { -1, -1 }, { -1, 1 } }, 1, true);
            } else
            {
                GetVerticalMoves(gridProperty, 1);
                GetDiagonalMoves(gridProperty, new int[2, 2] { { -1, -1 }, { -1, 1 } }, 1, true);
            }

            //if (hasChoosenPiece)
            //{
            //    for (int i = 0; i < pawnSteps; i++)
            //    {
            //        if (gridProperty.GridCoords.X == choosenPieceProperties.GridCoords.X + ((1+i)* whiteorblackMultiplier) && choosenPieceProperties.GridCoords.Y == gridProperty.GridCoords.Y)
            //        {
            //            grid[choosenPieceProperties.GridCoords.X, choosenPieceProperties.GridCoords.Y] = 0;
            //            grid[gridProperty.GridCoords.X, gridProperty.GridCoords.Y] = choosenPieceProperties.GridIdentifier;
            //        }
            //    }
            //    hasChoosenPiece = false;
            //}
            //else
            //{
            //    if ((gridProperty.GridCoords.X == 1 && whiteorblackMultiplier == 1) || (gridProperty.GridCoords.X == 6 && whiteorblackMultiplier == -1))
            //    {
            //        pawnSteps = 2;
            //    }
            //    else { pawnSteps = 1; }

            //    for (int i = 0; i < pawnSteps; i++)
            //    {
            //        int rightAttack = GetPieceOnGridCoord(new IntVector2(gridProperty.GridCoords.X + 1 * whiteorblackMultiplier, gridProperty.GridCoords.Y + 1));
            //        int leftAttack = GetPieceOnGridCoord(new IntVector2(gridProperty.GridCoords.X + 1 * whiteorblackMultiplier, gridProperty.GridCoords.Y - 1));

            //        Debug.WriteLine(rightAttack);


            //        if (leftAttack != 0 && rightAttack != 0 && GetPieceColorById(gridProperty.GridIdentifier) != GetPieceColorById(GetPieceOnGridCoord(new IntVector2(gridProperty.GridCoords.X + 1 * whiteorblackMultiplier, gridProperty.GridCoords.Y + 1)))
            //            && GetPieceColorById(gridProperty.GridIdentifier) != GetPieceColorById(GetPieceOnGridCoord(new IntVector2(gridProperty.GridCoords.X - 1 * whiteorblackMultiplier, gridProperty.GridCoords.Y - 1))))
            //        {
            //            grid[gridProperty.GridCoords.X + 1 * whiteorblackMultiplier, gridProperty.GridCoords.Y - 1] *= -1;
            //            grid[gridProperty.GridCoords.X + 1 * whiteorblackMultiplier, gridProperty.GridCoords.Y + 1] *= -1;

            //            break;
            //        }

            //        else if (leftAttack != 0)
            //        {
            //            grid[gridProperty.GridCoords.X + 1 * whiteorblackMultiplier, gridProperty.GridCoords.Y + 1] *= -1;
            //        }

            //        else if (rightAttack != 0)
            //        {
            //            Debug.WriteLine("ol");
            //            grid[gridProperty.GridCoords.X + 1 * whiteorblackMultiplier, gridProperty.GridCoords.Y - 1] *= -1;
            //        }

            //        if (GetPieceOnGridCoord(new IntVector2( gridProperty.GridCoords.X + ((1 + i) * whiteorblackMultiplier), gridProperty.GridCoords.Y )) == 0)
            //        {
            //            grid[gridProperty.GridCoords.X + ((1 + i) * whiteorblackMultiplier), gridProperty.GridCoords.Y] = -100;
            //        }  else
            //        {
            //            break;
            //        }

            //    }
            //    choosenPieceProperties = gridProperty;
            //    hasChoosenPiece = true;
            //}
        }
        
        //public void CreateGrid()
        //{
        //    int[] blackIdentifierPattern = new int[8] { 8, 9, 10, 11, 12, 10, 9, 8 };
        //    int[] whiteIdentifierPattern = new int[8] { 2, 3, 4, 5, 6, 4, 3, 2 };


        //    for (int i = 0; i < 8; i++)
        //    {
        //        for (int j = 0; j < 8; j++)
        //        {

        //            switch(i)
        //            {
        //                case 0:
        //                    grid[i, j] = blackIdentifierPattern[j];
        //                    break;
        //                case 1:
        //                    grid[i, j] = 7;
        //                    break;
        //                case 6:
        //                    grid[i, j] = 1;
        //                    break;
        //                case 7:
        //                    grid[i, j] = whiteIdentifierPattern[j];
        //                    break;
        //            }
        //        }
        //    }
        //}

       
    }
}
