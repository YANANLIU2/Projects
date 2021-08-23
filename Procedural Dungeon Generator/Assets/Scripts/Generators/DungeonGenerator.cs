using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// The script is for generating a playable dungeon
public class DungeonGenerator : MonoBehaviour
{
    #region Tileset
    // A struct for holding a list of tiles of the same type
    [System.Serializable]
    public class TileSet
    {
        public enum TilesetType
        {
            kBase,
            kTopWall,
            kLeftWall,
            kRightWall,
            kBotWall,
            kLeftRightCorridor,
            kTopDownCorridor,
            kCorners,
            kNum
        }

        [Tooltip("Type of the tileset")]
        [SerializeField] private TilesetType m_type;

        [Tooltip("If needs order: left -> right, top -> bot")]
        [SerializeField] private Tile[] m_tiles;

        // Getters & Setters
        public Tile this[int key] { get => m_tiles[key]; }
        public int GetCount() { return m_tiles.Length; }
        public TilesetType Type { get => m_type; }
    }

    [Header("Tilesets")]
    [Tooltip("A list of tilesets of different types")]
    [SerializeField] List<TileSet> m_tileSets;
    #endregion
    [Header("Random Generation")]
    [SerializeField] XorshiftRNG m_rng = new XorshiftRNG();

    [Header("Room Generation")]
    [SerializeField] private int m_roomWidth = 6;
    [SerializeField] private int m_roomHeight = 5;
    [SerializeField] private int m_topDownCorridorWidth = 2;
    [SerializeField] private int m_rightLeftCorridorWidth = 1;

    private Graph m_graph;
    private GraphGenerator m_graphGenerator;
    private DungeonMapGenerator m_mapGenerator;
    private List<Room> m_roomList;

    #region Main Logic
    private void Start()
    {
        // Map Generator
        m_mapGenerator = FindObjectOfType<DungeonMapGenerator>();
        if (!m_mapGenerator)
        {
#if DEBUG
            Debug.Log("The reference of the map generator cannot be null");
#endif
            return;
        }

        // Graph Generator
        m_graphGenerator = FindObjectOfType<GraphGenerator>();
        if(!m_graphGenerator)
        {
#if DEBUG
            Debug.Log("The reference of the graph generator cannot be null");
#endif
            return;
        }
        m_graph = m_graphGenerator.Graph;

        // Initialize the random number generator
        m_rng.Initialize();
        m_roomList = new List<Room>();
        Generate();
    }
    
    // Generate a dungeon
    public void Generate()
    {
        // Reset
        Reset();
        // Rooms
        foreach (var node in m_mapGenerator.Discovered)
        {
            GenerateARoom(node.Value);
        }
        // Corridors
        for (int i = 0; i < m_graph.OutgoingAdjacencyList.Count; i++)
        {
            var outgoingAdjacency = m_graph.OutgoingAdjacencyList[i];
            for (int j = 0; j < outgoingAdjacency.Count; j++)
            {
                int neighborIndex = m_graph.GetIndex(outgoingAdjacency[j]);
                Vector3Int neighborPos;
                if (m_mapGenerator.Discovered.TryGetValue(neighborIndex, out neighborPos))
                {
                    GenerateAConnection(m_mapGenerator.Discovered[i], neighborPos);
                }
            }
        }
    }
    
    private void Reset()
    {
        TilemapManager.Instance.Reset();
        m_roomList.Clear();    
    }
    #endregion

    #region Generate Rooms and Corridors
    // Generate a connection between Room A to Room B
    private void GenerateAConnection(Vector3Int roomAGridPos, Vector3Int roomBGridPos)
    {
        // If same x, the rooms are parallel vertically 
        if(roomAGridPos.x == roomBGridPos.x)
        {
            // A: top B: bot
            if(roomAGridPos.y > roomBGridPos.y)
            {
                GenerateATopDownConnection(roomAGridPos.x * m_roomWidth, roomAGridPos.y * m_roomHeight);
            }
            // A: bot B: top
            else
            {
                GenerateATopDownConnection(roomBGridPos.x * m_roomWidth, roomBGridPos.y * m_roomHeight);
            }
        }
        // If same y, the rooms are parallel horizontally 
        else if (roomAGridPos.y == roomBGridPos.y)
        {
            // A: right B: left
            if (roomAGridPos.x > roomBGridPos.x)
            {
                GenerateAleftRightConnection(roomAGridPos.x * m_roomWidth, roomAGridPos.y * m_roomHeight);
            }
            // A: left B: right
            else
            {
                GenerateAleftRightConnection(roomBGridPos.x * m_roomWidth, roomBGridPos.y * m_roomHeight);
            }
        }
        // Todo: generate teleports here
    }

