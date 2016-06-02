using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

namespace EvacuationSimulation
{
    class CentralFloor : MonoBehaviour
    {
        FloorGraph fGraph;
        FloorGrid fGrid;

        void Start()
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
            Color32[] pixels = map.GetPixels32();

            Color32[,] trueGrid = new Color32[dim,dim];

            //getpixels goes from bottom to top and left to right
            //our spatial reasoning is from top to bottom and from left to right
            for(int i = 0; i < pixels.Length; i++)
            {
                int column = i % dim;
                int row = dim - (i / dim) - 1;
                trueGrid[column, row] = pixels[i];
            }

            GameObject floorPrefab = Resources.Load<GameObject>("Prefabs/Floor");
            var floorGameObject = Instantiate(floorPrefab);
            fGrid = floorGameObject.GetComponent<FloorGrid>();
            floorGameObject.GetComponent<FloorGrid>().grid = trueGrid;

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
