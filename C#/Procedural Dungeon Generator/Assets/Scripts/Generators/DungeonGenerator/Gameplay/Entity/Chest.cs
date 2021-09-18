using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// At the end of the open chest animation. An animation event will call Clear() to destroy the gameobject
public class Chest : Entity
{
    bool m_isOpened = false;
    static string s_openChestParam = "IsOpen";
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(m_isOpened)
        {
            return;
        }

        // If it is player 
        if (IsPlayer(collision))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player)
            {
                OpenChest();
            }
        }
    }

    private void OpenChest()
    {
        m_isOpened = true;
        // Log
#if DEBUG
        Debug.Log(m_debugMsg);
#endif
        // Anim
        Animator animator = GetComponent<Animator>();
        if(animator)
        {
            animator.SetBool(s_openChestParam, true);
        }
    }
}
