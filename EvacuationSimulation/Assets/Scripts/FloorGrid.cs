using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace EvacuationSimulation
{
    class FloorGrid
    {
        //true -> free space
        //false -> not accessable space
        bool[,] objectGrid;

        //This class holds a basic representation of the environment.
        //Squares are either collidable or they aren't, which will allow the agent to plan a path.
        public FloorGrid(int width, int height)
        {
            objectGrid = new bool[width, height];
        }
    }
}
