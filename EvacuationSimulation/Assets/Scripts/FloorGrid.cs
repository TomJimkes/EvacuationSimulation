using UnityEngine;

namespace EvacuationSimulation
{
    public class FloorGrid : MonoBehaviour
    {
        public Color32[,] grid;

        void Start()
        {
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
                            t = Instantiate(tilePrefabs[0]);
                            break;
                    }
                    t.transform.localPosition = new Vector3(x, y);
                    t.transform.parent = transform;
                }
            }
        }
    }
}
