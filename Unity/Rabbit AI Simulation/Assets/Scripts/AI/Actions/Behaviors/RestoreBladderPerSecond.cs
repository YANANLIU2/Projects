using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestoreBladderPerSecond : Behavior
{
    private float true_value_;

    // Constructor
    public RestoreBladderPerSecond(string raw_value) : base(raw_value) { true_value_ = float.Parse(raw_value_); }

    public override void Execute(Agent agent) { agent.Bladder.Value += true_value_ * Time.deltaTime; }
}
