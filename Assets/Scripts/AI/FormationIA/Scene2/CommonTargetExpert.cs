using System;
using UnityEngine;

public class CommonTargetExpert : Expert
{
    private FormationManager manager;
    private Blackboard actualBlackboard;
    public CommonTargetExpert(FormationManager manager)
    {
        this.manager = manager;
    }

    public override float GetInsistence(Blackboard blackboard)
    {
        if (blackboard.GetDataByKey<bool>("ThereIsCommonObjective"))//Si en ThereIsCommonObjective es true le damos una insistencia alta para que ejecute
        {
            return 0.9f;
        }
        return 0f;
    }
    public override Action[] Run(Blackboard blackboard)
    {
        actualBlackboard = blackboard;
        return new Action[] { EjecutarRupturaFormacion };//le decimos que método ejecutar
    }
    private void EjecutarRupturaFormacion()
    {
        Vector3 target = actualBlackboard.GetDataByKey<Vector3>("CommonObjective");//obtenemos la posición del objetivo común guardado en la blackboard

        manager.SetPattern(null);//eliminamos la formación
        manager.MoveToCommonPoint(target);//les pasamos el objetivo común al administrador de la formación ejecutando el método para que se dirijan a ese punto
        actualBlackboard.AddData("ThereIsCommonObjective", false);//una vez hemos ejecutado la orden le decimos a la blackboard que ya no hay un objetivo común para que no vuelva a ejecutar esta orden
    }
}