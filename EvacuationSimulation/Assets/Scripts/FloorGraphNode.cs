using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EvacuationSimulation
{
    public class FloorGraphNode
    {
        public int id;
        List<int> incident;
        List<int> edges;
        int certainty;
        
        //pathfinding variables
        public float F;
        public float G;
        public float H;  // f = gone + heuristic
        public int parent;
        public int X, Y;

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

        //Getters Setters
        public List<int> GetEdges
        {
            get { return edges; }
        }

    }
}
