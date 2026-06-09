using UnityEngine;
using System.Collections.Generic;
using System;

public class FormationControllerScene2 : MonoBehaviour
{
    [Header("Arquitectura Coordinadora")]
    public FormationManager formationManager;
    [Tooltip("Arrastra aquí el componente Blackboard")]
    public Blackboard blackboard;

    [Header("Configuración del Escuadrón")]
    public List<GameObject> team;

    [Header("Sensores del Experto")]
    [Tooltip("Distancia mínima para que el clic sea válido")]
    public float proximityThreshold = 4f;
    [Tooltip("Capa que representa el suelo para el Raycast")]
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
        blackboard.RegisterExpert(new ClickSensorExpert(blackboard, team, proximityThreshold, groundLayer));
        blackboard.RegisterExpert(new CommonTargetExpert(formationManager));
    }

    void Update()
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