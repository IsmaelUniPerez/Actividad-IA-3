using UnityEngine;

namespace IaActividad1.Steering.Dynamic
{
    public class Pursue : Seek
    {
        [SerializeField] private float maxPredictionTime = 3f;
        [SerializeField] private Rigidbody targetRigidbody;

        public override SteeringOutput GetSteering()
        {
            Vector3 direction = target.position - transform.position;
            float distance = direction.magnitude;
            float speed = agentRb != null ? agentRb.linearVelocity.magnitude : 0f;

            float predictionTime;

            if (speed > 0)
            {
                predictionTime = distance / speed;

                if (predictionTime > maxPredictionTime)
                {
                    predictionTime = maxPredictionTime;
                }
            }
            else
            {
                predictionTime = maxPredictionTime;
            }

            Vector3 targetVelocity = targetRigidbody != null ? targetRigidbody.linearVelocity : Vector3.zero;
            Vector3 futurePosition = target.position + (targetVelocity * predictionTime);

            return base.GetSteering(futurePosition);
        }
    }
}