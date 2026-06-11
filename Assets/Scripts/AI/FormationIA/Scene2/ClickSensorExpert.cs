using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClickSensorExpert : Expert
{
    //éstos son los datos que el experto necesita para funcionar, se pasan a través del constructor
    private Blackboard blackboard;
    private List<GameObject> team;
    private float threshold;
    private LayerMask groundLayer;
    public ClickSensorExpert(Blackboard blackboard, List<GameObject> team, float threshold, LayerMask groundLayer) //hacemos un constructor del experto que crearemos a cada agente de la formación
    {
        this.blackboard = blackboard;
        this.team = team;
        this.threshold = threshold;
        this.groundLayer = groundLayer;
        this.blackboard.AddData("ThereIsCommonObjective", false);
        this.blackboard.AddData("CommonObjective", Vector3.zero);
    }
    //le daremos una insistencia total cuando se pulse el click izquierdo, luego ya comprobamos si el click se hace cerca y si necesitamos
    //escribir en la blackboard o no
    public override float GetInsistence(Blackboard blackboard)
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            return 1.0f;
        }
        return 0f;
    }
    public override Action[] Run(Blackboard blackboard)
    {
        return new Action[] { ClickDetector };
    }
    private void ClickDetector() //como se ha hecho click ejecutamos éste método para comprobar que sea un click válido
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());//Lanzamos un rayo desde la cámara hacia el punto de clik

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))//vemos si choca contra el suelo
        {
            Vector3 clickPoint = hit.point;//obtenemos la posición donde se ha clicado
            bool nearbyAgent = false;//seteamos ésto a false para comprobar y que no haya problemas de anteriores comprobaciones

            foreach (GameObject agent in team)//comprobamos por cada agente en la formación si la distancia entre el agente y el punto de click entra dentro del threshold
            {
                if (agent != null)
                {
                    float distance = Vector3.Distance(agent.transform.position, clickPoint);
                    if (distance <= threshold)
                    {
                        nearbyAgent = true; //si se cumple que el click está cerca del agente le decimos que hay un agente cerca
                        break;
                    }
                }
            }

            if (nearbyAgent)//y ahora tras comprobar si hay un agente cerca le decimos que si que lo hay y le pasamos la posición del click, todo a la blackboard
            {
                //no le pasaríamos nada a la blackboard si no hay un agente lo suficientemente cerca
                blackboard.AddData("CommonObjective", clickPoint);
                blackboard.AddData("ThereIsCommonObjective", true);
            }
        }
    }
}