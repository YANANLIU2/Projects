using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The player will pick up the coin when trigger enters 
public class Coin : Entity
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If it is player 
        if (IsPlayer(collision))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player)
            {
                // Pick up the coin
#if DEBUG
                Debug.Log(m_debugMsg);
#endif
                player.Coins++;
                Clear();
            }
        }
    }
}
