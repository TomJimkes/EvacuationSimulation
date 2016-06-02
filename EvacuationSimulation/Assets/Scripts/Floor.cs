using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace EvacuationSimulation
{

    public class Floor
    {
        FloorGraph fGraph;
        FloorGrid fGrid;

        public Floor()
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

        
    }
}
