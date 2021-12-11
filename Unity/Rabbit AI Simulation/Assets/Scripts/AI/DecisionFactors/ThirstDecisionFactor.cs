using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ThirstDecisionFactor", order = 1)]
[System.Serializable]
public class ThirstDecisionFactor : DecisionFactor
{
    [SerializeField] private float thirst_score_logistic_range = 6f;
    [SerializeField] private float thirst_score_steepness = 2.9901100113f; // euler * 1.1

    public override float Score(Target target, Context context)
    {
        // a variant of logistic function
        // - as the actor's thirst decrease, the desire to drink increase
        float x = context.Agent.Thirst.Value;
        float exponent = - x * thirst_score_logistic_range * 2 + thirst_score_logistic_range;
        float denominator = 1 + Mathf.Pow(thirst_score_steepness, exponent);
        float score = 1 - (1 / denominator);
        return score;
    }
}
