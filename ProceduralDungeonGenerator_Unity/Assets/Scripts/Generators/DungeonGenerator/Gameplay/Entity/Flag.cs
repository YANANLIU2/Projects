using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : Entity
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
                // Win the game
                player.IsWin = true;
            }
        }
    }
}
