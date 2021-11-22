using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EnergyDecisionFactor", order = 1)]
[System.Serializable]
public class EnergyDecisionFactor : DecisionFactor
{
    [SerializeField] private float energy_score_exponent;

    public override float Score(Target target, Context context)
    {
        // a quadratic curve
        // an agent's sleep desire will grow slow at the beginning to the mid but grow rapidly after a certain point
        // U = (x/m)^k

        float val = context.Agent.Energy.Value;
        
        float ret = Mathf.Pow(1 - val, energy_score_exponent);
        return ret;
    }
}
