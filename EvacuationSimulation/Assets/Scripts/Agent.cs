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
        public float _angle;

        void Update()
        {
            transform.Rotate(new Vector3(0, 0, 1), Angle);
        }

        public float Angle
        {
            get { return _angle; }
            set
            {
                transform.Rotate(new Vector3(0, 0, 1), value - _angle);
                _angle = value;
            }
        }
    }
}
