using UnityEngine;

namespace EvacuationSimulation
{
    public class FloorGrid : MonoBehaviour
    {
        bool[,] grid = new bool[5,5];

        void Start()
        {
            GameObject tile = Resources.Load<GameObject>("Prefabs/Floortile");
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(0); y++)
                {
                    var t = Instantiate(tile);
                    t.transform.localPosition = new Vector3(x, y);
                    t.transform.parent = transform;
                }
            }
        }
    }
}
