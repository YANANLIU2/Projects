using UnityEngine;

// Decision factors are used to evaluate an action's utility score (desire).
// All decision factors of an action will contribute to the final score.
// One decision factor will give a normalized value. [0,1]
public abstract class DecisionFactor : ScriptableObject
{
    public abstract float Score(Target target, Context context);
}
