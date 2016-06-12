using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EvacuationSimulation
{
    public class FloorGraphEdge
    {
        int id;
        int origin;
        int destination;
        int certainty;
        bool live;
        int room;

        public FloorGraphEdge(int id, int room, int origin, int destination, int certainty = 1, bool live = true)
        {
            this.id = id;
            this.origin = origin;
            this.destination = destination;
            this.certainty = certainty;
            this.live = live;
            this.room = room;
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

        public int Id
        {
            get { return id; }
        }

        public bool Live
        {
            get { return live; }
            set { live = value; }
        }

        public int Certainty
        {
            get { return certainty; }
            set { certainty = value; }
        }

        public int Room
        {
            get { return room; }
        }
    }
}
