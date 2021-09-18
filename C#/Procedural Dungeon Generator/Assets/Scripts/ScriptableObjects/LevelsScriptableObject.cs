using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/LevelsScriptableObject", order = 1)]
public class LevelsScriptableObject : ScriptableObject
{
    [System.Serializable]
    public class LevelData
    {
        public int Level;
        public int MaxExp;
        public int BonusMaxHp;
        public LevelData(int level, int maxExp) { Level = level; MaxExp = maxExp; }
    }

    [Tooltip("Level Exp Chart")]
    [SerializeField] private List<LevelData> m_levelDataList = null;

    [Tooltip("The minimum level")]
    [SerializeField] private int m_startLevel = 0;

    [Tooltip("The maximum level")]
    [SerializeField] private int m_maxLevel = 20;

    // Getters & Setters
    public LevelData this[int key]
    {
        get
        {
            if(key >= m_startLevel && key <= m_maxLevel)
            {
                return m_levelDataList[key];
            }
            return null;
        }
    }

    public void Reset()
    {
        m_levelDataList = new List<LevelData>();
        for (int i = m_startLevel; i < m_maxLevel; i++)
        {
            m_levelDataList.Add(new LevelData(i, 0));
        }
    }
}