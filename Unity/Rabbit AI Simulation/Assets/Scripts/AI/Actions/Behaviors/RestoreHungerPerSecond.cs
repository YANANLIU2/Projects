using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestoreHungerPerSecond : Behavior
{
    private float true_value_;

    // Constructor
    public RestoreHungerPerSecond(string raw_value) : base(raw_value) { true_value_ = float.Parse(raw_value_); }

    public override void Execute(Agent agent) { agent.Hunger.Value += true_value_ * Time.deltaTime; }
}
