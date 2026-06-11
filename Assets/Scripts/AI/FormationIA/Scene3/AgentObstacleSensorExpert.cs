using UnityEngine;
using System.Collections.Generic;
using System.Timers;

public class AgentObstacleSensor : MonoBehaviour
{
    [Header("Blackboard")]
    public Blackboard blackboard;

    [Header("Sensors")]
    public float visionDistance = 5f; //Distancia máxima a la que el agente puede detectar obstáculos
    public LayerMask obstacleLayer; //La capa que representa los obstáculos para el Raycast
    public Vector3 raycastOffset = new Vector3(0, 0.5f, 0);//El raycast que lanzamos

    void Update()
    {
        if (blackboard == null) return;//por si acaso se nos olvida asignar la blackboard en el inspector
        //Realizamos el raycast muy similar a como configuramos el ObstacleAndWallAvoidance en la actividad 1
        //sin embargo ésta vez en vez de detectar el obstáculo y moverse, lo que hará será escribir lo que detecte en 
        //la blackboard para que cualquier experto que necesite esa información pueda acceder a ella, como por ejemplo el AvoidObstacleExpert de esta misma escena
        Vector3 origin = transform.position + raycastOffset;
        Vector3 direction = transform.forward;

        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit, visionDistance, obstacleLayer))
        {
            List<GameObject> knownObstacles = blackboard.GetDataByKey<List<GameObject>>("ObstacleList");//aquí obtenemos la lista

            if (knownObstacles != null && !knownObstacles.Contains(hit.collider.gameObject))
            {
                knownObstacles.Add(hit.collider.gameObject);//una vez tenemos la lista (incluidos los obstáculos ya detectados) añadimos el nuevo obstáculo detectado por el raycast
                blackboard.AddData("ObstacleList", knownObstacles);//vamos a la blackboard y escribimos la lista actualizada con el nuevo obstáculo

                Debug.Log("Agent " + gameObject.name + " detected an obstacle: " + hit.collider.gameObject.name);
            }
        }
    }
}