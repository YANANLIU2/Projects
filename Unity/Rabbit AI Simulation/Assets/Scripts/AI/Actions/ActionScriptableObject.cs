using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Action", order = 1)]
[System.Serializable]
public class ActionScriptableObject : ScriptableObject
{
    [SerializeField] private string name_;
    [SerializeField] private float duration_;
    [SerializeField] private List<Behavior> begin_behaviors_sequence_;
    [SerializeField] private List<Behavior> process_behaviors_sequence_;
    [SerializeField] private List<DecisionFactor> decision_factors_array_;

    // Getters & Setters
    public List<DecisionFactor> DecisionFactors { get => decision_factors_array_; }
    public float Duration { get => duration_; }
    public string Name { get => name_; }
    public List<Behavior> BeginBehaviorsSequence { get => begin_behaviors_sequence_; }
    public List<Behavior> ProcessBehaviorsSequence { get => process_behaviors_sequence_; }
}
