using System.Collections.Generic;
using UnityEngine;

namespace IaActividad1.Steering.Dynamic
{
    public class BlendedSteering : SteeringBehaviour
    {
        public List<BehaviorAndWeight> behaviors = new List<BehaviorAndWeight>(); //Esta lista se puede configurar desde el Inspector de Unity, cada elemento es un par de comportamiento y peso
        [SerializeField] private float maxAcceleration = 20f; //Estos límites actuan por encima de los otros comportamientos, es decir se establece un límite máximo general
        [SerializeField] private float maxAngularAcceleration = 50f;

        public override SteeringOutput GetSteering()
        {
            SteeringOutput result = new SteeringOutput();

            //Comprobamos todos los comportamientos de la lista
            foreach (BehaviorAndWeight bw in behaviors)
            {
                if (bw.behavior != null)
                {
                    //Obtenemos la fuerza de cada comportamiento
                    SteeringOutput s = bw.behavior.GetSteering();

                    //Sumamos sus fuerzas multiplicadas por su peso (fórmula del Blending)
                    result.linear += s.linear * bw.weight;
                    result.angular += s.angular * bw.weight;
                }
            }

            //La suma ponderada puede superar la capacidad física del agente
            //por lo tanto se normaliza para que no supere los límites establecidos
            if (result.linear.magnitude > maxAcceleration)
            {
                result.linear = result.linear.normalized * maxAcceleration;
            }
            if (Mathf.Abs(result.angular) > maxAngularAcceleration)
            {
                result.angular = Mathf.Sign(result.angular) * maxAngularAcceleration;
            }
            return result;
        }
        //Para poder modificar la lista de comportamientos y pesos desde otros scripts o desde el Inspector de Unity necesitamos métodos para agregar comportamientos y cambiar sus pesos
        public void AddBehavior(SteeringBehaviour behavior, float weight)
        {
            if (behavior == null) return;
            behaviors.Add(new BehaviorAndWeight { behavior = behavior, weight = weight });
        }

        public void SetWeight(SteeringBehaviour behavior, float newWeight)
        {
            BehaviorAndWeight bw = behaviors.Find(x => x.behavior == behavior);
            if (bw != null)
            {
                bw.weight = newWeight;
            }
            else
            {
                Debug.LogWarning("El peso no tiene asignado un comportamiento.");
            }
        }
    }
}