using System;
using UnityEngine;

public class CommonTargetExpert : Expert
{
    private FormationManager manager;
    public CommonTargetExpert(FormationManager manager)
    {
        this.manager = manager;
    }

    public override float GetInsistence(Blackboard blackboard)
    {
        if (blackboard.GetDataByKey<bool>("HayObjetivoComun"))
        {
            return 0.9f;
        }
        return 0f;
    }

    public override Action[] Run(Blackboard blackboard)
    {
        Vector3 destino = blackboard.GetDataByKey<Vector3>("ObjetivoComun");

        return new Action[]
        {
            () =>
            {
                manager.SetPattern(null);
                manager.MoveToCommonPoint(destino);
                blackboard.AddData("HayObjetivoComun", false);

                Debug.Log("CommonTargetExpert: He leído la orden en la pizarra. ¡Formación rota! Tropas avanzando al objetivo común.");
            }
        };
    }
}