using UnityEngine;

namespace IaActividad1.Steering.Dynamic
{
    public class Align : SteeringBehaviour
    {
        protected Rigidbody agent;
        [SerializeField] protected Transform target;

        [SerializeField] protected float maxRotation = 100f; //rotación_máxima
        [SerializeField] protected float maxAngularAcceleration = 50f; //aceleración_angular_máxima
        [SerializeField] protected float targetRadius = 1f; //margen_de_entrada
        [SerializeField] protected float slowRadius = 15f; //margen_de_desaceleración
        [SerializeField] protected float timeToTarget = 0.1f; // tiempo_al_objetivo

        protected virtual void Awake()
        {
            agent = GetComponent<Rigidbody>();
        }

        protected SteeringOutput GetSteering(float targetOrientation)
        {
            SteeringOutput sOut = new SteeringOutput();
            sOut.linear = Vector3.zero;

            //Primero obtenemos la orientación del agente
            float currentOrientation = agent.transform.eulerAngles.y;

            //El error_angular es la resta de la orientación del agente a la orientación del objetivo
            //Se mapea el error_angular a un rango entre -180 y 180 grados
            float errorAngular = Mathf.DeltaAngle(currentOrientation, targetOrientation);

            //El valor absoluto del error_angular, por lo tanto, será la magnitud_de_rotación
            float rotationSize = Mathf.Abs(errorAngular);

            //Si la magnitud_de_rotación es menor que el margen_de_entrada, no devolver nada
            if (rotationSize < targetRadius)
            {
                sOut.angular = 0f;
                return sOut;
            }

            float targetRotation;
            //Si el agente se encuentra dentro del margen de desaceleración, ajustar la rotación
            if (rotationSize < slowRadius)
            {
                //La rotación_objetivo es proporcional a la magnitud_de_rotación
                targetRotation = maxRotation * (rotationSize / slowRadius);
            }
            else
            {
                //Si no, la rotación_objetivo será igual a la rotación_máxima
                targetRotation = maxRotation;
            }

            //Aplicar a la rotación_objetivo el mismo signo + y - que el error_angular
            targetRotation *= Mathf.Sign(errorAngular);

            //Diferencia con la rotación del agente, ajustada para llegar en el tiempo_al_objetivo y convierto los radianes a grados
            float currentRotationSpeed = agent.angularVelocity.y * Mathf.Rad2Deg; 
            sOut.angular = targetRotation - currentRotationSpeed;
            sOut.angular /= timeToTarget;

            //Para que el agente no rote más de lo permitido
            float angularAcceleration = Mathf.Abs(sOut.angular);
            if (angularAcceleration > maxAngularAcceleration)
            {
                sOut.angular = (sOut.angular / angularAcceleration) * maxAngularAcceleration;
            }

            return sOut;
        }

        public override SteeringOutput GetSteering()
        {
            //Align base iguala su orientación a la rotación Y del objetivo
            return GetSteering(target.eulerAngles.y);
        }
    }
}