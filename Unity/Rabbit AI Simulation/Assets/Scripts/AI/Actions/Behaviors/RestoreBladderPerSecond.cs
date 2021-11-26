using UnityEngine;

// The behavior will restor the agent's bladder value 
public class RestoreBladderPerSecond : Behavior
{
    private float true_value_;

    // Constructor
    public RestoreBladderPerSecond(string raw_value) : base(raw_value) { true_value_ = float.Parse(raw_value_); }

    public override void Execute(Agent agent) { agent.Bladder.Value += true_value_ * Time.deltaTime; }
}
