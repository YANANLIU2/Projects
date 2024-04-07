using System.Collections.Generic;
using UnityEngine;

// Graph: A data structure to store/add/remove/splice nodes
// ID Management: a 32 bits uint. 16 bits (id's reference count) + 16 bits (the node's index)
[System.Serializable]
public class Graph
{
    // Nth adjacency is the incoming neighbors of the Nth node 
    [Header("Adjacency")]
    [Tooltip("The neighborhood connections of the incoming relationships A -> B")]
    [SerializeField] private List<AdjacencySet> m_incomingAdjacencyList = null;

    // Nth adjacency is the outgoing neighbors of the Nth node 
    [Tooltip("The neighborhood connections of the incoming relationships B -> A")]
    [SerializeField] private List<AdjacencySet> m_outgoingAdjacencyList = null;

    // The nodes list of the graph
    [Header("Nodes")]
    [Tooltip("The nodes in the graph")]
    [SerializeField] private List<Node> m_nodes = null;

    // Travese a graph from its entrance
    [Header("Entrance/Exit")]
    [Tooltip("Where the graph starts")]
    [SerializeField] private uint m_entrance = 0;

    // The exit is only used in slicing a graph 
    [Tooltip("Where the graph ends")]
    [SerializeField] private uint m_exit = s_invalidId;

    // Globals
    static public uint s_invalidId = uint.MaxValue;

    // A stack of free ids from invalid nodes in the node list
    private Stack<uint> m_freeIds = null;

    // A class for a list adjacency of a node 
    [System.Serializable]
    public class AdjacencySet 
    {
        // A list of neighbor ids
        [SerializeField] private List<uint> m_list = null;
        // Constructor
        public AdjacencySet() { m_list = new List<uint>(); }
        // Copy Constructor
        public AdjacencySet(AdjacencySet copy) { m_list = new List<uint>(copy.m_list); }
        // Getters & Setters
        public int Count { get => m_list.Count; }
        public uint this[int index] { get => m_list[index]; }
        public void Add(uint node) { m_list.Add(node); }
        public void Remove(uint node) { m_list.Remove(node); }
        public bool Contains(uint node) { return m_list.Contains(node); }
    }

    // Getters & Setters 
    public int Count { get => m_nodes.Count; }
    public uint Exit { get => m_exit; set => m_exit = value; }
    public uint Entrance { get => m_entrance; set => m_entrance = value; }
    public Node this[int index] { get => m_nodes[index]; set => m_nodes[index] = value;}
    public List<AdjacencySet> IncomingAdjacencyList { get => m_incomingAdjacencyList; }
    public List<AdjacencySet> OutgoingAdjacencyList { get => m_outgoingAdjacencyList; }
    // Constructor 
    public Graph()
    {
        // Initialize adjacency lists
        m_incomingAdjacencyList = new List<AdjacencySet>();
        m_outgoingAdjacencyList = new List<AdjacencySet>();
        // Initialize level nodes list
        m_nodes = new List<Node>();
        // Initialize free ids list 
        m_freeIds = new Stack<uint>();
    }

    #region Graph Management
    // Add a node at the end of the nodes list. It's the caller's responsibility to make sure that the node's id matches with the index. Node can be null.
    public void AddNode(Node node)
    {
        m_nodes.Add(node);
        m_incomingAdjacencyList.Add(new AdjacencySet());
        m_outgoingAdjacencyList.Add(new AdjacencySet());
    }

    // Add a new node of the target type into the graph: will try to use a free id before creating a brand new id
    public uint AddNode(Node.ENodeType type)
    {
        // Reusing an old id when there's free ids in the free id stack
        uint id = GetFreeId();
        if(id!= s_invalidId)
        { 
            int index = GetIndex(id);
            m_nodes[index] = new Node(id, type);
        }
        // Brand new id
        else
        {
            id = (uint)m_nodes.Count;
            m_incomingAdjacencyList.Add(new AdjacencySet());
            m_outgoingAdjacencyList.Add(new AdjacencySet());
            m_nodes.Add(new Node(id, type));
        }
        return id;
    }

    // Destory a node
    public void DestroyNode(uint id)
    {
        int index = GetIndex(id);
        var incomingNeighbors = m_incomingAdjacencyList[index];
        var outgoingNeighbors = m_outgoingAdjacencyList[index];
        // Unlink incoming neighbors: use reverse order because we are removing while iterating 
        for (int i = incomingNeighbors.Count - 1; i >= 0; i--)
        {
            UnlinkNodes(incomingNeighbors[i], id);
        }
        // Unlink outgoing neighbors
        for (int i = outgoingNeighbors.Count - 1; i >= 0; i--)
        {
            UnlinkNodes(id, outgoingNeighbors[i]);
        }
        // Clear the slot
        m_nodes[index] = null;
        // Add the id to the free id stack
        m_freeIds.Push(id);
    }

