using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages all entities in the game
public class EntityManager : MonoBehaviour
{
    public enum EEntityType
    {
        kArrow,
        kBox1,
        kBox2,
        kCandlestick1,
        kCandlestick2,
        kChest,
        kCoin,
        kFlag,
        kFlamethrower1,
        kFlamethrower2,
        kFlask1,
        kFlask2,
        kFlask3,
        kFlask4,
        kKey1,
        kKey2,
        kMiniBox1,
        kMiniBox2,
        kMiniChest,
        kPeaks,
        kSideTorch,
        kTorch,
        kSkeleton1v1,
        kSkeleton1v2,
        kSkeleton2v1,
        kSkeleton2v2,
        kSkull1v1,
        kSkull1v2,
        kVampire1v1,
        kVampire1v2,
        kPriest1v1,
        kPriest1v2,
        kPriest2v1,
        kPriest2v2,
        kPriest3v1,
        kPriest3v2,
        kTeleportEntrance,
        kTeleportExit,
        kNum
    }

    [System.Serializable]
    public struct EntityData
    {
        public EEntityType Type;
        public string Address;
        public EntityData(EEntityType type, string address) { Type = type; Address = address; }
    }

    [Header("Data")]
    [Tooltip("Match entity type and its corresponding prefab here")]
    [SerializeField] private List<EntityData> m_entityDataList = null;
    private Dictionary<EEntityType, GameObject> m_entityDataDict = null;

    private void Start()
    {
        // Init entity data dictionary
        m_entityDataDict = new Dictionary<EEntityType, GameObject>();
        for (int i = 0; i < (int)EEntityType.kNum; i++)
        {
            GameObject obj = Resources.Load(m_entityDataList[i].Address) as GameObject;
            if(obj == null)
            {
#if DEBUG
                Debug.LogError("Failed loading entity prefab from resources");
#endif
            }
            m_entityDataDict.Add(m_entityDataList[i].Type, obj);
        }
    }

    public GameObject CreateEntity(Vector3 pos, EEntityType type)
    {
        return Instantiate(m_entityDataDict[type], pos, Quaternion.identity);
    }

    // A quick way to set entity data list
    public void Reset()
    {
        const string folderName = "Prefabs/";
        m_entityDataList = new List<EntityData>();
        for (int i = 0; i < (int)EEntityType.kNum; i++)
        {
            EEntityType type = (EEntityType)i;
            string name = type.ToString().Substring(1);
            m_entityDataList.Add(new EntityData(type, folderName + name));
        }
    }
}