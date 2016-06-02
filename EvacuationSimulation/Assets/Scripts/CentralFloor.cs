using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

namespace EvacuationSimulation
{
    class CentralFloor
    {
        FloorGraph fGraph;
        FloorGrid fGrid;

        public CentralFloor()
        {
            createGrid();
            createGraph();
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
            Texture2D map = Resources.Load<Texture2D>("Maps/test");
            int dim = map.height;
            Color[] pixels = map.GetPixels();

            //Each type of tile has its own color
            //flammability is defined by opacity

        }
        //Build graph
        private void createGraph()
        {
            //Take grid
            //perform flood search
            //create edges and nodes respectively
        }

    }
}
