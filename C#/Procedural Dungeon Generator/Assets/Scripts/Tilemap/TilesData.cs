using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class TilesData
{
    public enum ETileType
    {
        kTeleport,
        kTeleportArrow,
        kCoin,
        kSmallHpPotion,
        kLargeHpPotion,
        kSmallMaxHpPotion,
        kLargeMaxHpPotion,
    }

    [System.Serializable]
    private struct TileData
    {
        public ETileType Type;
        public string Address;
    }

    [SerializeField] private TileData[] m_tileDataList = null;
    private Dictionary<ETileType, TileBase> m_tileDataDict = null;
    private XorshiftRNG m_rng = null;

    public void Initialize()
    {
        m_rng = new XorshiftRNG();
        m_rng.Initialize();

        // Load single tile data 
        if(m_tileDataList != null)
        {
            m_tileDataDict = new Dictionary<ETileType, TileBase>();
            // Load each address
            foreach (var item in m_tileDataList)
            {
                Object data = Resources.Load(item.Address, typeof(TileBase));
                TileBase tile = (TileBase)data;
                m_tileDataDict.Add(item.Type, tile);
            }
        }
    }

    // Get the target tile
    public TileBase GetTile(ETileType type)
    {
        return m_tileDataDict[type];
    }
}
