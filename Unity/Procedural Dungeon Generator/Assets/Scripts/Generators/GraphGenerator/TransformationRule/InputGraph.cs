using UnityEngine;

// For a rule: define the linear input pattern 
[System.Serializable]
public class InputGraph
{
    [System.Serializable]
    public class InputNode
    {
        // Node type
        public Node.ENodeType Type = Node.ENodeType.kNum;
        // Replacing the node
        public uint ReplacementNodeId = 0;
    }

    // # For a input graph of a rule: the node's relationships can only be linear 
    [Tooltip("Linear relationship only")]
    [SerializeField] public InputNode[] m_nodePattern = null;

    // Getter
    public InputNode[] NodePattern { get => m_nodePattern; }
}