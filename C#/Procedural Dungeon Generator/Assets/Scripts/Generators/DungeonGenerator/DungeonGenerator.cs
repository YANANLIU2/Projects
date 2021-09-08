using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Tilemaps;

// The script is for generating a playable dungeon
public class DungeonGenerator : MonoBehaviour
{
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
    [Tooltip("Map tiles with types")]
    [SerializeField] private TilesData m_tilesData = null;

    [Header("Tile")]
    [Tooltip("The basic tile used for the whole dungeon")]
    [SerializeField] private TileBase m_dungeonTile = null;

    // To access processed rooms
    private MapGenerator m_mapGenerator;

    // To access the graph
    private GraphGenerator m_graphGenerator;

    // Is initialized?
    private bool m_isInit = false;

    // Constants 
    public const int kEntityLayer = 1;
    public const int kBaseLayer = 0;

    private void Start()
    {
        // Tiledata
        m_tilesData.Initialize();

        // Reference
        m_mapGenerator = FindObjectOfType<MapGenerator>();
        m_graphGenerator = FindObjectOfType<GraphGenerator>();
        if (!m_mapGenerator || !m_graphGenerator) 
        { 
            return; 
        }

        // Initialization
        m_rng.Initialize();
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
        // Reset
        Reset();

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
                    TilemapManager.Instance.SetTile(tilePos, m_dungeonTile, kBaseLayer);
                }
            }
            // Room interior
            Node node = m_graphGenerator.Graph.GetNodeById(room.Key);
            GenerateRoomInterior(node.Type, roomPos);
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
                TilemapManager.Instance.SetTile(new Vector3Int(w, h, 0), m_dungeonTile, kBaseLayer);
            }
        }
    }

    private void CreateATeleport(Vector3Int fromRoomPos, Vector3Int toRoomPos)
    {
        // From pos => to pos
        Vector3Int spawnPos = GetAnEmptyPos(fromRoomPos, 1);
        Vector3Int telePos = GetAnEmptyPos(toRoomPos, 1);

        // Set the tile
        CreateEntity(TilesData.ETileType.kTeleport, spawnPos);
        CreateEntity(TilesData.ETileType.kTeleport, telePos);
        CreateEntity(TilesData.ETileType.kTeleportArrow, spawnPos + new Vector3Int(0, 1, 0));

        // Get Instantiated object from the tile map
        GameObject fromObj = TilemapManager.Instance.GetTileGameObject(spawnPos, 1);
        GameObject toObj = TilemapManager.Instance.GetTileGameObject(telePos, 1);

        // Set position
        if (fromObj && toObj)
        {
            Teleport fromTeleport = fromObj.GetComponent<Teleport>();
            Teleport toTeleport = toObj.GetComponent<Teleport>();
            fromTeleport.Target = toTeleport;
            toTeleport.Target = fromTeleport;
        }
    }

    // Get map postion based on a grid position    
    private Vector3Int GetMapPosition(Vector3Int gridPos)
    {
        return gridPos * new Vector3Int(m_roomWidth + m_roomSpacing, m_roomHeight + m_roomSpacing, 0);
    }

    // Get an empty position in a room
    private Vector3Int GetAnEmptyPos(Vector3Int roomPos, int orderInLayer)
    {
        int w = roomPos.x + m_rng.GetRange(1, m_roomWidth - 2);
        int h = roomPos.y + m_rng.GetRange(1, m_roomHeight - 2);
        Vector3Int pos = new Vector3Int(w, h, 0);
        if (TilemapManager.Instance.IsTileEmpty(pos, orderInLayer))
        {
            return pos;
        }
        return GetAnEmptyPos(roomPos, orderInLayer);
    }

    // Reset
    private void Reset()
    {
        TilemapManager.Instance.Reset();
    }

    private void CreateEntity(TilesData.ETileType type, Vector3Int pos)
    {
        TilemapManager.Instance.SetTile(pos, m_tilesData.GetTile(type), kEntityLayer);
    }
    
    private void GenerateRoomInterior(Node.ENodeType roomType, Vector3Int pos)
    {
        CreateEntity(TilesData.ETileType.kCoin, pos + new Vector3Int(1, 1, 0));
    }
}