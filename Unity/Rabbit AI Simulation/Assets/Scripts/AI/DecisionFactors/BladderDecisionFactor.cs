using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BladderDecisionFactor", order = 1)]
[System.Serializable]
public class BladderDecisionFactor : DecisionFactor
{
    // I uses the Unity animation curve to define the bladder decision factor
    // Compared to defining it with a function and parameters, this way is more direct for designers
    [SerializeField] AnimationCurve bladder_curve_ = new AnimationCurve();

    public override float Score(Target target, Context context)
    {
        float val = context.Agent.Bladder.Value;
        float ret = bladder_curve_.Evaluate(1 - val);
        return ret;
    }
}
