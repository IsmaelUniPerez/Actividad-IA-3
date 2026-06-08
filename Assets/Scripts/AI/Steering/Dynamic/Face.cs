using UnityEngine;

namespace IaActividad1.Steering.Dynamic
{
    public class Face : Align
    {
        public override SteeringOutput GetSteering()
        {
            //El vector dirección es la resta de la posición del agente a la posición del Face.objetivo
            Vector3 direction = target.position - agent.position;

            //La distancia entre agentes es la longitud del vector dirección
            //Si la distancia es igual a cero, no devolver nada
            if (direction.magnitude < 0.01f)
            {
                return new SteeringOutput();
            }

            //La orientación_objetivo del agente es el arco tangente del vector dirección; un ángulo
            float targetOrientation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            //return Align.getSteering()
            return base.GetSteering(targetOrientation);
        }
    }
}
