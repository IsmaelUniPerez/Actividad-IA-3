using UnityEngine;
using System.Collections.Generic;

public class LineFormation : IFormationPattern
{
    private float separacionX;
    private int maxSlots;
    public LineFormation(float separacionX = 2.5f, int maxSlots = 10)
    {
        //Este es el constructor de la formación, aquí pondré todos los parámetros necesarios
        //para realizar ésta formación en concreto
        this.separacionX = separacionX;
        this.maxSlots = maxSlots;
    }

    public bool SupportsSlots(int slotCount)
    {
        //Limitamos el tamaño máximo de la línea si fuera necesario
        return slotCount <= maxSlots;
    }

    public Vector3 GetSlotTransform(int index)
    {
        //Se colocan uno al lado del otro a lo largo del eje X
        //El agente 0 en X=0, el 1 en X=2.5, el 2 en X=5.0, etc.
        float x = index * separacionX;

        return new Vector3(x, 0, 0);
    }

    public Vector3 GetAnchorPoint()
    {
        //El punto de anclaje base de la formación
        return Vector3.zero;
    }

    public Vector3 GetDriftOffset(List<SlotAssignment> slots)
    {
        if (slots.Count == 0) return Vector3.zero;

        Vector3 formationCenter = Vector3.zero;

        //Sumamos todas las posiciones ideales
        foreach (var slot in slots)
        {
            formationCenter += GetSlotTransform(slot.index);
        }

        //Dividimos para encontrar el punto de equilibrio que mantendrá la línea centrada
        return formationCenter / slots.Count;
    }
}