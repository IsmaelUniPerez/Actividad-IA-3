using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class FormationControllerScene1 : MonoBehaviour
{
    [Header("Arquitectura")]
    public FormationManager formationManager;

    [Header("Agentes Activos")]
    public List<GameObject> team;

    [Header("Agentes en Reserva (Cola)")]
    public List<GameObject> reserveAgents;

    [Header("Ajustes Geométricos")]
    public float espacioEntreAgentes = 3f;
    public int limiteAgentes = 10;

    void Start()
    {
        if (formationManager == null) return;

        formationManager.SetPattern(new LineFormation(espacioEntreAgentes, limiteAgentes));

        // Añadimos solo a los del equipo inicial
        foreach (GameObject agent in team)
        {
            formationManager.AddAgent(agent);
        }
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        // Tecla Espacio: Meter al siguiente agente de la cola en la formación
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (reserveAgents.Count > 0)
            {
                // Cogemos al primer agente de la cola (índice 0)
                GameObject recluta = reserveAgents[0];

                // Intentamos meterlo en el gestor. Si hay hueco en el patrón, AddAgent devolverá TRUE.
                if (formationManager.AddAgent(recluta))
                {
                    Debug.Log($"Agente {recluta.name} añadido a la formación exitosamente.");

                    // Como ya está dentro, lo borramos de la cola de espera
                    reserveAgents.RemoveAt(0);
                }
                else
                {
                    // Si AddAgent devuelve FALSE (ej. el límite es 10 y ya hay 10)
                    Debug.LogWarning("Orden rechazada: La formación actual ha alcanzado su límite máximo.");
                }
            }
            else
            {
                Debug.Log("La cola de reserva está vacía. No hay más agentes para añadir.");
            }
        }
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            formationManager.SetPattern(new LineFormation(espacioEntreAgentes, limiteAgentes));
        }
        else if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            formationManager.SetPattern(new WedgeFormation(espacioEntreAgentes, espacioEntreAgentes, limiteAgentes));
        }
        else if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            float radioCirculo = espacioEntreAgentes + 1.5f;
            formationManager.SetPattern(new CircleFormation(radioCirculo, limiteAgentes));
        }
    }
}