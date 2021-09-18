using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Tilemaps;

// The script is for generating a playable dungeon
public class DungeonGenerator : MonoBehaviour
{
    // Defines the probability of having an entity for a room
    [System.Serializable]
    public struct EntityProbability
    {
        // The entity
        public EntityManager.EEntityType Type;
        // The chance
        public float Probability;
    }

    // Defines a room's possible entities
    [System.Serializable]
    public class RoomData
    {
        [SerializeField] private Node.ENodeType m_roomType;
        [SerializeField] private List<EntityProbability> m_entities;
        public Node.ENodeType RoomType { get => m_roomType; set => m_roomType = value; }
        public int GetCount() { return m_entities.Count; }
        public EntityProbability this[int key] { get => m_entities[key]; }
    }

    [Header("Data")]
    [Tooltip("Match room type and its corresponding entity interiors here")]
    [SerializeField] private List<RoomData> m_roomDataList = null;

    [Header("Random Generation")]
    [Tooltip("Random number generator")]
    [SerializeField] XorshiftRNG m_rng = new XorshiftRNG();

    [Tooltip("Width of a room")]
    [SerializeField] int m_roomWidth = 6;

    [Tooltip("Height of a room")]
    [SerializeField] int m_roomHeight = 5;

    [Tooltip("Spacing between rooms")]
    [SerializeField] int m_roomSpacing = 1;

    [Tooltip("Height of a corridor connection")]
    [SerializeField] int m_corridorHeight = 3;

    [Tooltip("Width of a corridor connection")]
    [SerializeField] int m_corridorWidth = 4;

    [Header("Reference")]
    [Tooltip("The basic tile used for the whole dungeon")]
    [SerializeField] private TileBase m_dungeonTile = null;

    [Tooltip("The tilemap for the dungeon tiles")]
    [SerializeField] private Tilemap m_tilemap = null;

    [Tooltip("To acess entity data")]
    [SerializeField] private EntityManager m_entityManager = null;

    [Tooltip("To filter out layers except entity layer")]
    [SerializeField] private LayerMask m_entityLayerMask;

    // To access processed rooms
    private MapGenerator m_mapGenerator;

    // To access the graph
    private GraphGenerator m_graphGenerator;

    // Is initialized?
    private bool m_isInit = false;

    // To access different types of rooms' preset entities
    private Dictionary<Node.ENodeType, RoomData> m_roomDataDict = null;

    // Static 
    public static string s_playerTag = "Player";

    private readonly Vector3 m_invalidPos = new Vector3(-1, -1, -1);
    
    private void Start()
    {
        // Tilemap reference check
        if (!m_tilemap)
        {
#if DEBUG
            Debug.LogError("Tilemap reference cannnot be null");
#endif
            return;
        }

        // Entity reference check
        if(!m_entityManager)
        {
#if DEBUG
            Debug.LogError("EntityManager reference cannnot be null");
#endif
            return;
        }

        // Map generator reference check
        m_mapGenerator = FindObjectOfType<MapGenerator>();
        if (!m_mapGenerator)
        {
#if DEBUG
            Debug.LogError("Map generator reference cannnot be null");
#endif
            return;
        }

        // Graph generator reference check
        m_graphGenerator = FindObjectOfType<GraphGenerator>();
        if (!m_graphGenerator)
        {
#if DEBUG
            Debug.LogError("Graph generator reference cannnot be null");
#endif
            return;
        }

        // Rng
        m_rng.Initialize();

        // Room data 
        m_roomDataDict = new Dictionary<Node.ENodeType, RoomData>();
        for (int i = 0; i < m_roomDataList.Count; i++)
        {
            var data = m_roomDataList[i];
            m_roomDataDict.Add(data.RoomType, data);
        }
    }

    private void Update()
    {
        // Tile's instantiated objects will be created after Start(). So to access them, we cannot generate the dungeon in Start().
        if (!m_isInit)
        {
            Generate();
            m_isInit = true;
        }
    }

    // Generate a dungeon
    public void Generate()
    {
        // Check
        if (!m_mapGenerator || !m_graphGenerator) 
        { 
            return; 
        }

        // Draw rooms 
        foreach (var room in m_mapGenerator.Rooms)
        {
            // Room position
            Vector3Int roomPos = GetMapPosition(room.Value);
            for (int h = 0; h < m_roomHeight; h++)
            {
                for (int w = 0; w < m_roomWidth; w++)
                {
                    // Set base tiles
                    Vector3Int tilePos = roomPos + new Vector3Int(w, h, 0);
                    m_tilemap.SetTile(tilePos, m_dungeonTile);
                }
            }
            Node node = m_graphGenerator.Graph.GetNodeById(room.Key);
            // Room interior
            GenerateRoomInterior(roomPos, node.Type);
        }

        // Draw connections
        for (int i = 0; i < m_mapGenerator.Connections.Count; i++)
        {
            var connection = m_mapGenerator.Connections[i];
            Vector3Int fromPosition = GetMapPosition(connection.FromPosition);
            Vector3Int endPosition = GetMapPosition(connection.EndPosition);
            if(connection.IsAdjacent())
            {
                DrawCorridor(fromPosition, endPosition);
            }
            else
            {
                CreateATeleport(fromPosition, endPosition);
            }
        }
    }

