using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// The behavior will restor the agent's slepp value 
public class RestoreSleepPerSecond : Behavior
{
    private float true_value_;

    // Constructor
    public RestoreSleepPerSecond(string raw_value) : base(raw_value) { true_value_ = float.Parse(raw_value_); }

    public override void Execute(Agent agent) { agent.Energy.Value += true_value_ * Time.deltaTime; }
}
