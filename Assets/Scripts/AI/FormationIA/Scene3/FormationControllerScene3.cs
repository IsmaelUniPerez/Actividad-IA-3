using UnityEngine;
using System.Collections.Generic;
using System;

public class FormationControllerScene3 : MonoBehaviour
{
    [Header("Arquitectura Coordinadora")]
    public FormationManager formationManager;
    public Blackboard blackboard;

    [Header("Configuración del Escuadrón")]
    public List<GameObject> team;
    public float spaceBetweenAgents = 3f;
    public int limitAgents = 10;

    [Header("Configuración del Movimiento de la formación")]
    public Vector3 advanceDirection = new Vector3(0, 0, 1);
    public float formationSpeed = 3f;

    [Header("Configuración de Expertos de Evitación")]
    public float sensorDistance = 5f;
    public float avoidanceRadius = 4f;
    public float offsetIntensity = 3.5f;
    public LayerMask obstacleLayer;

    void Start()
    {
        if (formationManager == null || blackboard == null) return;
        blackboard.AddData("ObstacleList", new List<GameObject>());//si tenemos ya una variable en la blackboard con el mismo nombre, la sobreescribimos, si no la añadimos
        formationManager.SetPattern(new LineFormation(spaceBetweenAgents, limitAgents));//le decimos la formación que usamos

        foreach (GameObject agent in team)
        {
            if (agent != null) formationManager.AddAgent(agent);
        }
        //Registramos los expertos de cada agente que añadimos a la formación y les pasamos los parámetros que hemos configurado en el inspector
        blackboard.RegisterExpert(new ObstacleAvoidanceExpert(team, avoidanceRadius, offsetIntensity));
    }

    void Update()//Cada frame moveremos el pivote de la formación hacia la dirección que hemos puesto
    {
        if (formationManager != null)
        {
            formationManager.globalAnchor += advanceDirection.normalized * formationSpeed * Time.deltaTime;
        }
    }
    void LateUpdate()//En el update vamos a evaluar la blackboard y ejecutamos las acciones que nos dice el experto elegido por el árbitro
    {
        if (blackboard == null) return;

        Action[] chosenActions = blackboard.EvaluateExperts();
        if (chosenActions != null)
        {
            foreach (var action in chosenActions)
            {
                action?.Invoke();
            }
        }
    }
}