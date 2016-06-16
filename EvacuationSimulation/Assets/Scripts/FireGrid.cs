using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EvacuationSimulation
{
    public class FireGrid : MonoBehaviour
    {
        GameObject[,] fireObjects = new GameObject[50, 50];
        GameObject firePrefab;
        private float time;

        void Start()
        {
            firePrefab = Resources.Load<GameObject>("Prefabs/Fire");
            CreateFire(new Vector2(25, 25));
        }

        void Update()
        {
            time += Time.deltaTime;
            if (time > 1)
            {
                UpdateFire();
                time = 0;
            }
        }

        void UpdateFire()
        {
            foreach (var fireObject in fireObjects)
            {
                if (fireObject != null && fireObject.GetComponent<Fire>().state == 2)
                {
                    var surrounding = GetSurroundingSquares(fireObject.transform.localPosition);
                    foreach (var surroundingSquare in surrounding)
                    {
                        if (this[surroundingSquare] == null && Random.value > 0.92)
                            CreateFire(surroundingSquare);

                    }
                }
            }
        }

        void CreateFire(Vector2 location)
        {
            var f = Instantiate(firePrefab);
            f.transform.localPosition = location;
            f.transform.parent = transform;
            fireObjects[(int)location.x, (int)location.y] = f;
        }

        List<Vector2> GetSurroundingSquares(Vector2 square)
        {
            List<Vector2> result = new List<Vector2>();
            for (int x = -2; x <= 2; x++)
            {
                for (int y = -2; y <= 2; y++)
                {
                    var current = new Vector2(square.x + x, square.y + y);
                    if (Vector2.Distance(square, current) > 2.5)
                        continue;
                    if (x == 0 && y == 0)
                        continue;
                    if (current.x < 0 || current.x > fireObjects.GetLength(0) - 1)
                        continue;
                    if (current.y < 0 || current.y > fireObjects.GetLength(1) - 1)
                        continue;
                    result.Add(current);
                }
            }

            return result;
        }

        GameObject this[Vector2 v]
        {
            get { return fireObjects[(int)v.x, (int)v.y]; }
        }

        GameObject this[int x, int y]
        {
            get { return fireObjects[x, y]; }
        }
    }
}
