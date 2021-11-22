using UnityEngine;

// value the distance desire
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DistanceDecisionFactor", order = 1)]
[System.Serializable]
public class DistanceDecisionFactor : DecisionFactor
{
    [SerializeField] private float distance_score_exponent = 2;
    [SerializeField] private float distance_base_score = 0.03f;
    [SerializeField] private float max_distance_value = 50;
    public override float Score(Target target, Context context)
    {
        // similar to a quadratic curve
        // - an agent will prefer nearby places and ignore minor distance differences
        // - places with medium distances are okay
        // - places are very far will get much lower utility score 
        float delta = (target.transform.position - context.Agent.transform.position).magnitude;
        float compare = Mathf.Min( 1, delta / max_distance_value);
        float score = Mathf.Min(1 - Mathf.Pow(compare, distance_score_exponent) + distance_base_score, 1);
        return score;
    }
} 
