using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// For designers to set the template information for a type of an action. 
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Action", order = 1)]
[System.Serializable]
public class ActionScriptableObject : ScriptableObject
{
    [Tooltip("Name of the action")]
    [SerializeField] private string name_;

    [Tooltip("The duration of the action for the processing state")]
    [SerializeField] private float duration_;

    [Tooltip("The behaviors will be executed for the beginning state")]
    [SerializeField] private List<Behavior> begin_behaviors_sequence_;

    [Tooltip("The behaviors will be executed for the processing state")]
    [SerializeField] private List<Behavior> process_behaviors_sequence_;

    [Tooltip("The decision factors that will influence the action's score")]
    [SerializeField] private List<DecisionFactor> decision_factors_array_;

    // Getters & Setters
    public string Name { get => name_; }
    public float Duration { get => duration_; }
    public List<DecisionFactor> DecisionFactors { get => decision_factors_array_; }
    public List<Behavior> BeginBehaviorsSequence { get => begin_behaviors_sequence_; }
    public List<Behavior> ProcessBehaviorsSequence { get => process_behaviors_sequence_; }
}
