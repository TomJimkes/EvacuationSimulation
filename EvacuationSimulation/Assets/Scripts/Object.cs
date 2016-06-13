using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EvacuationSimulation;
using UnityEditorInternal;

namespace Assets.Scripts
{
    class Object
    {
        public Point coord;
        public int width, height;
        public bool passable;

        public Object( Point Coord, int Width, int Height, bool Passable )
        {
            coord = Coord;
            width = Width;
            height = Height;
            passable = Passable;
        }
    }
}
