using UnityEngine;

namespace IaActividad1.Steering.Dynamic
{
    public class Seek : SteeringBehaviour
    {
        [SerializeField] protected float maxAcceleration = 20f;
        [SerializeField] protected Transform target;

        protected Rigidbody agentRb;

        protected virtual void Awake()
        {
            agentRb = GetComponent<Rigidbody>();
        }

        protected SteeringOutput GetSteering(Vector3 targetPosition)
        {
            SteeringOutput sOut = new SteeringOutput();

            sOut.linear = targetPosition - transform.position;

            
            sOut.linear = sOut.linear.normalized * maxAcceleration;

            sOut.angular = 0f;

            return sOut;
        }

        public override SteeringOutput GetSteering()
        {
            return GetSteering(target.position);
        }
    }
}