using UnityEngine;

namespace IaActividad1.Steering.Dynamic
{
    public class SeekVirtual : Seek
    {
        public Vector3 objetiveTarget;

        public override SteeringOutput GetSteering()
        {
            Vector3 adjustedObjetive = new Vector3(objetiveTarget.x, transform.position.y, objetiveTarget.z);
            SteeringOutput force = base.GetSteering(adjustedObjetive);
            return force;
        }
    }
}