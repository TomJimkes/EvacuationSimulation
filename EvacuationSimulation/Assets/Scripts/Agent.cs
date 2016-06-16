using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

namespace EvacuationSimulation
{
    public abstract class Agent : MonoBehaviour
    {
        private object physicalState;
        private object mentalState;
        private Floor mentalMap;
        private List<PathFinderNode> currentPath;
        private float speed = 5f;
        public Vector2 target;

        public bool Test;

        void Update()
        {

            if (target != Vector2.zero && Test)
            {
                var targetPoint = new Point((int) target.x, (int) target.y);
                var currentPoint = new Point((int)transform.localPosition.x, (int)transform.localPosition.y);
                currentPath = GameManager.Instance.GetComponent<CentralFloor>().Grid.PathFinder.FindPath(currentPoint, targetPoint);
                target = Vector2.zero;
            }

            if (currentPath != null && currentPath.Any())
            {
                var nextSpace = currentPath.First();
                var nextVector = new Vector3(nextSpace.X, nextSpace.Y, transform.localPosition.z);
                transform.position = Vector3.MoveTowards(transform.position, nextVector,
                    Time.deltaTime * speed);

                if (Vector3.Distance(transform.localPosition, nextVector) < 0.0001f)
                {
                    currentPath.Remove(nextSpace);
                }
            }
        }
    }
}
