using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EvacuationSimulation
{
    class CentralFloor
    {
        FloorGraph fGraph;
        FloorGrid fGrid;

        public CentralFloor()
        {

        }

        //Getters setters
        public FloorGraph Graph
        {
            get { return fGraph; }
        }

        public FloorGrid Grid
        {
            get { return fGrid; }
        }

        //Create grid
        private void createGrid()
        {
            //Read from file
            //build grid
        }
        //Build graph

    }
}
