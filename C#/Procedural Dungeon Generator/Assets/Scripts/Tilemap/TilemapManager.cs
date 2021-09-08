using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
public class TilemapManager : MonoBehaviour
{
    private static TilemapManager s_instance;

    [System.Serializable]
    public struct MyTileMap
    {
        public Tilemap TileMap;
        public int OrderInLayer;
    }

    [SerializeField] private MyTileMap[] m_tileMapList = null;
    private Dictionary<int, Tilemap> m_tileMapDict = null;

    public Tilemap this[int index]
    {
        get => m_tileMapList[index].TileMap;
    }

    public static TilemapManager Instance { get => s_instance; }

    private void Awake()
    {
        if(s_instance!= null && s_instance!= this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            s_instance = this;
        }
        Initialize();
    }

    private void Initialize()
    {
        if(m_tileMapList != null)
        {
            m_tileMapDict = new Dictionary<int, Tilemap>();
            foreach (var item in m_tileMapList)
            {
                m_tileMapDict.Add(item.OrderInLayer, item.TileMap);
            }
        }
    }

    // Set a tile in tilemap 
    // tilebase can be null
    public void SetTile(Vector3Int pos, TileBase tileBase, int orderInLayer = 0)
    {
        if(m_tileMapDict.ContainsKey(orderInLayer))
        {
            Tilemap map = m_tileMapDict[orderInLayer];
            map.SetTile(pos, tileBase);
        }
    }

    // Return the tile's gameobject on the tilemap
    public GameObject GetTileGameObject(Vector3Int pos, int orderInLayer = 0)
    {
        if (m_tileMapDict.ContainsKey(orderInLayer))
        {
            Tilemap map = m_tileMapDict[orderInLayer];
            return map.GetInstantiatedObject(pos);
        }
        return null;
    }

    // Reset
    public void Reset()
    {
        if (m_tileMapList != null)
        {
            foreach (var item in m_tileMapList)
            {
                item.TileMap.ClearAllTiles();
            }
        }
    }

    // Return is the tile on the tilemap empty or not
    public bool IsTileEmpty(Vector3Int pos, int orderInLayer)
    {
        if (m_tileMapDict.ContainsKey(orderInLayer))
        {
            Tilemap map = m_tileMapDict[orderInLayer];
            return map.GetTile(pos) == null;
        }
        return false;
    }
}

