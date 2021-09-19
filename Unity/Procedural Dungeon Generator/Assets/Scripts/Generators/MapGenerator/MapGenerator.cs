using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

// - Give nodes positions
// - Draw nodes and connections on a minimap 
public class MapGenerator : MonoBehaviour
{
    [Header("Generation Settings")]
    [Tooltip("Seed can greatly effect the whole random generation")]
    [SerializeField] private XorshiftRNG m_rng = new XorshiftRNG();

    [Tooltip("Neighbor tiles of one tile")]
    [SerializeField] private NeighborMode.EType m_neighborModeType;

    [System.Serializable]
    private struct IconsData
    {
        public TileBase Icon;
        public Node.ENodeType Type;
    }

    [Tooltip("Map room icons and node types")]
    [SerializeField] private List<IconsData> m_iconDataList = null;
    private Dictionary<Node.ENodeType, TileBase> m_iconDataDict = null;

    [Header("References")]
    [Tooltip("For adjacent connections")]
    [SerializeField] private LineRenderer m_connectionPrefab = null;

    [Tooltip("The UI text prefab that displays a node's name and Id")]
    [SerializeField] private GameObject m_nodeTextPrefab = null;

    [Tooltip("Reference of the tilemap")]
    [SerializeField] private Tilemap m_tilemap = null;

    // Read-onlys 
    private const int kTileSpace = 2;
    public readonly Vector3 m_CenterOffset = new Vector3(0.5f, 0.5f, 0);

    // private variables
    private Graph m_graph;
    private NeighborMode m_neighborMode;
    private GraphGenerator m_graphGenerator = null;

    public struct Connection
    {
        public Connection(Vector3Int from, Vector3Int end) { FromPosition = from; EndPosition = end; }
        public Vector3Int FromPosition;
        public Vector3Int EndPosition;
        public bool IsAdjacent()
        {
            return Mathf.Abs(FromPosition.x - EndPosition.x) + Mathf.Abs(FromPosition.y - EndPosition.y) == 1;
        }
    }

    private Dictionary<uint, Vector3Int> m_roomsDict = null;
    private List<Connection> m_connectionsList = null;
    private List<GameObject> m_instantiatedObjects = null;

    // Getters & Setters
    public Dictionary<uint, Vector3Int> Rooms { get => m_roomsDict; }
    public List<Connection> Connections { get => m_connectionsList; }

    // Strategy pattern
    public int NeighborMode 
    { 
        set 
        { 
            m_neighborModeType = (NeighborMode.EType)value;
            switch (m_neighborModeType)
            {
                case global::NeighborMode.EType.kFourNeighbors:
                    m_neighborMode = new FourNeighborMode();
                    break;
                case global::NeighborMode.EType.kEightNeighbors:
                    m_neighborMode = new EightNeighborMode();
                    break;
                default:
                    break;
            }
        } 
    }

    // Initialization
    private void Awake()
    {
        // Tilemap reference check
        if(!m_tilemap)
        {
#if DEBUG
            Debug.LogError("Tilemap reference cannnot be null");
#endif
            return;
        }
        // Load icon tiles
        if(m_iconDataList != null)
        {
            m_iconDataDict = new Dictionary<Node.ENodeType, TileBase>();
            foreach (var item in m_iconDataList)
            {
                m_iconDataDict.Add(item.Type, item.Icon);
            }
        }
        // Random number generator
        m_rng.Initialize();
        // Graph generator 
        m_graphGenerator = FindObjectOfType<GraphGenerator>();
        if (m_graphGenerator == null) 
        { 
            return; 
        }
        m_graph = m_graphGenerator.Graph;
        // Initialize neighbor modes
        NeighborMode = (int)m_neighborModeType;
        // Initialize lists
        m_instantiatedObjects = new List<GameObject>();
        m_roomsDict = new Dictionary<uint, Vector3Int>();
        m_connectionsList = new List<Connection>();
    }

    // Process a node and assign a position, then head to its neighbors
    //private void ProcessANode(int index, Vector3Int pos)
    //{
    //    // Process itself
    //    m_roomsDict.Add(index, pos);
    //    DrawANode(index, pos);
    //    // Process neighbors
    //    var neighbors = m_graph.OutgoingAdjacencyList[index];
    //    for (int i = 0; i < neighbors.Count; i++)
    //    {
    //        int neighbor = m_graph.GetIndex(neighbors[i]);
    //        // Head deeper if haven't been processed yet
    //        if (!m_roomsDict.ContainsKey(neighbor))
    //        {
    //            Vector3Int neighborPos = GetANeighborPos(pos);
    //            ProcessANode(neighbor, neighborPos);
    //        }
    //    }
    //}

