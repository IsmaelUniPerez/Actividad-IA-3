using System;

public abstract class Expert
{
    public abstract float GetInsistence(Blackboard blackboard);//aquí le daremos la insistencia a cada experto
    public abstract Action[] Run(Blackboard blackboard);//y aquí la acción que ejecutará si es el seleccionado por el árbitro de la blackboard
}
