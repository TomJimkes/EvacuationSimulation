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
        int dim;

        public CentralFloor()
        {
            createGrid();
            //createGraph();
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
            dim = map.height;
            Color32[] pixels = map.GetPixels32();

            Color32[,] trueGrid = new Color32[dim,dim];

            //getpixels goes from bottom to top and left to right
            //our spatial reasoning is from top to bottom and from left to right
            for(int i = 0; i < pixels.Length; i++)
            {
                int column = i % dim;
                int row = dim - (i / dim);
                trueGrid[row, column] = pixels[i];
            }
            
        }
        //Build graph
        private void createGraph(Color32[,] map)
        {

            int[,] abs = new int[dim, dim];
            //0 is free
            //1 is obstruction
            //2 is door
            //3 is exit
            //4 is filled by the fill algorithm 

            for(int i = 0; i < dim; i++)
            {
                for(int j = 0; j < dim; j++)
                {
                    int sum = map[i, j].r + map[i, j].g + map[i, j].b;
                    if (sum == 0)
                    { abs[i, j] = 0; }
                    else if (sum == 765 || sum == map[i, j].b)
                    { abs[i, j] = 1; }
                    else if (sum == 384)
                    { abs[i, j] = 3; }
                    else
                    { abs[i, j] = 2; }
                }
            }

            //The first index represents doors
            //The second index represents exits

            //For each iteration of the algorithm, we can add a new room. Edges get a room property.
            int room = 0;

            for (int i = 0; i < dim; i++)
            {
                for(int j = 0; j < dim; j++)
                {
                    if (abs[i, j] == 0)
                    {
                        //Create the graph
                        int[] result = floodSearch(abs, i, j, new int[] { 0, 0 });

                        //Add all regular doors to the graph
                        for(int x = 0; x <= result[0]; x++)
                        {
                            //TODO add nodes and edges using the result matrix
                            //TODO give edges a room property
                        }

                        //Add all exits to the graph
                        for (int x = 0; x <= result[1]; x++)
                        {
                            //TODO add nodes and edges using the result matrix
                            //TODO give edges a room property
                        }

                        room++;
                    }
                }
            }
            
        }

        //A recursive floodsearch algorithm for creating a graph from the grid
        //The j and i variables are used to find empty spaces to start the algorithm on
        //these will be increased in each run of the algorithm
        private int[] floodSearch(int[,] map, int i, int j, int[] result)
        {
            if (i >= dim || i < 0 || j >= dim || j < 0)
            {
                return result;
            }
            //foreach adjacent
            //if door -> index 0 ++
            //if exit -> index 1 ++

            switch (map[i, j])
            {
                case 0: { map[i, j] = 4; break; }
                case 2: { result[0]++; return result; }
                case 3: { result[1]++; return result; }
                default: { return result; }
            }

            //recurse on all 4 adjacent neighbours
            floodSearch(map, i + 1, j, result);
            floodSearch(map, i - 1, j, result);
            floodSearch(map, i, j + 1, result);
            floodSearch(map, i, j - 1, result);

            return result;
        }

    }
}