    // Link From and To 
    public void LinkNodes(uint from, uint to)
    {
        if (!IsLinked(from, to))
        {
            m_outgoingAdjacencyList[GetIndex(from)].Add(to);
            m_incomingAdjacencyList[GetIndex(to)].Add(from);
        }
    }

    // Unlink From and To 
    public void UnlinkNodes(uint from, uint to)
    {
        m_outgoingAdjacencyList[GetIndex(from)].Remove(to);
        m_incomingAdjacencyList[GetIndex(to)].Remove(from);
    }

    // Reset the level graph 
    public void Reset()
    {
        // Clear the adjacency lists
        m_incomingAdjacencyList.Clear();
        m_outgoingAdjacencyList.Clear();
        // Clear the free id stack
        m_freeIds.Clear();
        // Clear the node list
        m_nodes.Clear();
        // Set the entrance and the exit to invalid
        m_entrance = 0;
        m_exit = 0;
    }
    #endregion

    #region ID Management
    // Get a free id from the queue
    private uint GetFreeId()
    {
        if (m_freeIds.Count == 0) { return s_invalidId; }
        uint id = m_freeIds.Pop();
        IncrementRefCount(ref id);
        return id;
    }

    // Give a free Id but increment ref count (the first 16 bits) so we know it's new 
    private void IncrementRefCount(ref uint id)
    {
        // Get the current reference count
        uint refCount = GetRefCount(id);
        // Increment
        refCount++;
        // Clear out the old ref count
        id &= 0x0000ffff;
        // Record the new ref count
        id |= (refCount << 16);
    }

    // The second 16 bits
    public int GetIndex(uint id)
    {
        uint index = id & 0x0000ffff;
        return (int)index;
    }

    // Get the reference count
    private uint GetRefCount(uint id)
    {
        return id >> 16;
    }
    #endregion

    #region Splice
    // Replace a subgraph with another subgraph
    public void Splice(uint[] destNodes, TransformationRule rule)
    {
        int destSize = destNodes.Length;
        // Copy incoming edges of the entrance node and the outgoing edges of the exit node
        AdjacencySet destEntranceEdges = new AdjacencySet(m_incomingAdjacencyList[GetIndex(destNodes[0])]);
        AdjacencySet destExitEdges = new AdjacencySet(m_outgoingAdjacencyList[GetIndex(destNodes[destSize - 1])]);
        // Copy the rest of dest graph's edges 
        AdjacencySet[] destIncomingEdgesList = new AdjacencySet[destSize];
        AdjacencySet[] destOutgoingEdgesList = new AdjacencySet[destSize];
        for (int i = 0; i < destSize; i++)
        {
            int nodeIndex = GetIndex(destNodes[i]);
            if(i != 0)
            {
                destIncomingEdgesList[i] = new AdjacencySet(m_incomingAdjacencyList[nodeIndex]);
            }
            if(i != destSize - 1)
            {
                destOutgoingEdgesList[i] = new AdjacencySet(m_outgoingAdjacencyList[nodeIndex]);
            }
        }
        // Destroy the dest graph
        foreach (var node in destNodes)
        {
            DestroyNode(node);
        }
        // Add the new subgraph as an island, and create a map for new ids and old ids
        Dictionary<uint, uint> islandMap = new Dictionary<uint, uint>();
        AddIsland(islandMap, rule.OutputGraph);
        // Paste the external edges to the new entrance node and the new exit node
        uint newEntrance = islandMap[rule.OutputGraph.m_entrance];
        uint newExit = islandMap[rule.OutputGraph.Exit];
        CopyExternalEdges(newEntrance, destEntranceEdges, true, destNodes);
        CopyExternalEdges(newExit, destExitEdges, false, destNodes);
        // Paste the external edges to the new graph based on the map
        for (int i = 0; i < destSize; i++)
        {
            uint newId = islandMap[rule.InputGraph.NodePattern[i].ReplacementNodeId];
            if(i != 0)
            {
                CopyExternalEdges(newId, destIncomingEdgesList[i], true, destNodes);
            }
            if(i!= destSize - 1)
            {
                CopyExternalEdges(newId, destOutgoingEdgesList[i], false, destNodes);
            }
        }
    }

