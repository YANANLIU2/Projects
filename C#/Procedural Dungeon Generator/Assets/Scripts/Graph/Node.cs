using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A node in a graph
[System.Serializable]
public class Node
{
    // Node types: be careful of changing the enum types because editor only save integar data 
    public enum ENodeType
    {
        kEntrance,
        kGoal,
        kBoss,
        kItem,
        kPuzzle,
        kMonster,
        kTreasure,
        kMiniBoss,
        kKey,
        kLock,
        kTask,
        kRest,
        kStart,
        kNum,
    }

    [Tooltip("Identification Number")]
    [SerializeField] private uint m_id;

    [Tooltip("Node Type")]
    [SerializeField] private ENodeType m_type;

    // Getters and Setters
    public uint Id { get => m_id; set => m_id = value; }
    public ENodeType Type { get => m_type; set => m_type = value; }

    // Constructor
    public Node(uint id, ENodeType type) 
    { 
        m_id = id; 
        m_type = type; 
    }

    // Get a string that contains the node's information 
    public string GetPrintInfo()
    {
        return m_type.ToString() + ' ' + m_id.ToString();
    }
}