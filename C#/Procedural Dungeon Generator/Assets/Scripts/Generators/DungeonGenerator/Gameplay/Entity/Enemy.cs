using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [Tooltip("The damage for each attack")]
    [SerializeField] int m_damage = 3;

    [Tooltip("Decide the frequency the player can attack")]
    [SerializeField] float m_attackCooldownTime = 0.5f;

    [Tooltip("Health points")]
    [SerializeField] private int m_hp;

    [Tooltip("The player can gain the exp points after the enemy gets killed")]
    [SerializeField] private int m_exp;

    private Timer m_timer = null;
    private bool m_isAlive = true;

    public int Hp
    {
        get => m_hp;
        set
        {
            m_hp = value;
            if(m_hp <= 0)
            {
                m_isAlive = false;
            }
        }
    }

    public bool IsAlive { get => m_isAlive; }

    public int Exp { get => m_exp; }

    private void Awake()
    {
        m_timer = new Timer(m_attackCooldownTime);    
    }

    private void Update()
    {
        m_timer.Update();    
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Collider2D collider = collision.collider;
        // If it's player
        if(IsPlayer(collider))
        {
            PlayerController player = collider.GetComponent<PlayerController>();
            // Attack
            if (!m_timer.IsActive)
            {
                player.GetEnemyDamage(m_damage);
                m_timer.IsActive = true;
#if DEBUG
                Debug.Log(m_debugMsg);
#endif
            }

            // Receive attack
            player.Attack(this);
        }
    }
}
