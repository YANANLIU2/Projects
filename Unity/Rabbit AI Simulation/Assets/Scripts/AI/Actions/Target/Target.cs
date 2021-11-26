using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A target can carry one or multiple actions.
// When an agent is making a decision, firstly it will gather all targets and pair them with their possile actions.
// Then it will score every pair and pick the one with the highest score.
public class Target : MonoBehaviour
{
    [Tooltip("The actions the target carry")]
    [SerializeField] private List<ActionScriptableObject > actions_array;

    [Tooltip("The agents are currently using the target")]
    private List<Agent> current_agents_ = new List<Agent>();

    // Getters & Setters
    public List<ActionScriptableObject > ActionsArray { get => actions_array; }
    public int Count { get => current_agents_.Count; }

    // An agent joins the target
    public void Join(Agent agent)
    {
        if(!current_agents_.Contains(agent)) current_agents_.Add(agent);
    }
    
    // An agent leaves the target 
    public void Leave(Agent agent)
    {
        for (int i = 0; i < current_agents_.Count; i++)
            if (current_agents_[i] == agent) { current_agents_.RemoveAt(i); return; }
    }

}
