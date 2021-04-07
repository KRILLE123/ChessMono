using System;
using System.Collections.Generic;
using System.Text;

namespace Chess
{
    abstract class Piece
    {
        public abstract void Move(int currentX, int currentY);
        public int Identifier { get; set; }
    }
}
