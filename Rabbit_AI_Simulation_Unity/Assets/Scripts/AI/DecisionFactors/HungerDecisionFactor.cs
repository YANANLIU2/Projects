using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/HungerDecisionFactor", order = 1)]
[System.Serializable]
public class HungerDecisionFactor : DecisionFactor
{
    [SerializeField] private float hunger_base_score = 0.2f;

    public override float Score(Target target, Context context)
    {
        // a linear curve 
        // - because a rabbit is likely to be seen as eating all day, I want it start to eat when it feels a bit hungry
        // - the younger the rabbit is, the eagerer the agent is to eat
        Agent agent = context.Agent;
        float x = agent.Hunger.Value;
        float y = agent.Age.Value;
        float ret = (1 - x) * Mathf.Min(1 - y + hunger_base_score, 1);
        return ret;
    }
}
