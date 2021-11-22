using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : Behavior
{
    public PlayAnimation(string raw_value) : base(raw_value) { }

    public override void Execute(Agent agent) { agent.PlayAnim(raw_value_); }
}
