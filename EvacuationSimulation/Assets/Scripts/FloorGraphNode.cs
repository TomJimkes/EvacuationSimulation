﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EvacuationSimulation
{
    public class FloorGraphNode
    {
        int id;
        List<int> incident;
        int certainty;
        bool live; //Indicates wether a node is accessible, or if it was removed from the mental map (due to obstruction)
        bool exit;

        public FloorGraphNode(int id, List<int> incident, bool exit = false, int certainty = 1, bool live = true)
        {
            this.id = id;
            this.incident = incident;
            this.certainty = certainty;
            this.live = live;
            this.exit = exit;
        }

        //Getters Setters
        public List<int> GetIncident
        {
            get { return incident; }
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
