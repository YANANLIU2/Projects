// The behavior will play an animation on the agent
public class PlayAnimation : Behavior
{
    public PlayAnimation(string raw_value) : base(raw_value) { }

    public override void Execute(Agent agent) { agent.PlayAnim(raw_value_); }
}
