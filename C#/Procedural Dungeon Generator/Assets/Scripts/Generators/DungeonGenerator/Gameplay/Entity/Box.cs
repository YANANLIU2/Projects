using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : Entity
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If it is player 
        if (IsPlayer(collision))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player)
            {
                // Open the box
#if DEBUG
                Debug.Log(m_debugMsg);
#endif
                Clear();
            }
        }
    }
}
