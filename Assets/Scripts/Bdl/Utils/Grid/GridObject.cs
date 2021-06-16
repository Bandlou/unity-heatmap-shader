using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bdl.Utils.Grid
{
    public class GridObject
    {
        // PROPERTIES
        public int X { get => x; }
        public int Y { get => y; }

        // PRIVATE FIELDS
        protected int x;
        protected int y;

        // CONSTRUCTOR

        public GridObject(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
