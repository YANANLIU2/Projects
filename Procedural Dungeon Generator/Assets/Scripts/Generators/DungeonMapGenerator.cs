using System.Collections.Generic;
using UnityEngine;

// The script is for generating a mini map of the dungeon
public class DungeonMapGenerator : MonoBehaviour
{
    [Header("RNG")]
    [Tooltip("A random number generator")]
    [SerializeField] private XorshiftRNG m_rng = new XorshiftRNG();

    [Header("Neighbors")]
    [Tooltip("The maximum number of neighbor tiles of one tile")]
    [SerializeField] private NeighborModeBase.ENeighborModeType m_neighborModeType;

    [Header("Teleport")]
    [Tooltip("Are we generating teleports? - Connections whose magnitude > n will be generated as teleports")]
    [SerializeField] private bool m_isUsingTeleport = true;

    [Tooltip("The minimum requirement of the teleport connections")]
    [SerializeField] private float m_minimumTeleportMagnitude = 3;

    [Tooltip("The position offset of the connections")]
    [SerializeField] private Vector3 m_connectionOffset = new Vector3(0.5f, 0.5f, 0);

    [Header("Render")]
    [Tooltip("Space between tiles")]
    [SerializeField] private int m_tileSpace = 2;

    [Tooltip("The position of the entrance node icon")]
    [SerializeField] private Vector3Int m_startPos = Vector3Int.zero;

    [Tooltip("The prefab that displays a connection of two nodes")]
    [SerializeField] private LineRenderer m_connectionPrefab = null;

    [Tooltip("The prefab that displays a teleport of two nodes")]
    [SerializeField] private LineRenderer m_teleportPrefab = null;

    private Graph m_graph;
    private GraphGenerator m_graphGenerator = null;
    private NeighborModeBase m_neighborMode;
    private Dictionary<int, Vector3Int> m_discovered = null;
    private List<NeighborModeBase> m_neighborModeList = null;

    // Getters & Setters
    public bool IsUsingTeleport { set => m_isUsingTeleport = value; }
    public float MinimumTeleportMagnitude { set => m_minimumTeleportMagnitude = value; }
    public Dictionary<int, Vector3Int> Discovered { get => m_discovered; }
    public int NeighborMode
    {
        set
        {
            m_neighborModeType = (NeighborModeBase.ENeighborModeType)value;
            m_neighborMode = m_neighborModeList[value];
        }
    }
    #region MainLogic
    private void Awake()
    {
        m_rng.Initialize();
        m_discovered = new Dictionary<int, Vector3Int>();
        // Graph generator
        m_graphGenerator = FindObjectOfType<GraphGenerator>();
        if (m_graphGenerator != null)
        {
            m_graph = m_graphGenerator.Graph;
        }
        // Neighbor mode
        m_neighborModeList = new List<NeighborModeBase>();
        for (int i = 0; i < (int)NeighborModeBase.ENeighborModeType.kNum; i++)
        {
            m_neighborModeList.Add(NeighborModeBase.CreateMode(i));
        }
        NeighborMode = (int)m_neighborModeType;
        DontDestroyOnLoad(gameObject);
    }

    // Generate a new dungeon based on the graph
    public void Generate()
    {
        Reset();
        // Assume the graph starts from index 0
        if (m_graph == null || m_graph.Count == 0 || m_graph[m_graph.GetIndex(m_graph.Entrance)] == null)
        {
#if DEBUG
            Debug.LogWarning("The initialization of the graph failed");
#endif
            return;
        }
        ProcessGraph();
        DrawGraph();
    }

    // Assign nodes with positions
    public void ProcessGraph()
    {
        ProcessANode(m_graph.GetIndex(m_graph.Entrance), m_startPos);
    }

    // Reset everything for a new round of generation
    private void Reset()
    {
        m_discovered.Clear();
        // Clear the drawing screen
        TilemapManager.Instance.Reset();
    }
    #endregion

    #region Render
    private void DrawGraph()
    {
        // Draw nondes
        foreach (var node in m_discovered)
        {
            DrawANode(node.Key, node.Value);
        }
        // Draw connections
        for (int i = 0; i < m_graph.OutgoingAdjacencyList.Count; i++)
        {
            Vector3Int myPos = m_discovered[i];
            Vector3 posA = myPos * m_tileSpace + m_connectionOffset;
            var outgoingAdjacency = m_graph.OutgoingAdjacencyList[i];
            for (int j = 0; j < outgoingAdjacency.Count; j++)
            {
                int neighborIndex = m_graph.GetIndex(outgoingAdjacency[j]);
                Vector3Int NeighborPos = m_discovered[neighborIndex];
                Vector3 posB = NeighborPos * m_tileSpace + m_connectionOffset;
                LineRenderer line = IsATeleport(myPos, NeighborPos) ? m_teleportPrefab : m_connectionPrefab;
                TilemapManager.Instance.DrawALine(posA, posB, line);
            }
        }
    }

    // Draw a tile's icon and name on screen
    private void DrawANode(int index, Vector3Int pos)
    {
        Vector3Int displayPos = pos * m_tileSpace;
        // Icon
        Node.ENodeType type = m_graph[index].Type;
        TilemapManager.Instance.SetTile(displayPos, type);
        // Name + Index(Id is too long to display)
        TilemapManager.Instance.SetText(displayPos, type.ToString() + index.ToString());
    }
    #endregion

    #region Traverse a graph
    private void ProcessANode(int index, Vector3Int pos)
    {
        // Process itself
        m_discovered.Add(index, pos);
        // Process neighbors
        var outgoingAdjacency = m_graph.OutgoingAdjacencyList[index];
        for (int i = 0; i < outgoingAdjacency.Count; i++)
        {
            int neighborIndex = m_graph.GetIndex(outgoingAdjacency[i]);
            if (!m_discovered.ContainsKey(neighborIndex))
            {
                Vector3Int neighborPos = GetANeighborPos(neighborIndex, pos);
                ProcessANode(neighborIndex, neighborPos);
            }
        }
    }

    // Get a valid neighbor position around the target position 
    Vector3Int GetANeighborPos(int index, Vector3Int pos)
    {
        // decide to use four neighbors system or eight neighbors system here
        int neighborSize = m_neighborMode.Count;
        int start = m_rng.GetRange(0, neighborSize - 1);
        int end = start + neighborSize;
        for (int i = start; i < end; i++)
        {
            int posIndex = i < neighborSize ? i : i - start;
            Vector3Int neighborPos = pos + m_neighborMode[posIndex];
            if (!m_discovered.ContainsValue(neighborPos))
            {
                return neighborPos;
            }
        }
        return GetANeighborPos(index, pos + m_neighborMode[start]);
    }
    #endregion

    #region Helper
    private bool IsATeleport(Vector3Int nodeAPos, Vector3Int nodeBPos)
    {
        return m_isUsingTeleport && Mathf.Abs((nodeAPos - nodeBPos).magnitude) > m_minimumTeleportMagnitude;
    }
    #endregion
}
