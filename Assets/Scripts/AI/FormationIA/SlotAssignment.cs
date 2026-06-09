using UnityEngine;

[System.Serializable]
public class SlotAssignment //esta clase es un contenedor que asocia a un agente con su índice en la formación
{
    public GameObject agent;
    public int index;

    public SlotAssignment(GameObject agent, int index)
    {
        this.agent = agent;
        this.index = index;
    }
}