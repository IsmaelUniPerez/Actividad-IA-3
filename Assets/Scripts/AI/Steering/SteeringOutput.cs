using UnityEngine;

namespace IaActividad1.Steering
{
    public struct SteeringOutput
    {
        public Vector3 linear;
        public float angular;

        public SteeringOutput(Vector3 linear, float angular)
        {
            this.linear = linear;
            this.angular = angular;
        }
    }
}
