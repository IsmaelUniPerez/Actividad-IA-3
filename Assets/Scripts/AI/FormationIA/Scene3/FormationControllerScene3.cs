using UnityEngine;
using System.Collections.Generic;

public class FormationControllerScene3 : MonoBehaviour
{
    [Header("Arquitectura")]
    public FormationManager formationManager;

    [Header("Configuración del Escuadrón")]
    public List<GameObject> team;
    public float spaceBetweenAgents = 3f;
    public int limitAgents = 10;

    [Header("Motor de la Formación")]
    [Tooltip("Dirección hacia la que caminará todo el grupo")]
    public Vector3 advanceDirection = new Vector3(0, 0, 1);
    [Tooltip("Velocidad a la que se desplaza la geometría geométrica")]
    public float formationSpeed = 3f;

    void Start()
    {
        if (formationManager == null)
        {
            Debug.LogError("Asigna el FormationManager en el Inspector.");
            return;
        }
        formationManager.SetPattern(new WedgeFormation(spaceBetweenAgents, spaceBetweenAgents, limitAgents));
        foreach (GameObject agent in team)
        {
            if (agent != null) formationManager.AddAgent(agent);
        }
    }

    void Update()
    {
        if (formationManager != null)
        {
            formationManager.globalAnchor += advanceDirection.normalized * formationSpeed * Time.deltaTime;
        }
    }
}