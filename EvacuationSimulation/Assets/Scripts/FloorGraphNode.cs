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
        bool live; //Indicates wether a node is accessible, or if it was removed from the mental map (due to obstruction)
        bool exit;
        
        //pathfinding variables
        public float F;
        public float G;
        public float H;  // f = gone + heuristic
        public int parent;
        public int X, Y;
        public bool dicovered, explored;

        public FloorGraphNode(int id, List<int> incident, bool exit = false, int certainty = 1, bool live = true)
        {
            this.id = id;
            this.incident = incident;
            this.certainty = certainty;
            this.live = live;
            this.exit = exit;
            dicovered = false;
            explored = false;
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

        public int Id
        {
            get { return id; }
        }

        public int Certainty
        {
            get { return certainty; }
            set { certainty = value; }
        }

        public bool Live
        {
            get { return live; }
            set { live = value; }
        }

        public bool Exit
        {
            get { return exit; }
        }
    }
}
