using UnityEngine;

namespace IaActividad1.Steering.Dynamic
{
    public class Separation : SteeringBehaviour
    {
        [SerializeField] protected float maxAcceleration = 10f;
        [SerializeField] protected float satisfactionRadius = 5f; //radio_de_satisfacción o detección
        //Usamos una LayerMask para que el detecte lo marcado como obstáculo (esto lo hago para reutilizar la capa de obstáculos pero serían otros agentes)
        [SerializeField] protected LayerMask obstacleLayer;

        [SerializeField] protected float brakingCoefficient = 10f; //coeficiente_de_frenada para la inversa del cuadrado

        public override SteeringOutput GetSteering()
        {
            SteeringOutput sOut = new SteeringOutput();
            sOut.linear = Vector3.zero;
            sOut.angular = 0f;

            //Creamos un array de colliders para almacenar los obstáculos encontrados dentro del radio de satisfacción usando Physics.OverlapSphere
            //báscamente creará una esfera invisible alrededor del agente y detectará todos los obstaculos añadiendolos al array de colliders
            Collider[] obstacles = Physics.OverlapSphere(transform.position, satisfactionRadius, obstacleLayer);

            //Se itera sobre cada obstáculo encontrado 
            foreach (Collider obs in obstacles)
            {
                //Esto lo añadí para evitar que el agente se considere a sí mismo como un obstáculo ya que
                //cuando lo probé el agente se empujaba a sí mismo y daba un comportamiento extraño.
                if (obs.gameObject == gameObject) continue;
                //El vector dirección es la resta de la posición del agente a la posición del obstáculo
                Vector3 direction = transform.position - obs.transform.position;
                //La distancia entre agentes es la longitud del vector dirección
                float distance = direction.magnitude;
                //Si están exactamente en la misma coordenada, los empujamos al azar,
                //así evito que sì por cualquier motivo dos agentes se encuentren en la misma posición,
                //el algoritmo no se quede atascado sin poder calcular una dirección de separación.
                if (distance == 0f)
                {
                    direction = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
                    distance = 0.01f;
                }

                //Si la distancia es menor que el radio_de_satisfacción
                if (distance < satisfactionRadius)
                {
                    float repulsionForce = 0f;
                    repulsionForce = maxAcceleration * (satisfactionRadius - distance) / satisfactionRadius;

                    //Se suman los resultados individuales si hay varios obstáculos
                    //sout.lineal += calcular la fuerza_de_repulsión y multiplicarla por la dirección normalizada
                    sOut.linear += direction.normalized * repulsionForce;
                }
            }
            return sOut;
        }
    }
}