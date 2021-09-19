using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Encounter issues:
// 1. OnTriggerStay2D only detecting trigger collisions while moving
//    Solution: https://answers.unity.com/questions/1001159/ontriggerstay2d-only-detecting-trigger-collisions.html
public class Trap : Entity
{
    [Tooltip("The damage will be received if the player was on the trap when it's active")]
    [SerializeField] private int m_damage;

    private bool m_isActive = true;
  
    private void OnTriggerStay2D(Collider2D collision)
    {
        // If it is player 
        if (collision.CompareTag(DungeonGenerator.s_playerTag))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player)
            {
                // If the trap is active
                if(m_isActive)
                {
                    if(player.GetTrapDamage(m_damage))
                    {
#if DEBUG
                        Debug.Log(m_debugMsg);
#endif
                    }
                }
            }
        }
    }

    public void ActivateTrap()
    {
        m_isActive = true;
    }

    public void DeactivateTrap()
    {
        m_isActive = false;
    }
}
