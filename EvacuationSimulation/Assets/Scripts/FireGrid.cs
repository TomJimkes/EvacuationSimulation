using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EvacuationSimulation
{
    public class FireGrid : MonoBehaviour
    {
        GameObject[,] fireObjects;
        GameObject firePrefab;
        private float time;

        void Start()
        {
            fireObjects = new GameObject[GameManager.Instance.RealFloorGrid.grid.GetLength(0), 
                                         GameManager.Instance.RealFloorGrid.grid.GetLength(1)];
            firePrefab = Resources.Load<GameObject>("Prefabs/Fire");
            CreateFire(new Vector2(25, 25));
        }

        void Update()
        {
            time += Time.deltaTime;
            if (time > 2)
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
                        var burnability =
                            1 - GameManager.Instance.RealFloorGrid.grid[(int)surroundingSquare.x, (int)surroundingSquare.y].a / 255;
                        var distance = 
                            1 - Vector2.Distance(fireObject.transform.localPosition, surroundingSquare) / 2.5;
                        if (Random.value < distance * burnability * 0.1)
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
            this[location] = f;
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
                    if (current.x < 0 || current.x > fireObjects.GetLength(0) - 1)
                        continue;
                    if (current.y < 0 || current.y > fireObjects.GetLength(1) - 1)
                        continue;
                    if (this[current] != null)
                        continue;
                    result.Add(current);
                }
            }

            return result;
        }

        GameObject this[Vector2 v]
        {
            get {
                try
                {
                    return fireObjects[(int) v.x, (int) v.y];

                }
                catch
                {
                    return null;
                } }
            set { fireObjects[(int) v.x, (int) v.y] = value; }
        }

        GameObject this[int x, int y]
        {
            get { return fireObjects[x, y]; }
        }
    }
}
