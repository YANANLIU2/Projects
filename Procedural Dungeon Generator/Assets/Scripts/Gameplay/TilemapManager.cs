using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class TilemapManager : MonoBehaviour
{
    [SerializeField] private static TilemapManager s_instance;

    // A struct for mapping the room types and sprites
    [System.Serializable]
    public struct RoomSprite
    {
        public TileBase TileBase;
        public Node.ENodeType Type;
    }

    [Header("References")]
    [Tooltip("Map the node types and sprits")]
    [SerializeField] private List<RoomSprite> m_nodeSpritesList = null;

    [Header("Tilemap")]
    [SerializeField] private Tilemap m_tileMap;

    [Tooltip("The UI text prefab that displays a node's name and Id")]
    [SerializeField] private GameObject m_nodeTextPrefab = null;

    private readonly string m_outRangeErrorMsg = "Node type is out of range";
    public static TilemapManager Instance { get { return s_instance; } }
    private List<GameObject> m_instantiatedObjects = null;

    private void Awake()
    {
        if (s_instance != null && s_instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            s_instance = this;
            m_instantiatedObjects = new List<GameObject>();
        }
    }

    // Set a tile in tilemap 
    public void SetTile(Vector3Int pos, TileBase tileBase)
    {
        if (tileBase)
        {
            m_tileMap.SetTile(pos, tileBase);
        }
    }

    public void SetTile(Vector3Int pos, Node.ENodeType type)
    {
        int index = (int)type;
        if (index >= 0 && index < (int)Node.ENodeType.kNum)
        {
            TileBase tileBase = m_nodeSpritesList[index].TileBase;
            m_tileMap.SetTile(pos, tileBase);
        }
        else
        {
#if DEBUG
            Debug.Log(m_outRangeErrorMsg);
#endif
        }

    }

    public void SetText(Vector3 displayPos, string content)
    {
        GameObject indexTextObj = Instantiate(m_nodeTextPrefab);
        indexTextObj.transform.position = displayPos;
        indexTextObj.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = content;
        m_instantiatedObjects.Add(indexTextObj);
    }

    public void Reset()
    {
        if(m_tileMap)
        {
            m_tileMap.ClearAllTiles();
        }
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

    // Draw a line between 2 points on screen
    public void DrawALine (Vector3 nodeAPos, Vector3 nodeBPos, LineRenderer line)
    {
        LineRenderer lRend;
        lRend = Instantiate(line);
        lRend.SetPosition(0, nodeAPos);
        lRend.SetPosition(1, nodeBPos);
        m_instantiatedObjects.Add(lRend.gameObject);
    }
}

