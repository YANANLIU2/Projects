using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// An action is created from an action scriptable object which stores the template information for the type of action 
[System.Serializable]
public class Action 
{
    // The states of an action: awake -> start -> processing -> end
    public enum EActionState
    {
        kAwake,
        kStart,
        kProcessing,
        kEnd,
    }

    private Agent agent_ = null;
    private string name_;
    private Timer timer_ = new Timer();
    private EActionState action_state_ = EActionState.kAwake;
    private List<Behavior> begin_behaviors_sequence_ = new List<Behavior>();
    private List<Behavior> process_behaviors_sequence_ = new List<Behavior>();
    private Target target_;

    // Getters & Setters
    public Timer Timer { get => timer_; }
    public EActionState ActionState { get => action_state_; }
    public Agent Agent { get => agent_; set => agent_ = value; }
    public string Name { get => name_; }
    public string Target { get =>  target_ ? target_.name : "Anywhere"; }

    // constructor
    public Action(ActionScriptableObject scriptableObject, Agent agent, Target target)
    {
        timer_.Duration = scriptableObject.Duration;
        Initialize(scriptableObject.BeginBehaviorsSequence, begin_behaviors_sequence_);
        Initialize(scriptableObject.ProcessBehaviorsSequence, process_behaviors_sequence_);
        name_ = scriptableObject.Name;
        agent_ = agent;
        target_ = target;
    }

    // Update the timer, the state, and return true if the action is finished
    public bool Update()
    {
        if (agent_ == null) return true;

        switch (action_state_)
        {
            case EActionState.kAwake:
                {
                    if (target_)
                        agent_.NavMeshAgent.destination = target_.transform.position;

                    action_state_ = EActionState.kStart;
                    break;
                }
            case EActionState.kStart:
                if (agent_.IsArrived())
                {
                    action_state_ = EActionState.kProcessing;
                    if (target_)
                        target_.Join(agent_);
                    for (int i = 0; i < begin_behaviors_sequence_.Count; i++)
                        begin_behaviors_sequence_[i].Execute(agent_);
                }
                break;

            case EActionState.kProcessing:
                {
                    if (timer_.Update())
                    { 
                        action_state_ = EActionState.kEnd; 
                    }
                    else
                    {
                        for (int i = 0; i < process_behaviors_sequence_.Count; i++)
                            process_behaviors_sequence_[i].Execute(agent_);
                    }
                    break;
                }
            case EActionState.kEnd:
                if (target_)
                    target_.Leave(agent_);
                return true;

            default:
                break;
        }
        return false;
    }
    
    // Initialize
    public void Initialize(List<Behavior> target_sequence, List<Behavior> dest_sequence)
    {
        // Copy target to dest && initialize the real behavior types
        foreach (var targrt_behavior in target_sequence)
            dest_sequence.Add(Behavior.Create(targrt_behavior));
    }
}
