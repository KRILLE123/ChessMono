using System;
using System.Collections.Generic;
using System.Text;

namespace Chess
{
    class GridProperties
    {
        public IntVector2 GridCoords { get; set; }
        public int GridIdentifier { get; set; }

        public GridProperties(IntVector2 gridCoords, int gridIdentifier)
        {
            GridCoords = gridCoords;
            GridIdentifier = gridIdentifier;
        }
    }
}
