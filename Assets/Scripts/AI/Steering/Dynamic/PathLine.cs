using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace IaActividad1.Steering.Dynamic
{
    //Componente que actúa como contenedor y traductor de rutas
    //su responsabilidad es proporcionar una lista genérica de coordenadas (Vector3)
    //independientemente de si la ruta se creó manualmente en el editor o fue
    //calculada dinámicamente por el sistema NavMesh de Unity
    public class PathLine : MonoBehaviour
    {
        //Puntos de control definidos manualmente en el editor
        public List<Transform> editorPoints;

        //Puntos de la trayectoria activa, representados como vectores en el espacio,
        //esta lista se actualiza ya sea desde los puntos del editor o
        //desde las esquinas de un NavMeshPath
        public List<Vector3> points;

        // Inicializa el componente y carga los puntos definidos en el editor
        void Awake()
        {
            LoadEditorPath();
        }
        public void LoadEditorPath()
        {
            //Nos aseguramos que no hay puntos anteirores
            points.Clear();

            //Pasamos los puntos de la lista del editor a la lista de puntos que usaremos luego
            foreach (Transform point in editorPoints)
            {
                points.Add(point.position);
            }
        }


        //Carga una trayectoria generada por el sistema NavMesh
        public void LoadNavMeshPath(NavMeshPath navMeshPath)
        {
            //Primero, aseguramos que la lista 'points' esté limpia.
            points.Clear();

            //Copiamos los puntos de la trayectoria
            points.AddRange(navMeshPath.corners);
        }
    }
}