using UnityEngine;
using System.Collections.Generic;

public class WedgeFormation : IFormationPattern
{
    private float separationX;
    private float separationZ;
    private int maxSlots;

    public WedgeFormation(float separacionX = 2.0f, float separacionZ = 2.0f, int maxSlots = 10)
    {
        this.separationX = separacionX;
        this.separationZ = separacionZ;
        this.maxSlots = maxSlots;
    }

    public bool SupportsSlots(int slotCount)
    {
        return slotCount <= maxSlots;
    }

    public Vector3 GetSlotTransform(int index)
    {
        if (index == 0) return Vector3.zero;
        int fila = (index + 1) / 2;
        float lado = (index % 2 == 0) ? 1.0f : -1.0f;
        float x = lado * fila * separationX;
        float z = -fila * separationZ;

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
            centroMasas += GetSlotTransform(slot.index);
        }

        return centroMasas / slots.Count;
    }
}
