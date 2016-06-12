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

        //Used for construction
        int centralID;

        void Start()
        {
            Color32[,] grid = createGrid();
            createGraph(grid);
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
        private Color32[,] createGrid()
        {
            //Read from file
            //build grid
            Texture2D map = Resources.Load<Texture2D>("Maps/Test2");
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
            return trueGrid;
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
                    if (sum == 765) //White
                    { abs[i, j] = 0; }
                    else if (sum == 0 || sum == map[i, j].b) //Black/blue
                    { abs[i, j] = 1; }
                    else if (sum == 381) //Grey
                    { abs[i, j] = 3; }
                    else //Yellow/Red
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
            centralID = 1;

            for (int i = 0; i < dim; i++)
            {
                for(int j = 0; j < dim; j++)
                {
                    if (abs[i, j] == 0)
                    {
                        //Create a grid with door id's, types and adjacencies
                        floodSearch(abs, i, j, found, room);
                        room++;
                    }
                }
            }

            //TEST
            for(int i = 0; i < dim; i++)
            {
                string s = "";
                for(int j = 0; j < dim; j++)
                {
                    s += found[i, j, 0];
                }
                print(s);
            }

            createFromFound(found);
            //ENDTEST
            //Now we have a grid with door id's, adjacencies and types
            
            
        }

        //A recursive floodsearch algorithm for creating a graph from the grid
        //The j and i variables are used to find empty spaces to start the algorithm on
        //these will be increased in each run of the algorithm //TS
        private void floodSearch(int[,] map, int i, int j, int[,,] found, int room)
        {
            if (i >= dim || i < 0 || j >= dim || j < 0)
                return;

            switch (map[i, j])
            {
                case 0: { map[i, j] = 4; break; }
                case 2: { doorFound(i, j, found, map, room, 2); return; } 
                case 3: { doorFound(i, j, found, map, room, 3); return; }
                default: { return ; }
            }

            //recurse on all 4 adjacent neighbours
            floodSearch(map, i, j + 1, found, room);
            floodSearch(map, i, j - 1, found, room);
            floodSearch(map, i + 1, j, found, room);
            floodSearch(map, i - 1, j, found, room);
        }

        //If a door is found in the grid //TS
        private void doorFound(int i, int j, int[,,] found, int[,]map, int room, int type)
        {
            if (found[i, j, 0] != 0) //If already has an ID
            {
                if(!(found[i, j, 1] == room))
                    found[i, j, 2] = room;
            }
            else
            {
                List<int[]> door = new List<int[]>();
                int count = 1;
                door.Add(new int[] { i, j });

                //find all coordinates that are part of the door, find the right id
                if(j + count < dim && map[i, j + count] == type) //If we find a piece of door in j+1, iterate
                {
                    while(map[i, j + count] == type) //While we are still on a door square
                    {
                        door.Add(new int[] { i, j + count });
                        count++;
                        if (j + count >= dim)
                            break;
                        
                    }
                    count = 1;
                }
                if (i + count < dim && map[i + count, j] == type)
                {
                    while (map[i + count, j] == type)
                    {
                        door.Add(new int[] { i + count, j});
                        count++;
                        if (i + count >= dim)
                            break;
                        
                    }
                    count = 1;
                }
                if (j - count >= 0 && map[i, j - count] == type)
                {
                    while (map[i, j - count] == type)
                    {
                        door.Add(new int[] { i, j - count });
                        count++;
                        if (j - count < 0)
                            break;
                        
                    }
                    count = 1;
                }
                if (i - count >= 0 && map[i - count, j] == type)
                {
                    while (map[i - count, j] == type)
                    {
                        door.Add(new int[] { i - count, j });
                        count++;
                        if (i - count < 0)
                            break;
                    }
                    count = 1;
                }

                //Set all values of the door in the found 
                for(int n = 0; n < door.Count; n++)
                {
                    found[door[n][0], door[n][1], 0] = centralID;
                    found[door[n][0], door[n][1], 1] = room;
                    found[door[n][0], door[n][1], 3] = type;
                }
                centralID++;
            }
        }
        
        //We have an augmented grid of doors and exits, with this, we can build a graph //TS
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
                            typePerDoor.Add(id, found[i, j, 3]);
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
            string s = "";
            //After we have built the two new datastructures, we can finally create the graph using DFS
            //TODO: DFS IMPLEMENTATION
        }


    }
}
