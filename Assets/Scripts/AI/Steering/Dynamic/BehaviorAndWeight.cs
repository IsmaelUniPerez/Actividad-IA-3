using System;
using UnityEngine;

namespace IaActividad1.Steering.Dynamic
{
    [Serializable]
    public class BehaviorAndWeight
    {
        public SteeringBehaviour behavior;
        [Range(0f, 1f)] public float weight = 1f;
    }
}