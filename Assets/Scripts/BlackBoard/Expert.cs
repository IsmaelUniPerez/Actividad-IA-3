using System;

public abstract class Expert
{
    public abstract float GetInsistence(Blackboard blackboard);
    public abstract Action[] Run(Blackboard blackboard);
}