    // Generate a horizontal connection between Room A to Room B
    private void GenerateATopDownConnection(int startX, int startY)
    {
        int corridorStartX = startX + m_roomWidth/2 - m_topDownCorridorWidth/2;
        int corridorEndX = corridorStartX + m_topDownCorridorWidth - 1;
        // The walkable corridor itself
        for (int j = startY - 1; j < startY + 1; j++)
        {
            for (int i = corridorStartX; i <= corridorEndX; i++)
            {
                TilemapManager.Instance.SetTile(new Vector3Int(i, j, 0), GetRandTile(TileSet.TilesetType.kBase));
            }
        }
        var set = GetTileset(TileSet.TilesetType.kTopDownCorridor);
        // The left of the corridor
        TilemapManager.Instance.SetTile(new Vector3Int(corridorStartX - 1, startY, 0), set[0]);
        // The right of the corridor
        TilemapManager.Instance.SetTile(new Vector3Int(corridorEndX + 1, startY, 0), set[1]);
    }

    // Generate a vertical connection between Room A to Room B
    private void GenerateAleftRightConnection(int startX, int startY)
    {
        int corridorStartY = startY + m_roomHeight / 2 - m_rightLeftCorridorWidth / 2;
        int corridorEndY = corridorStartY + m_rightLeftCorridorWidth;
        // The walkable corridor itself
        for (int j = corridorStartY; j < corridorEndY; j++)
        {
            for (int i = startX - 1; i < startX + 1; i++)
            {
                TilemapManager.Instance.SetTile(new Vector3Int(i, j, 0), GetRandTile(TileSet.TilesetType.kBase));
            }
        }
        // Above the corridor
        for (int i = startX - 1; i < startX + 1; i++)
        {
            TilemapManager.Instance.SetTile(new Vector3Int(i, corridorEndY, 0), GetRandTile(TileSet.TilesetType.kTopWall));
        }
        // Under the corridor
        var set = GetTileset(TileSet.TilesetType.kLeftRightCorridor);
        for (int i = startX - 1, p = 0; i < startX + 1; i++, p++)
        {
            TilemapManager.Instance.SetTile(new Vector3Int(i, corridorStartY - 1, 0), set[p]);
        }
    }

    // Generate a room at the given position: roomWidth * roomHeight
    private void GenerateARoom(Vector3Int gridPos)
    {
        Vector3Int mapPos = GetMapPosition(gridPos);
        // Inner walkable tiles
        int startY = mapPos.y;
        int endY = mapPos.y + m_roomHeight;
        int startX = mapPos.x;
        int endX = mapPos.x + m_roomWidth;
        for (int j = startY + 1; j < endY - 1; j++)
        {
            for (int i = startX + 1; i < endX - 1; i++)
            {
                Vector3Int tilePos = new Vector3Int(i, j, 0);
                TilemapManager.Instance.SetTile(tilePos, GetRandTile(TileSet.TilesetType.kBase));
            }
        }
        // Top
        for (int i = startX + 1; i < endX - 1; i++)
        {
            Vector3Int tilePos = new Vector3Int(i, endY - 1, 0);
            TilemapManager.Instance.SetTile(tilePos, GetRandTile(TileSet.TilesetType.kTopWall));
        }
        // Left
        for (int j = startY + 1; j < endY - 1; j++)
        {
            Vector3Int tilePos = new Vector3Int(startX, j, 0);
            TilemapManager.Instance.SetTile(tilePos, GetRandTile(TileSet.TilesetType.kLeftWall));
        }
        // Right
        for (int j = startY + 1; j < endY - 1; j++)
        {
            Vector3Int tilePos = new Vector3Int(endX - 1, j, 0);
            TilemapManager.Instance.SetTile(tilePos, GetRandTile(TileSet.TilesetType.kRightWall));
        }
        // Bot
        for (int i = startX + 1; i < endX - 1; i++)
        {
            Vector3Int tilePos = new Vector3Int(i, startY, 0);
            TilemapManager.Instance.SetTile(tilePos, GetRandTile(TileSet.TilesetType.kBotWall));
        }
        // Right Top Corner
        var set = GetTileset(TileSet.TilesetType.kCorners);
        TilemapManager.Instance.SetTile(new Vector3Int(startX, endY - 1, 0), set[0]);
        TilemapManager.Instance.SetTile(new Vector3Int(startX, startY, 0), set[1]);
        TilemapManager.Instance.SetTile(new Vector3Int(endX - 1, endY - 1, 0), set[2]);
        TilemapManager.Instance.SetTile(new Vector3Int(endX - 1, startY, 0), set[3]);
    }
    #endregion

    #region Helpers
    // Get the match tileset from the tileset type
    private TileSet GetTileset(TileSet.TilesetType type)
    {
        for (int i = 0; i < m_tileSets.Count; i++)
        {
            if(m_tileSets[i].Type == type)
            {
                return m_tileSets[i];
            }
        }
#if DEBUG
        Debug.LogError("Failed to find the target type tileset");
#endif
        return null;
    }

    // Get a random tile from the target tileset
    private Tile GetRandTile(TileSet.TilesetType type)
    {
        var set = GetTileset(type);
        if(set != null)
        {
            return set[m_rng.GetRange(0, set.GetCount() - 1)];
        }
        else
        {
            return null;
        }
    }

    // Get map postion based on a grid position    
    private Vector3Int GetMapPosition(Vector3Int gridPos) 
    { 
        return gridPos * new Vector3Int(m_roomWidth, m_roomHeight, 0); 
    }
    #endregion
}
