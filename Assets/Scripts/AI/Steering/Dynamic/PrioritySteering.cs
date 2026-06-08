using System.Collections.Generic;
using UnityEngine;

namespace IaActividad1.Steering.Dynamic
{
    public class PrioritySteering : SteeringBehaviour
    {
        public List<SteeringBehaviour> behaviors = new List<SteeringBehaviour>(); //Lista con los comportamientos ordenados por prioridad
        [SerializeField] private float activationLimit = 0.05f; //Limite de activación

        public override SteeringOutput GetSteering()
        {
            SteeringOutput result = new SteeringOutput();

            //Recorremos los comportamientos por orden de prioridad
            foreach (SteeringBehaviour behavior in behaviors)
            {
                if (behavior != null)
                {
                    result = behavior.GetSteering();
                    //Si el comportamiento solicita una aceleración mayor que el limite,
                    //significa que la situación requiere intervención (ej. el agente ve un muro delante)
                    if (result.linear.magnitude > activationLimit || Mathf.Abs(result.angular) > activationLimit)
                    {
                        //Cortamos la evaluación inmediatamente y devolvemos esta fuerza que está por encima del limite
                        return result;
                    }
                }
            }
            //Si el código llega aquí, significa que ningún comportamiento solicitó una aceleración significativa,
            //por lo que se devuelve un resultado sin fuerza.
            return result;
        }
    }
}