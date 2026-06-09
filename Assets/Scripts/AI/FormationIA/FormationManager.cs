using UnityEngine;
using System.Collections.Generic;
using IaActividad1.Steering.Dynamic;

public class FormationManager : MonoBehaviour
{
    private List<SlotAssignment> slots = new List<SlotAssignment>();//la lista de slots que contiene a los agentes y su índice en la formación
    private IFormationPattern pattern;//el patrón que se asigna, es una interfaz que nos permite a través de un constructor crear y asignar distintos patrones
    private Vector3 formationAdjustement;//el vector que usamos para centrar la formación

    //Tanto añadir comno eliminar agentes son booleanos, ésto lo hacemos para que podamos por saber por ejemplo en la blackboard si se ha añadido o eliminado un agente correctament
    //o si no se ha podido, podría funcionar con voids a secas pero lo dejaré así para tener más info sobre lo que ocurre en la formación, y así que los expertos de la blackboard
    //puedan saber el resultado de éstos métodos
    public bool AddAgent(GameObject agent)//éste método añadirá agentes del tipo game obect a la firmación, es bool porque devuelve true si el agente se ha añadido correctamente, y false si no se ha podido añadir
    {
        if (pattern != null && pattern.SupportsSlots(slots.Count + 1))//antes se comprueba que haya espacio en el patrón para un nuevo agente, y que el patrón no sea nulo
        {
            SlotAssignment newSlot = new SlotAssignment(agent, slots.Count);//se crea el nuevo slot
            slots.Add(newSlot);//se añade el nuevo slot a la lista de slots
            UpdateSlotsAssignments();//se llama a UpdateSlotsAssignments para actualizar las asignaciones de los slots, y así mantener la formación ajustada al nuevo número de agentes
            return true;
        }
        return false;
    }

    public bool RemoveAgent(GameObject agente)//Iagual que en lo anterior, devuelve true si el agente se ha eliminado correctamente, y false si no se ha podido eliminar
    {
        for (int i = 0; i < slots.Count; i++)//se recorre la lista de slots
        {
            if (slots[i].agent == agente)//Se encuentra el slot que contiene al agente que queremos eliminar
            {
                slots.RemoveAt(i);//lo elmininamos
                UpdateSlotsAssignments();//y actualizamos las asignaciones de los slots
                return true;
            }
        }
        return false;
    }

    private void UpdateSlotsAssignments()//éste metodo que usamos para actualizar las asignaciones de los slots
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].index = i;
        }

        if (pattern != null)
        {
            formationAdjustement = pattern.GetDriftOffset(slots);
        }
    }

    public void SetPattern(IFormationPattern newPattern)//éste metodo lo usaremos para asingar un nuevo patrón a la formación y actualizar los slots
    {
        this.pattern = newPattern;
        UpdateSlotsAssignments();
    }
    public void MoveToCommonPoint(Vector3 finalDestiny)//éste método hace que dejen de seguir un patrón y se dirijan a un lugar en concreto, el finalDestiny
    {
        foreach (var slot in slots)
        {
            if (slot.agent != null)
            {
                var seek = slot.agent.GetComponent<SeekVirtual>();
                if (seek != null)
                {
                    seek.objetiveTarget = finalDestiny;
                }
            }
        }
    }

    void Update()//vamos a usar el update para actualizar la posición de cada agente en el patrón, y así mantener la formación si ésta cambia en posición, orientación o numero de agentes
    {
        //Si no tenemos un patrón asignado, no hacemos nada
        if (pattern == null) return;

        //por cada slot en la lista de slots
        foreach (var slot in slots)
        {
            if (slot.agent != null)
            {
                //se obtiene la transformationInPatron con pattern.GetSlotTransform(índice de slot)
                Vector3 transformationInPatron = pattern.GetSlotTransform(slot.index);

                //la localización es la suma de la transformationInPatron y el pattern.GetAnchorPoint()
                Vector3 localitation = transformationInPatron + pattern.GetAnchorPoint();

                //se resta a la localización el formationAdjustement
                localitation -= formationAdjustement;

                //por último, se establece la localización como el objetivo del agente que se encuentra en slot
                SeekVirtual steeringSeek = slot.agent.GetComponent<SeekVirtual>();
                if (steeringSeek != null)
                {
                    steeringSeek.objetiveTarget = localitation;
                }
            }
        }
    }
}