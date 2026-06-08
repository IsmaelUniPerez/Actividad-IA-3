using UnityEngine;

namespace IaActividad1.Steering.Dynamic
{
    public class ObstacleAndWallAvoidance : Seek
    {
        [Header("Parámetros de Evasión")]
        [SerializeField] protected float avoidDistance = 2f; //distancia_de_evasión
        [SerializeField] protected float lookAhead = 5f; //longitud_de_raycast

        //Usamos una LayerMask para que el raycast solo detecte obstáculos y no al propio agente o objetos no deseados
        [SerializeField] protected LayerMask wallLayer;
        public override SteeringOutput GetSteering()
        {
            //El raycast que se usa es el vector velocidad del agente
            Vector3 rayVector;

            //Si el agente está quieto, usamos su orientación actual para mirar al frente
            if (agentRb != null && agentRb.linearVelocity.magnitude > 0.01f)
            {
                rayVector = agentRb.linearVelocity.normalized;
            }
            else
            {
                rayVector = transform.forward;
            }

            //Comprobamos si el raycast choca con un objeto
            RaycastHit hit;
            //Lanzamos el rayo desde el centro del agente hacia la dirección de su movimiento
            if (Physics.Raycast(transform.position, rayVector, out hit, lookAhead, wallLayer))
            {
                //Encapsulamos la información de colisión
                Collision collisionInfo = new Collision(hit.point, hit.normal);

                //Generar un "falso" objetivo frente al obstáculo
                //Seek.objetivo.posición = la posición de la colisión + (el vector normal * distancia_de_evasión)
                Vector3 targetPosition = collisionInfo.position + (collisionInfo.normal * avoidDistance);

                //Delegamos el movimiento a la clase padre (Seek) pasándole esa nueva coordenada
                return base.GetSteering(targetPosition);
            }

            //Si no hay colisión con ningún objeto, hacemos un Seek normal hacia el objetivo
            if (target != null)
            {
                return base.GetSteering(target.position);
            }

            return new SteeringOutput();
        }
    }
}
