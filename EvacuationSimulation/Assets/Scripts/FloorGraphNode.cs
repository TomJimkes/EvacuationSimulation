using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EvacuationSimulation
{
    class FloorGraphNode
    {
        int id;
        List<int> incident;
        int certainty;

        public FloorGraphNode(int id, List<int> incident, int certainty = 1)
        {
            this.id = id;
            this.incident = incident;
            this.certainty = certainty;
        }

        //Getters Setters
        public List<int> GetIncident
        {
            get { return incident; }
        }

    }
}
