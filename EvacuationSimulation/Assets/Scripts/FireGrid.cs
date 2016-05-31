using UnityEngine;

namespace EvacuationSimulation
{
    class FireGrid : MonoBehaviour
    {
        GameObject[,] fireObjects = new GameObject[5,5];
        GameObject firePrefab;

        void Start()
        {
            firePrefab = Resources.Load<GameObject>("Prefabs/Fire");
            var f = Instantiate(firePrefab);
            f.transform.localPosition = new Vector3(2, 2);
            f.transform.parent = transform;
            fireObjects[2, 2] = f;
        }

        void Update()
        {
            
        }
    }
}
