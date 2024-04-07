using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Bug: Teleporting Back and forth
// Solution: https://forum.unity.com/threads/teleporting-back-and-forth.44197/

public class Teleport : Entity
{
    // Is the teleport working now?
    private bool m_isActive = true;

    // The place the player will be teleported to
    private Teleport m_destination = null;
    
    // Getters & Setters
    public Teleport Destination { set => m_destination = value; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsPlayer(collision))
        {
            if (m_isActive)
            {
                m_destination.m_isActive = false;
                collision.gameObject.transform.position = m_destination.gameObject.transform.position;
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (IsPlayer(collision))
        {
            m_isActive = true;
        }
    }
}
