using UnityEngine;

namespace IaActividad1.Steering.Dynamic
{
    public class LookWhereYoureGoing : Align
    {
        public override SteeringOutput GetSteering()
        {
            //Se extrae la dirección según su velocidad actual 
            Vector3 velocity = agent.linearVelocity;

            //Si la velocidad del agente es igual a cero, no devolver nada
            if (velocity.magnitude < 0.01f)
            {
                return new SteeringOutput();
            }

            //La orientación_objetivo del agente es el arco tangente de la velocidad del agente; un ángulo
            float targetOrientation = Mathf.Atan2(velocity.x, velocity.z) * Mathf.Rad2Deg;

            return base.GetSteering(targetOrientation);
        }
    }
}
