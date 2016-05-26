using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EvacuationSimulation
{
    class FloorGraphEdge
    {
        int id;
        int origin;
        int destination;
        int certainty;

        public FloorGraphEdge(int id, int origin, int destination, int certainty = 1)
        {
            this.id = id;
            this.origin = origin;
            this.destination = destination;
            this.certainty = certainty;
        }

        //Getters Setters
        public int GetOrigin
        {
            get
            {
                return origin;
            }
        }

        public int GetDestination
        {
            get
            {
                return destination;
            }
        }


    }
}
