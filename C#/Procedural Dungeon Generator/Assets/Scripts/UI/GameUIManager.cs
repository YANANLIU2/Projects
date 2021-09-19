using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    // To identify the type of an text UI
    public enum EType
    {
        kCoin, 
        kGoldKey,
        kSilverKey,
        kHealthBar,
        kExpBar,
        kLevel,
        kNum
    }

    // To contain Text UI info
    [System.Serializable]
    public struct TextUI
    {
        public EType Type;
        public Text Text;
    }

    // to contain bar UI info
    [System.Serializable]
    public class BarUI
    {
        public EType Type;
        public Text ValueText;
        public Text MaxValueText;
        public GameObject Bar;
    }

    [Header("Reference")]
    [Tooltip("Match text UI objects and their types here")]
    [SerializeField] private List<TextUI> m_textUIList = null;

    [Tooltip("Match bar UI objects and their types here")]
    [SerializeField] private List<BarUI> m_barUIList = null;

    [Tooltip("The text will show while winning the game")]
    [SerializeField] private GameObject m_winTxt = null;

    [Tooltip("The text will show while losing the game")]
    [SerializeField] private GameObject m_LoseTxt = null;

    private Dictionary<EType, Text> m_textUIDict = null;
    private Dictionary<EType, BarUI> m_barUIDict = null;

    // Initialize Text UI dictionary
    private void Awake()
    {
        m_textUIDict = new Dictionary<EType, Text>();
        foreach (var item in m_textUIList)
        {
            m_textUIDict.Add(item.Type, item.Text);
        }

        m_barUIDict = new Dictionary<EType, BarUI>();
        foreach (var item in m_barUIList)
        {
            m_barUIDict.Add(item.Type, item);
        }
    }

    // Set the target text UI to the value
    public void SetTextUI(EType type, int value)
    {
        var text = m_textUIDict[type];
        if(text)
        {
            text.text = value.ToString();
        }
    }    
    
    // Update the target bar UI 
    public void SetBarUI(EType type, int value, int maxValue)
    {
        var bar = m_barUIDict[type];
        if(bar != null)
        {
            // Update value text
            bar.ValueText.text = value.ToString();
            // Update max value text
            bar.MaxValueText.text = maxValue.ToString();
            // Update bar
            if(maxValue != 0)
            {
                float ratio = value / (float)maxValue;
                bar.Bar.transform.localScale = ratio > 0 ? new Vector3(ratio, 1, 1) : new Vector3(0, 1, 1);
            }
        }
    }

    // Win the gamme 
    public void Win()
    {
        m_winTxt.SetActive(true);
    }

    // Lose the game
    public void Lose()
    {
        m_LoseTxt.SetActive(true);
    }
}
