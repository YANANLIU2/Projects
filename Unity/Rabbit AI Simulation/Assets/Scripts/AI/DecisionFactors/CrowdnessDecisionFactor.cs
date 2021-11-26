using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CrowdnessDecisionFactor", order = 1)]
public class CrowdnessDecisionFactor : DecisionFactor
{
    [SerializeField] private int max_capacity = 5;
    [SerializeField] private float crowdness_steepness_score = -0.2f;

    public override float Score(Target target, Context context)
    {
        // before max capacity, score is 1
        // after that, since the agents realize that they are going to wait, the score will drop as the waiting line goes up
        // U = min(-0.2x + 2,1)
        int current_agents = target.Count;
        float ret = Mathf.Min(crowdness_steepness_score * current_agents + 2, 1);
        return ret;
    }
}
