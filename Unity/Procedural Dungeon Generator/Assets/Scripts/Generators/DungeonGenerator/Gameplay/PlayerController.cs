using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

// The script is for handling the player's input  
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("How fast the player can move")]
    [SerializeField] private Vector2 m_speed = new Vector2(50, 50);

    [Header("Axis")]
    [Tooltip("Horizontal axis name")]
    [SerializeField] private string m_horizontalAxis = "Horizontal";

    [Tooltip("Vertical axis name")]
    [SerializeField] private string m_verticalAxis = "Vertical";

    [Header("Stats")]
    [Tooltip("Health points")]
    [SerializeField] private int m_hp = 10;

    [Tooltip("Max health points")]
    [SerializeField] private int m_maxHp = 10;

    [Tooltip("Money")]
    [SerializeField] private int m_coins = 0;

    [Tooltip("Decide the player's attack frequency")]
    [SerializeField] private float m_attackTime = 0.6f;

    [Tooltip("The damage of one attack")]
    [SerializeField] private int m_damage = 3;

    [Tooltip("The protectin time after the player gets damage")]
    [SerializeField] private float m_shieldTime = 1f;

    [Header("Reference")]
    [Tooltip("To update UI information about the player")]
    [SerializeField] private GameUIManager m_gameUIManager = null;

    [Tooltip("To acess level up info")]
    [SerializeField] private LevelsScriptableObject m_levelsScriptableObject = null;

    // Key chain
    private Dictionary<Key.EKeyType, int> m_keyChain = null;

    // Start position
    private Vector3 m_standardPos;

    // Is alive?
    private bool m_isAlive;

    // The timer will be on after the player receives damage from a trap
    private Timer m_shieldTimer = null;

    // The timer will be on after the player attacks
    private Timer m_attackTimer = null;

    // Did the player win the game
    private bool m_isWin = false;

    // Level 
    private int m_level = 0;

    // Exp
    private int m_exp = 0;

    // Max exp of the level
    private int m_maxExp = 0;

    // Getters & Setters
    public int Coins
    {
        get => m_coins;
        set
        {
            m_coins = value;
            m_gameUIManager.SetTextUI(GameUIManager.EType.kCoin, value);
        }
    }

    public int Hp
    {
        get => m_hp;
        set
        {
            m_hp = value;
            m_gameUIManager.SetBarUI(GameUIManager.EType.kHealthBar, value, m_maxHp);
        }
    }

    public int MaxHp
    {
        get => m_maxHp;
        set
        {
            m_maxHp = value;
            m_gameUIManager.SetBarUI(GameUIManager.EType.kHealthBar, m_hp, value);
        }
    }

    public int Exp
    {
        get => m_exp;
        set
        {
            m_exp = value;
            // Check for leveling up
            if (m_exp >= m_maxExp)
            {
                LevelUp();
            }
            // Update UI
            m_gameUIManager.SetBarUI(GameUIManager.EType.kExpBar, value, m_maxExp);
        }
    }

    public int MaxExp
    {
        get => m_maxExp;
        set
        {
            m_maxExp = value;
            // Update UI
            m_gameUIManager.SetBarUI(GameUIManager.EType.kExpBar, m_exp, value);
        }
    }

    public bool IsWin
    {
        set
        {
            m_isWin = value;
            if(m_isWin)
            {
                m_gameUIManager.Win();
            }
        }
    }

    public bool IsAlive
    {
        set
        {
            m_isAlive = value;
            if(!m_isAlive)
            {
                m_gameUIManager.Lose();
            }
        }
    }

    // Gain key
    public void AddKey(Key.EKeyType type)
    {
        m_keyChain[type]++;
        switch (type)
        {
            case Key.EKeyType.kSilver:
                m_gameUIManager.SetTextUI(GameUIManager.EType.kSilverKey, m_keyChain[type]);
                break;
            case Key.EKeyType.kGold:
                m_gameUIManager.SetTextUI(GameUIManager.EType.kGoldKey, m_keyChain[type]);
                break;
            default:
                break;
        }
    }

    #region Main Loop
    // Start: record
    private void Start()
    {
        // Timer
        m_shieldTimer = new Timer(m_shieldTime);
        m_attackTimer = new Timer(m_attackTime);

        // Keys
        m_keyChain = new Dictionary<Key.EKeyType, int>();
        for (int i = 0; i < (int)Key.EKeyType.kNum; i++)
        {
            m_keyChain.Add((Key.EKeyType)i, 0);
        }

        // Stats
        m_standardPos = transform.position;
        IsAlive = true;

        // UI
        if(!m_gameUIManager)
        {
#if DEBUG
            Debug.LogWarning("The UI manager reference is missing");
#endif
        }
        Coins = m_coins;
        Hp = m_hp;
        MaxExp = m_levelsScriptableObject[m_level].MaxExp;
    }

    // Update: movement
    private void Update()
    {
        // Quit?
        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        // Alive?
        if(!m_isAlive || m_isWin)
        {
            return;
        }

        // Move
        Movement();

        // Timers
        if(m_shieldTimer.IsActive)
        {
            m_shieldTimer.Update();
        }
        if (m_attackTimer.IsActive)
        {
            m_attackTimer.Update();
        }
    }

    // Reset: position ... etc
    public void Reset()
    {
        transform.position = m_standardPos;
        IsAlive = true;
    }
    #endregion

    #region Hp Management

    // Get damage from a trap
    public bool GetTrapDamage(int value)
    {
        if (m_isAlive && !m_shieldTimer.IsActive)
        {
            m_shieldTimer.IsActive = true;
            if (value > 0)
            {
                Hp -= value;
                if (m_hp <= 0)
                {
                    IsAlive = false;
                }
                return true;
            }
        }
        return false;
    }

    // Get damage from an enemy
    public void GetEnemyDamage(int value)
    {
        if (m_isAlive && value > 0)
        {
            Hp -= value;
            if (m_hp <= 0)
            {
                IsAlive = false;
            }
        }
    }

    // Get heal
    public void Heal(int value)
    {
        if(value > 0)
        {
            m_hp += value;
            if(m_hp > m_maxHp)
            {
                m_hp = m_maxHp;
            }
            Hp = m_hp;
        }
    }
    #endregion

    // Level up the player
    private void LevelUp()
    {
        // Update hp 
        m_exp -= m_maxExp;

        // Update max hp from chart
        m_level++;
        var levelData = m_levelsScriptableObject[m_level];
        MaxExp = levelData.MaxExp;
        MaxHp += levelData.BonusMaxHp;
        m_gameUIManager.SetTextUI(GameUIManager.EType.kLevel, m_level);
    }

    // Move horizontally && vertically
    private void Movement()
    {
        float inputX = Input.GetAxis(m_horizontalAxis);
        float inputY = Input.GetAxis(m_verticalAxis);
        Vector3 movement = new Vector3(m_speed.x * inputX, m_speed.y * inputY);
        movement *= Time.deltaTime;
        transform.Translate(movement);
    }

    public void Attack(Enemy enemy)
    {
        if (!m_attackTimer.IsActive)
        {
            m_attackTimer.IsActive = true;
            enemy.Hp -= m_damage;
            if(!enemy.IsAlive)
            {
                // Get the exp
                Exp += enemy.Exp;
                // Destroy the enemy
                Destroy(enemy.gameObject);
            }
        }
    }
}
