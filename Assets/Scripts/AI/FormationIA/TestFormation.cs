using UnityEngine;
using System.Collections.Generic;

public class TestFormacion : MonoBehaviour
{
    public FormationManager gestor;
    public List<GameObject> agentesEnEscena;

    void Start()
    {
        // Le asignamos el patrón de línea al arrancar (separados por 3 unidades)
        //gestor.SetPattern(new WedgeFormation(2.5f, 2.5f));
        //gestor.SetPattern(new CircleFormation(4.0f, 8));
        //gestor.SetPattern(new LineFormation(3.0f, 10));
        // Metemos a todos los agentes de la lista en la formación
        foreach (GameObject agente in agentesEnEscena)
        {
            gestor.AddAgent(agente);
        }

    }
}