    private void DrawCorridor(Vector3Int fromPosition, Vector3Int endPosition)
    {
        int startX = 0, startY = 0, endX = 0, endY = 0;
        // If horizontal
        if (fromPosition.y == endPosition.y)
        {
            // Smaller x will be start; Bigger x will be end
            if (fromPosition.x < endPosition.x)
            {
                startX = fromPosition.x;
                endX = endPosition.x;
            }
            else
            {
                startX = endPosition.x;
                endX = fromPosition.x;
            }
            startY = fromPosition.y + m_roomHeight / 2 - 1; // The start Y of the corridor
            endY = startY + m_corridorHeight; // Add the corridor's height
        }
        // If vertical
        else
        {
            // Smaller y will be start; Bigger y will be end
            if (fromPosition.y < endPosition.y)
            {
                startY = fromPosition.y;
                endY = endPosition.y;
            }
            else
            {
                startY = endPosition.y;
                endY = fromPosition.y;
            }
            startX = fromPosition.x + m_roomWidth / 2 - 2; // The start X of the corridor
            endX = startX + m_corridorWidth; // Add the corridor's width
        }
        // Set tiles 
        for (int h = startY; h < endY; h++)
        {
            for (int w = startX; w < endX; w++)
            {
                Vector3Int tilePos = new Vector3Int(w, h, 0);
                m_tilemap.SetTile(tilePos, m_dungeonTile);
            }
        }
    }

    // Get map postion based on a grid position    
    private Vector3Int GetMapPosition(Vector3Int gridPos)
    {
        return gridPos * new Vector3Int(m_roomWidth + m_roomSpacing, m_roomHeight + m_roomSpacing, 0);
    }

    // Generate entities in a room
    private void GenerateRoomInterior(Vector3Int roomPos, Node.ENodeType type)
    {
        // Get data
        var data = m_roomDataDict[type];
        // Generate each entity
        for (int i = 0; i < data.GetCount(); i++)
        {
            var entity = data[i];
            // If the entity's probability is higher than the random norm value => create the entity
            if(entity.Probability >= m_rng.GetNorm())
            {
                // Get a possible pos
                Vector3 pos = GetRandPosInRoom(roomPos);
                
                // If the pos is valid we generate the entity. Otherwise, skip it => in case the room is full 
                if(pos != m_invalidPos)
                {
                    GameObject obj = m_entityManager.CreateEntity(pos, entity.Type);
                }
            }
        }
    }

    // Get a random position in a room
    // If max reroll times < 0 => unlimited reroll times
    private Vector3 GetRandPosInRoom(Vector3Int pos, int maxReroll = 3, int reroll = 0)
    {
        // A random pos in the room
        Vector3 randPos = pos + new Vector3(m_rng.GetRange(1, m_roomWidth - 2), m_rng.GetRange(1, m_roomHeight - 2), 0);

        // Overlap point
        Collider2D collider = Physics2D.OverlapPoint(randPos, m_entityLayerMask);

        // If the position is occupied 
        if(collider != null)
        {
            // If reroll times is limited
            if(maxReroll > 0)
            {
                reroll++;
                // Compare 
                if (reroll >= maxReroll)
                {
                    return m_invalidPos;
                }
            }
            // Else reroll
            return GetRandPosInRoom(pos, maxReroll, reroll);
        }
        else
        {
            return randPos;
        }
    }

    private void Reset()
    {
        // A fast way of generating the room data list
        for (int i = 0; i < (int)Node.ENodeType.kNum; i++)
        {
            Node.ENodeType type = (Node.ENodeType)i;
            RoomData data = new RoomData();
            data.RoomType = type;
            m_roomDataList.Add(data);
        }
    }

    private void CreateATeleport(Vector3Int fromRoomPos, Vector3Int toRoomPos)
    {
        // Get positions for entities
        Vector3 fromPos = GetRandPosInRoom(fromRoomPos, -1);
        Vector3 toPos = GetRandPosInRoom(toRoomPos, -1);
        // Instantiate entities
        GameObject entrance = m_entityManager.CreateEntity(fromPos, EntityManager.EEntityType.kTeleportEntrance);
        GameObject exit = m_entityManager.CreateEntity(toPos, EntityManager.EEntityType.kTeleportExit);
        // Set the pair
        Teleport entranceTeleport = entrance.GetComponent<Teleport>();
        Teleport exitTeleport = exit.GetComponent<Teleport>();
        entranceTeleport.Destination = exitTeleport;
        exitTeleport.Destination = entranceTeleport;
    }
}