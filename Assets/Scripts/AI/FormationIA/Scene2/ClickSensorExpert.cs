using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClickSensorExpert : Expert
{
    private Blackboard blackboard;
    private List<GameObject> escuadron;
    private float umbral;
    private LayerMask capaSuelo;
    public ClickSensorExpert(Blackboard blackboard, List<GameObject> escuadron, float umbral, LayerMask capaSuelo)
    {
        this.blackboard = blackboard;
        this.escuadron = escuadron;
        this.umbral = umbral;
        this.capaSuelo = capaSuelo;
        this.blackboard.AddData("HayObjetivoComun", false);
        this.blackboard.AddData("ObjetivoComun", Vector3.zero);
    }

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
        return new Action[]
        {
            () =>
            {
                Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, capaSuelo))
                {
                    Vector3 puntoClic = hit.point;
                    bool algunAgenteCerca = false;
                    foreach (GameObject agente in escuadron)
                    {
                        if (agente != null)
                        {
                            float distancia = Vector3.Distance(agente.transform.position, puntoClic);
                            if (distancia <= umbral)
                            {
                                algunAgenteCerca = true;
                                break;
                            }
                        }
                    }
                    if (algunAgenteCerca)
                    {
                        Debug.Log("ClickSensorExpert: Objetivo válido detectado. Escribiendo 'true' en la Pizarra...");
                        blackboard.AddData("ObjetivoComun", puntoClic);
                        blackboard.AddData("HayObjetivoComun", true);
                    }
                }
            }
        };
    }
}