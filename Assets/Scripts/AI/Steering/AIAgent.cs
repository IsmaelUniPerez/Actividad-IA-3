using UnityEngine;

namespace IaActividad1.Steering
{
    [RequireComponent(typeof(Rigidbody))]
    public class AIAgent : MonoBehaviour
    {
        [SerializeField] private SteeringBehaviour behaviour;
        private Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (behaviour == null) return;

            SteeringOutput steering = behaviour.GetSteering();

            //Aplicamos fuerza lineal (Seek, Pursue...)
            rb.AddForce(steering.linear, ForceMode.Acceleration);

            //Aplicamos fuerza de giro (Align, Face...)
            //Unity espera radianes para las fuerzas angulares, así que convertimos nuestros grados
            Vector3 torque = Vector3.up * (steering.angular * Mathf.Deg2Rad);
            rb.AddTorque(torque, ForceMode.Acceleration);
        }
    }
}