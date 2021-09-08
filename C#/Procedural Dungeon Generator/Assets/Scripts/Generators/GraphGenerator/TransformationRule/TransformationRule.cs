using UnityEngine;

[System.Serializable]
public class TransformationRule
{
    [Header("Predecessor")]
    [Tooltip("The prerequisites")]
    [SerializeField] private InputGraph m_inputGraph = null;

    // # In editor only needs to set outgoing adjacencies for connections
    [Header("Successor")]
    [Tooltip("The replacement: you can ignore incoming adjacencies")]
    [SerializeField] private Graph m_outputGraph = null;

    // The weight for weighted randomness
    [Header("Weight")]
    [Tooltip("The probability of the rule being picked")]
    [SerializeField] private float m_weight = 0;

    // Getters & Setters
    public Graph OutputGraph { get => m_outputGraph; }
    public InputGraph InputGraph { get => m_inputGraph; }
    public float Weight { get => m_weight; }
}