    // Generate and draw a map
    public void Generate()
    {
        // Constants
        Vector3Int startPos = Vector3Int.zero;

        // Reset
        Reset();

        // Conditions check
        if (m_graph == null || !m_graph.IsValid())
        {
            return;
        }

        // Generate rooms and connections in a BFS
        Queue<uint> openSet = new Queue<uint>();
        List<uint> explored = new List<uint>();

        // Add root
        uint root = m_graph[0].Id;
        explored.Add(root);
        openSet.Enqueue(root);
        m_roomsDict.Add(root, startPos);
        DrawANode(root, startPos);

        // BFS 
        while (openSet.Count!=0)
        {
            uint id = openSet.Dequeue();
            // Draw the room
            Vector3Int pos = m_roomsDict[id];

            // Handle neighbors
            var neighbors = m_graph.OutgoingAdjacencyList[m_graph.GetIndex(id)];
            for (int i = 0; i < neighbors.Count; i++)
            {
                uint neighbor = neighbors[i];
                if(!explored.Contains(neighbor))
                {
                    openSet.Enqueue(neighbor);
                    explored.Add(neighbor);

                    // Assign a pos to the neighbor
                    Vector3Int neighborPos = GetANeighborPos(pos);
                    m_roomsDict.Add(neighbor, neighborPos);
                    DrawANode(neighbor, neighborPos);

                    // Add and draw a connection
                    AddAConnection(pos, neighborPos);
                }
            }
        }
    }

    private void Reset()
    {
        // Clear lists
        if(m_roomsDict != null)
        {
            m_roomsDict.Clear();
        }
        if(m_connectionsList != null)
        {
            m_connectionsList.Clear();
        }
        // Screen
        m_tilemap.ClearAllTiles();
        // Clear the text objects
        if (m_instantiatedObjects != null)
        {
            foreach (var item in m_instantiatedObjects)
            {
                Destroy(item);
            }
            m_instantiatedObjects.Clear();
        }
    }

    // Draw a connection between node A and node B
    private void AddAConnection(Vector3Int nodeA, Vector3Int nodeB)
    {
        m_connectionsList.Add(new Connection(nodeA, nodeB));
        DrawALine(nodeA * kTileSpace + m_CenterOffset, nodeB * kTileSpace + m_CenterOffset, m_connectionPrefab);
    }

    // Draw a tile's icon and name on screen
    private void DrawANode(uint id, Vector3Int pos)
    {
        Vector3Int displayPos = pos * kTileSpace;
        int index = m_graph.GetIndex(id);
        // Icon
        Node.ENodeType type = m_graph[index].Type;
        m_tilemap.SetTile(displayPos, m_iconDataDict[type]);
        // Name + Index(Id is too long to display)
        SetText(displayPos, type.ToString() + index.ToString());
    }

    // Set the text at the target location
    public void SetText(Vector3 displayPos, string content)
    {
        GameObject indexTextObj = Instantiate(m_nodeTextPrefab);
        indexTextObj.transform.position = displayPos;
        indexTextObj.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = content;
        m_instantiatedObjects.Add(indexTextObj);
    }

    // Draw a line between 2 points on screen
    public void DrawALine(Vector3 nodeAPos, Vector3 nodeBPos, LineRenderer prefab)
    {
        LineRenderer lRend = Instantiate(prefab);
        lRend.SetPosition(0, nodeAPos);
        lRend.SetPosition(1, nodeBPos);
        m_instantiatedObjects.Add(lRend.gameObject);
    }

    // Get a valid neighbor pos
    Vector3Int GetANeighborPos(Vector3Int pos)
    {
        // Start from a random neighbor
        int size = m_neighborMode.Count;
        int start = m_rng.GetRange(0, size - 1);
        int end = start + size;
        // Loop
        for (int i = start; i < end; i++)
        {
            int posIndex = i < size ? i : i - start;
            Vector3Int neighbor = pos + m_neighborMode[posIndex];
            if(m_tilemap.GetTile(neighbor * kTileSpace) == null)
            {
                return neighbor;
            }
        }
        // Head deeper
        return GetANeighborPos(pos + m_neighborMode[start]);
    }
}
