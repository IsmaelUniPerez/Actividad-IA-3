using UnityEngine;
using System.Collections.Generic;

public class CircleFormation : IFormationPattern
{
    private float radio;
    private int maxSlots;

    public CircleFormation(float radio = 4.0f, int maxSlots = 8)
    {
        this.radio = radio;
        this.maxSlots = maxSlots;
    }

    public bool SupportsSlots(int slotCount)
    {
        return slotCount <= maxSlots;
    }

    public Vector3 GetSlotTransform(int index, int totalSlots)
    {
        // Protección de seguridad por si no hay agentes
        if (totalSlots == 0) return Vector3.zero;

        // Dividimos 360 grados entre el número ACTUAL de agentes, no entre el máximo
        float anguloGrados = index * (360f / totalSlots);
        float anguloRadianes = anguloGrados * Mathf.Deg2Rad;

        float x = Mathf.Cos(anguloRadianes) * radio;
        float z = Mathf.Sin(anguloRadianes) * radio;

        return new Vector3(x, 0, z);
    }

    public Vector3 GetAnchorPoint()
    {
        return Vector3.zero;
    }

    public Vector3 GetDriftOffset(List<SlotAssignment> slots)
    {
        if (slots.Count == 0) return Vector3.zero;

        Vector3 centroMasas = Vector3.zero;
        foreach (var slot in slots)
        {
            // Pasamos slots.Count como segundo parámetro
            centroMasas += GetSlotTransform(slot.index, slots.Count);
        }

        return centroMasas / slots.Count;
    }
}