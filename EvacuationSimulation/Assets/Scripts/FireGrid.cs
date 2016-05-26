using UnityEngine;

namespace EvacuationSimulation
{
    class FireGrid : MonoBehaviour
    {
        GameObject[,] fireObjects = new GameObject[5,5];
        Sprite[] fireSprites = new Sprite[3];
        GameObject firePrefab;

        void Start()
        {
            fireSprites[0] = Resources.Load<Sprite>("Sprites/Fire1");
            fireSprites[1] = Resources.Load<Sprite>("Sprites/Fire2");
            fireSprites[2] = Resources.Load<Sprite>("Sprites/Fire3");

            firePrefab = Resources.Load<GameObject>("Prefabs/Fire");
            var f = Instantiate(firePrefab);
            f.transform.localPosition = new Vector3(2, 2);
            f.transform.parent = transform;
            fireObjects[2, 2] = f;
            f.GetComponent<Fire>().fireSprites = fireSprites;
        }

        void Update()
        {
            
        }
    }
}
