using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] private Text m_playerCoinsTxt = null;
    [SerializeField] private Text m_playerHpTxt = null;
    [SerializeField] private Text m_playerMaxHpTxt = null;

    private void Start()
    {
        if (!m_playerCoinsTxt || !m_playerHpTxt || !m_playerMaxHpTxt)
        {
#if DEBUG
            Debug.LogError("References of UI elements are missing");
#endif
        }
        else
        {
            // To do: initialize
        }
    }

    public void SetCoinsUI(int value)
    {
        m_playerCoinsTxt.text = value.ToString();
    }

    public void SetHpUI(int value)
    {
        m_playerHpTxt.text = value.ToString();
    }

    public void SetMaxHpUI(int value)
    {
        m_playerMaxHpTxt.text = value.ToString();
    }
}
