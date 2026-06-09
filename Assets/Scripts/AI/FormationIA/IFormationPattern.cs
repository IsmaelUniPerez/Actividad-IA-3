using UnityEngine;
using System.Collections.Generic;

public interface IFormationPattern
{
    bool SupportsSlots(int slotCount); // Devuelve true si esta formación puede soportar el número de agentes actual
    Vector3 GetSlotTransform(int index);// Devuelve la posición ideal del agente en el slot dado, relativa al punto de anclaje
    Vector3 GetAnchorPoint();// Devuelve el punto de anclaje de la formación, que es el punto de referencia para calcular las posiciones de los agentes
    Vector3 GetDriftOffset(List<SlotAssignment> slots);// Devuelve un offset que se aplicará a todos los agentes para mantener la formación centrada
}