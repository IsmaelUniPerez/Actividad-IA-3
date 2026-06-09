using UnityEngine;
using System.Collections.Generic;

public class WedgeFormation : IFormationPattern
{
    private float separacionX;
    private float separacionZ;
    private int maxSlots;

    public WedgeFormation(float separacionX = 2.0f, float separacionZ = 2.0f, int maxSlots = 10)
    {
        this.separacionX = separacionX;
        this.separacionZ = separacionZ;
        this.maxSlots = maxSlots;
    }

    public bool SupportsSlots(int slotCount)
    {
        return slotCount <= maxSlots;
    }

    public Vector3 GetSlotTransform(int index, int totalSlots)
    {
        if (index == 0) return Vector3.zero;

        int fila = (index + 1) / 2;
        float lado = (index % 2 == 0) ? 1.0f : -1.0f;

        float x = lado * fila * separacionX;
        float z = -fila * separacionZ;

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
            centroMasas += GetSlotTransform(slot.index, slots.Count);
        }
        return centroMasas / slots.Count;
    }
}