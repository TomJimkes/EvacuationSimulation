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
        int dim;

        void Start()
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
                int row = dim - (i / dim) - 1;
                trueGrid[column, row] = pixels[i];
            }

            GameObject floorPrefab = Resources.Load<GameObject>("Prefabs/Floor");
            var floorGameObject = Instantiate(floorPrefab);
            fGrid = floorGameObject.GetComponent<FloorGrid>();
            floorGameObject.GetComponent<FloorGrid>().grid = trueGrid;

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

            //TODO, we keep a central array oject containing coordinates with a door id, and two integers representing adjacent rooms 
            int[,,] found = new int[dim,dim,4]; //The last index indicates wether it is a door or and exit

            //The first index represents doors
            //The second index represents exits

            //The counting of rooms starts at 1, 0 is outside
            int room = 1;
            //Node id's also start from 1
            int centralID = 1;

            for (int i = 0; i < dim; i++)
            {
                for(int j = 0; j < dim; j++)
                {
                    if (abs[i, j] == 0)
                    {
                        //Create a grid with door id's, types and adjacencies
                        floodSearch(abs, i, j, found, room, centralID);
                        room++;
                    }
                }
            }

            //Now we have a grid with door id's, adjacencies and types
            
            
        }

        //A recursive floodsearch algorithm for creating a graph from the grid
        //The j and i variables are used to find empty spaces to start the algorithm on
        //these will be increased in each run of the algorithm
        private void floodSearch(int[,] map, int i, int j, int[,,] found, int room, int centralID)
        {
            if (i >= dim || i < 0 || j >= dim || j < 0)
                return;

            switch (map[i, j])
            {
                case 0: { map[i, j] = 4; break; }
                case 2: { centralID = doorFound(i, j, found, room, centralID, 2); break; } 
                case 3: { centralID = doorFound(i, j, found, room, centralID, 3); break; }
                default: { return ; }
            }

            //recurse on all 4 adjacent neighbours
            floodSearch(map, i + 1, j, found, room, centralID);
            floodSearch(map, i - 1, j, found, room, centralID);
            floodSearch(map, i, j + 1, found, room, centralID);
            floodSearch(map, i, j - 1, found, room, centralID);
        }

        //If a door is found in the grid
        private int doorFound(int i, int j, int[,,] found, int room, int centralID, int type)
        {
            if (found[i, j, 0] != 0) //If already has an ID
            {
                found[i, j, 2] = room;
            }
            else
            {
                if (adjID(found, i, j) != -1)
                    found[i, j, 2] = adjID(found, i, j);
                else
                {
                    found[i, j, 0] = centralID;
                    found[i, j, 1] = room;
                    found[i, j, 3] = type;
                    centralID++;
                }
            }
            return centralID;
        }

        //If an adjacent square has a door field that already has an ID
        private int adjID(int[,,] found, int i, int j)
        {
            for(int x = -1; i <= 1; i++)
            {
                for(int y = -1; j <= 1; j++)
                {
                    int dX = i + x;
                    int dY = j + y;
                    if(!(dY < 0 || dX < 0 || dY >= dim || dX >= dim))
                    {
                        if (found[dX, dY, 0] != 0)
                            return found[dX, dY, 0];
                    }
                }
            }

            return -1;
        }
        
        //We have an augmented grid of doors and exits, with this, we can build a graph
        private void createFromFound(int[,,] found)
        {
            Dictionary<int, List<int>> roomsPerDoor = new Dictionary<int, List<int>>();
            Dictionary<int, List<int>> doorsPerRoom = new Dictionary<int, List<int>>();
            Dictionary<int, int> typePerDoor = new Dictionary<int, int>();
            for(int i = 0; i < dim; i++)
            {
                for(int j = 0; j < dim; j++)
                {
                    if(found[i,j,0] != 0) //If there is a door
                    {
                        int id = found[i, j, 0];
                        if(!roomsPerDoor.ContainsKey(id))
                        {
                            roomsPerDoor.Add(id, new List<int> { found[i, j, 1], found[i, j, 2] });
                            if(!(doorsPerRoom.ContainsKey(found[i, j, 1])))
                            {
                                doorsPerRoom.Add(found[i, j, 1], new List<int> { id });
                            }
                            else
                            {
                                doorsPerRoom[found[i, j, 1]].Add(id);
                            }
                            if (!(doorsPerRoom.ContainsKey(found[i, j, 2])))
                            {
                                doorsPerRoom.Add(found[i, j, 2], new List<int> { id });
                            }
                            else
                            {
                                doorsPerRoom[found[i, j, 2]].Add(id);
                            }
                        }
                    }
                }
            }

            //After we have built the two new datastructures, we can finally create the graph using DFS
            //TODO: DFS IMPLEMENTATION
        }


    }
}
