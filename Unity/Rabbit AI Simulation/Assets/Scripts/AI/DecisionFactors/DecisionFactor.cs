using UnityEngine;

// returns a normalized value from 0 - 1
public abstract class DecisionFactor : ScriptableObject
{
    public abstract float Score(Target target, Context context);
}
