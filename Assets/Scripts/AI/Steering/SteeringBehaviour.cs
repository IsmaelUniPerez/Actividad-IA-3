using UnityEngine;
namespace IaActividad1.Steering
{
        public abstract class SteeringBehaviour : MonoBehaviour
        {
            public abstract SteeringOutput GetSteering();
        }
}
