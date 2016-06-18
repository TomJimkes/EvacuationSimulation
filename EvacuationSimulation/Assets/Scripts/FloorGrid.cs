using System.Collections.Generic;
//using Algorithms;
using UnityEngine;

namespace EvacuationSimulation
{
    public class FloorGrid : MonoBehaviour
    {
        public Color32[,] grid;
        public PathFinder PathFinder;

        void Start()
        {
            //changed to byte for pathfinding purposes, shouldn't change storage space
            //0 is impassable
            //1 is passable
            //2 is door
            var pathfindGrid = new byte[grid.GetLength(0), grid.GetLength(1)];

            GameObject[] tilePrefabs = new GameObject[3];
            tilePrefabs[0] = Resources.Load<GameObject>("Prefabs/Floortile");
            tilePrefabs[1] = Resources.Load<GameObject>("Prefabs/Walltile");
            tilePrefabs[2] = tilePrefabs[0];
            
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(0); y++)
                {
                    GameObject t;
                    switch (grid[x, y].r)
                    {
                        case 0:
                            t = Instantiate(tilePrefabs[1]);
                            break;
                        default:
                            pathfindGrid[x, y] = 1;
                            t = Instantiate(tilePrefabs[0]);
                            break;
                    }
                    t.transform.localPosition = new Vector3(x, y);
                    t.transform.parent = transform;
                }
            }

            PathFinder = new PathFinder(pathfindGrid);

            

        }
    }
}
