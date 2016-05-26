using UnityEngine;

namespace EvacuationSimulation
{
    enum TileType
    { Empty, Wall, Other }

    class FloorGrid : MonoBehaviour
    {
        TileType[,] grid = new TileType[5, 5];

        void Start()
        {
            GameObject[] tilePrefabs = new GameObject[3];
            tilePrefabs[0] = Resources.Load<GameObject>("Prefabs/Floortile");
            tilePrefabs[1] = Resources.Load<GameObject>("Prefabs/Walltile");
            tilePrefabs[2] = tilePrefabs[0];

            grid[2,2] = TileType.Wall;
            grid[2,3] = TileType.Wall;
            grid[2,4] = TileType.Wall;

            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(0); y++)
                {
                    var t = Instantiate(tilePrefabs[(int)grid[x, y]]);
                    t.transform.localPosition = new Vector3(x, y);
                    t.transform.parent = transform;
                }
            }
        }
    }
}
