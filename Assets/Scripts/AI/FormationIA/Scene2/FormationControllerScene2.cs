using UnityEngine;
using System.Collections.Generic;
using System;

public class FormationControllerScene2 : MonoBehaviour
{
    [Header("Arquitectura Coordinadora")]
    public FormationManager formationManager;
    public Blackboard blackboard;

    [Header("Configuración del Escuadrón")]
    public List<GameObject> team;

    [Header("Sensores del Experto")]
    public float proximityThreshold = 4f;
    public LayerMask groundLayer;

    void Start()
    {
        if (formationManager == null || blackboard == null)
        {
            Debug.LogError("Faltan referencias en el Inspector (FormationManager o Blackboard).");
            return;
        }
        formationManager.SetPattern(new CircleFormation(4.5f, 10));

        foreach (GameObject agent in team)
        {
            if (agent != null) formationManager.AddAgent(agent);
        }
        //al añadir los agentes a la formación les registramos los expertos sensores y ejecutores en la blackboard

        //Cada agente tendrá un sensor para escribir en la blackboard con los parámetros puestos en éste script
        blackboard.RegisterExpert(new ClickSensorExpert(blackboard, team, proximityThreshold, groundLayer));
        //Cada agente a su vez también tendrá un experto que reaccionará a la información de la blackboard
        blackboard.RegisterExpert(new CommonTargetExpert(formationManager));
    }

    void Update()//las acciones se ejecutarán desde el Update del FormationControllerScene2, que es el encargado de evaluar la blackboard y ejecutar las acciones de los expertos elegidos por el árbitro
    {
        if (blackboard == null) return;
        Action[] choosenActions = blackboard.EvaluateExperts();

        if (choosenActions != null)
        {
            foreach (var action in choosenActions)
            {
                action?.Invoke();
            }
        }
    }
}