    // Copy the edges but ignore the internal edges
    private void CopyExternalEdges(uint id, AdjacencySet neighbors, bool isIncoming, uint[] internalNodes)
    {
        for (int j = 0; j < neighbors.Count; j++)
        {
            uint neighborId = neighbors[j];
            // Ignore internal adjacency
            bool isInternal = false;
            for (int i = 0; i < internalNodes.Length; i++)
            {
                if (internalNodes[i] == neighborId)
                {
                    isInternal = true;
                    break;
                }
            }
            if (isInternal) { continue; }
            // Connect From and To
            if (isIncoming)
            {
                LinkNodes(neighborId, id);
            }
            else
            {
                LinkNodes(id, neighborId);
            }
        }
    }

    // Add a subgraph base on the rule graph as an island which means it is only connected internally
    private void AddIsland(Dictionary<uint, uint> islandMap, Graph ruleGraph)
    {
        // Copy the nodes and map them
        foreach (var node in ruleGraph.m_nodes)
        {
            uint newId = AddNode(node.Type);
            islandMap.Add(node.Id, newId);
        }

        // Copy the relationships
        for (int i = 0; i < ruleGraph.m_nodes.Count; i++)
        {
            // Get the mapping ids 
            uint oldId = ruleGraph.m_nodes[i].Id;
            uint newId = islandMap[oldId];
            // Link based on outgoing lists 
            var outgoingAdjacency = ruleGraph.OutgoingAdjacencyList[i];
            for (int j = 0; j < outgoingAdjacency.Count; j++)
            {
                LinkNodes(newId, islandMap[outgoingAdjacency[j]]);
            }
        }
    }
    #endregion

    #region HELPERS
    // Return the node with the id
    public Node GetNodeById(uint id)
    {
        Node result = m_nodes[GetIndex(id)];
        return id == result.Id ? result : null;
    }

    // Return the id of the node at the index
    public uint GetIdByIndex(int index)
    {
        return m_nodes[index].Id;
    }

    // Can we access a valid node by the index?
    public bool IsIndexValid(int index)
    {
        if(index < 0 || index >= m_nodes.Count )
        {
            return false;
        }
        if(m_nodes[index] == null)
        {
            return false;
        }
        return true;
    }

    // Are both reference count and index valid?
    public bool IsIdValid(uint id)
    {
        int index = GetIndex(id);
        if(!IsIndexValid(index))
        {
            return false;
        }
        return (GetRefCount(id) == GetRefCount(m_nodes[index].Id));
    }

    // Is From linked with To?
    public bool IsLinked(uint from, uint to)
    {
        int fromIndex = GetIndex(from);
        return m_outgoingAdjacencyList[fromIndex].Contains(to);
    }

    // Can we traverse the graph from the entrance
    public bool IsValid()
    {
        int entranceIndex = GetIndex(m_entrance);
        if(entranceIndex >= 0 && entranceIndex < m_nodes.Count)
        {
            return m_nodes[entranceIndex] != null;
        }
        return false;
    }
    #endregion

    #region Print
    // Get either the incoming or the outgoing adjacencies print info
    private void GetAdjacencyPrintInfo(ref string msg, List<AdjacencySet> adjacencies, string start, string explaination)
    {
        const string kCommaStr = ", ";
        const string kLineStr = "\n";
        msg += start;
        for (int i = 0; i < adjacencies.Count; i++)
        {
            // Skip empty slots
            if (m_nodes[i] == null)
            {
                continue;
            }
            // Get the node's id and type
            msg += m_nodes[i].GetPrintInfo() + explaination;
            // Get the outgoing adjacency of the node
            var neighbors = adjacencies[i];
            for (int j = 0; j < neighbors.Count; j++)
            {
                msg = msg + m_nodes[GetIndex(neighbors[j])].GetPrintInfo() + kCommaStr;
            }
            msg += kLineStr;
        }
    }

    // Get a string containing all nodes's id, type and their adjacencies
    public string GetGraphPrintInfo()
    {
        const string kOutgoingStr = "Outgoing: \n";
        const string kIncomingStr = "\nIncoming: \n";
        const string kCanAccessStr = " can access: ";
        const string kCanBeAccessedStr = " can be accessed from: ";
        string msg = string.Empty;
        GetAdjacencyPrintInfo(ref msg, m_outgoingAdjacencyList, kOutgoingStr, kCanAccessStr);
        GetAdjacencyPrintInfo(ref msg, m_incomingAdjacencyList, kIncomingStr, kCanBeAccessedStr);
        return msg;
    }
    #endregion
}
