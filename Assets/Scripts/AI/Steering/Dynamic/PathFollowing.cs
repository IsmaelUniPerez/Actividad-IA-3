using UnityEngine;
using UnityEngine.AI;

namespace IaActividad1.Steering.Dynamic
{
    //Comportamiento de seguimiento de trayectorias (Path Following)
    //Hereda de 'Seek' para aprovechar la lógica de aceleración hacia un objetivo
    //Se encarga de leer una lista de puntos desde 'PathLine' y mover al agente
    //de un punto a otro secuencialmente.
    [RequireComponent(typeof(PathLine))]
    public class PathFollowing : Seek
    {
        public enum PathMode { Inspector, NavMesh } //Hacemos una pestaña para elejir el modo de seguimiento de ruta desde el Inspector de Unity
        public PathMode modoActual = PathMode.Inspector; //Pongo por defecto el Inspector
        public float targetRadius = 0.5f;//El radio por el que se consifera que hemos llegado al punto objetivo
        private PathLine pathLine;//Referencia a un componente PathLine que contiene la lista de puntos a seguir
        private int currentPointIndex = 0; //Indica hacia qué punto de la lista nos estamos dirigiendo actualmente
        protected override void Awake()
        {
            //Llamamos al Awake de la clase base (Seek) para asegurar que inicialice su Rigidbody
            base.Awake();
            pathLine = GetComponent<PathLine>();
        }
        //Se ejecuta en el primer frame. Configura la ruta inicial basándose
        //en el modo seleccionado en el Inspector
        private void Start()
        {
            //Comprobamos qué modo hemos dejado configurado en el editor de Unity
            if (modoActual == PathMode.NavMesh)
            {
                //Para usar NavMesh necesitamos saber a dónde ir. Comprobamos si hay un 'target' asignado en el script Seek
                if (target != null)
                {
                    ChangeMode(PathMode.NavMesh, target.position);
                }
                else
                {
                    Debug.LogWarning("Modo NavMesh seleccionado, pero no hay 'Target' asignado.");
                }
            }
            else
            {
                ChangeMode(PathMode.Inspector);
            }
        }
        //Calcula y devuelve la fuerza de dirección (Steering) necesaria en este frame.para esto sobreescribimos
        //la función original de Seek para adaptarla a una lista de puntos.
        public override SteeringOutput GetSteering()
        {
            //Tenemos una ruta válida por la que ir, si no, habremos llegado al final del trayecto, esto evita errores si no lo he configurado bien
            if (pathLine == null || pathLine.points.Count == 0 || currentPointIndex >= pathLine.points.Count)
            {
                return new SteeringOutput(); //Devolvemos fuerza cero (Detenerse)
            }

            //Obtenemos las coordenadas al que nos dirigimos
            Vector3 currentTargetPosition = pathLine.points[currentPointIndex];

            //Calculamos la distancia en línea recta desde el agente hasta ese punto
            float distanceToTarget = Vector3.Distance(transform.position, currentTargetPosition);

            //Comprobamos si hemos entrado en el radio en el que consideramos que hemos alcanzado el punto objetivo
            if (distanceToTarget <= targetRadius)
            {
                currentPointIndex++;
                if (currentPointIndex >= pathLine.points.Count)
                {
                    return new SteeringOutput(); //Fin de la ruta, devolvemos fuerza cero
                }

                //Actualizamos nuestro objetivo para que apunte al nuevo punto
                currentTargetPosition = pathLine.points[currentPointIndex];
            }
            return base.GetSteering(currentTargetPosition);//Le pasamos el target al Seek
        }
        //Cambia el modo de navegación
        public void ChangeMode(PathMode nuevoModo, Vector3 destinoNavMesh = default)
        {
            modoActual = nuevoModo;
            currentPointIndex = 0; //Reiniciamos el progreso al cambiar de ruta para empezar desde el principio

            if (modoActual == PathMode.Inspector)
            {
                //Volvemos a cargar los puntos manuales definidos en el editor
                pathLine.LoadEditorPath();
            }
            else if (modoActual == PathMode.NavMesh)
            {
                //Solicitamos a Unity que calcule una ruta evitando obstáculos
                SetPathFromNavMesh(destinoNavMesh);
            }
        }
        //Calcula una ruta segura usando la Inteligencia Artificial del motor (NavMesh)
        private void SetPathFromNavMesh(Vector3 destination)
        {
            NavMeshPath navPath = new NavMeshPath();

            //CalculatePath devuelve 'true' si encuentra un camino posible hasta el destino
            if (NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, navPath))
            {
                pathLine.LoadNavMeshPath(navPath);
            }
            else
            {
                Debug.LogWarning("No se pudo encontrar una ruta.");
            }
        }
    }
}