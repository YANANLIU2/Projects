using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHpPotion : Entity
{
    [Tooltip("The max hp increment the player will receive from this potion")]
    [SerializeField] private int m_value;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If it is player 
        if (IsPlayer(collision))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player)
            {
#if DEBUG
                Debug.Log(m_debugMsg);
#endif
                player.MaxHp += m_value;
                Clear();
            }
        }
    }
}

