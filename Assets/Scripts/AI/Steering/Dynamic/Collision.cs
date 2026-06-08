using UnityEngine;

namespace IaActividad1.Steering.Dynamic
{
    public struct Collision
    {
        public Vector3 position;
        public Vector3 normal;

        public Collision(Vector3 position, Vector3 normal)
        {
            this.position = position;
            this.normal = normal;
        }
    }
}
