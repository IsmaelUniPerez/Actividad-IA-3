using UnityEngine;
using System.Collections.Generic;

public class LineFormation : IFormationPattern
{
    private float separacionX;
    private int maxSlots;

    public LineFormation(float separacionX = 2.5f, int maxSlots = 10)
    {
        this.separacionX = separacionX;
        this.maxSlots = maxSlots;
    }

    public bool SupportsSlots(int slotCount)
    {
        return slotCount <= maxSlots;
    }

    public Vector3 GetSlotTransform(int index, int totalSlots)
    {
        float x = index * separacionX;
        return new Vector3(x, 0, 0);
    }

    public Vector3 GetAnchorPoint()
    {
        return Vector3.zero;
    }

    public Vector3 GetDriftOffset(List<SlotAssignment> slots)
    {
        if (slots.Count == 0) return Vector3.zero;

        Vector3 formationCenter = Vector3.zero;
        foreach (var slot in slots)
        {
            formationCenter += GetSlotTransform(slot.index, slots.Count);
        }
        return formationCenter / slots.Count;
    }
}