using UnityEngine;
using UnityEngine.Tilemaps;

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

    [Header("UI Manager")]
    [Tooltip("To update UI information about the player")]
    [SerializeField] GameUIManager m_gameUIManager = null;

    // Start position
    private Vector3 m_standardPos;

    // Is alive?
    private bool m_isAlive;

    // Money
    private int m_coins;

    // Getters & Setters
    public int Coins 
    { 
        get => m_coins;
        set 
        { 
            m_coins = value;
            m_gameUIManager.SetCoinsUI(m_coins);
        }
    }

    public int Hp
    {
        get => m_hp;
        set
        {
            m_hp = value;
            m_gameUIManager.SetHpUI(m_hp);
        }
    }

    public int MaxHp
    {
        get => m_maxHp;
        set
        {
            m_maxHp = value;
            m_gameUIManager.SetMaxHpUI(m_maxHp);
        }
    }


    // Start: record
    private void Start()
    {
        m_standardPos = transform.position;
        m_isAlive = true;
        m_coins = 0;
        if(!m_gameUIManager)
        {
#if DEBUG
            Debug.LogWarning("The UI manager reference is missing");
#endif
        }
    }

    // Update: movement
    private void Update()
    {
        if(!m_isAlive)
        {
            return;
        }
        Movement();
    }

    // Reset: position ... etc
    public void Reset()
    {
        transform.position = m_standardPos;
        m_isAlive = true;
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

    // Get damage 
    public void GetDamage(int value)
    {
        if (value < 0)
        {
            m_hp -= value;
            if(m_hp <= 0)
            {
                m_isAlive = false;
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
        }
    }
}
