using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class FormationControllerScene1 : MonoBehaviour
{
    [Header("Arquitectura")] //Gestor de formación
    public FormationManager formationManager;

    [Header("Agentes Activos")] //Lista de agentes que empiezan dentro de la formación
    public List<GameObject> team;

    [Header("Agentes en Reserva (Cola)")] //Lista de agentes que empiezan fuera de la formación, esperando a ser añadidos
    public List<GameObject> reserveAgents;

    [Header("Ajustes Geométricos")] //Parámetros para configurar el patrón de formación
    public float espacioEntreAgentes = 8f;
    public int limiteAgentes = 10;

    void Start()
    {
        if (formationManager == null) return;

        formationManager.SetPattern(new LineFormation(espacioEntreAgentes, limiteAgentes));

        //Añadimos solo a los del equipo inicial
        foreach (GameObject agent in team)
        {
            formationManager.AddAgent(agent);
        }
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        //Usando el espacio podemos meter al siguiente agente de la cola en la formación
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (reserveAgents.Count > 0)
            {
                //Cogemos al primer agente de la cola
                GameObject recluta = reserveAgents[0];

                //Intentamos meterlo en el gestor. Si hay hueco en el patrón, AddAgent devolverá TRUE.
                if (formationManager.AddAgent(recluta))
                {
                    Debug.Log($"Agente {recluta.name} añadido a la formación exitosamente.");

                    //Como está en la formación lo quitamos de reserva
                    reserveAgents.RemoveAt(0);
                }
                else
                {
                    //Si AddAgent devuelve FALSE (ej. el límite es 10 y ya hay 10)
                    Debug.LogWarning("Orden rechazada: La formación actual ha alcanzado su límite máximo.");
                }
            }
            else
            {
                Debug.Log("La cola de reserva está vacía. No hay más agentes para añadir.");
            }
        }
        //dependiendo de la tecla cambiamos a un patrón de formación u otro
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
            float radioCirculo = espacioEntreAgentes + 1.5f;//le añadimos un extra para que no se amontonen tanto al ser una formación circular
            formationManager.SetPattern(new CircleFormation(radioCirculo, limiteAgentes));
        }
    }
}