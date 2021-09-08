using System.Collections; 
using System.Collections.Generic;
using UnityEngine;

// Reference: Solved the bug of teleporting back and forth: https://forum.unity.com/threads/teleporting-back-and-forth.44197/
public class Teleport : Entity
{
    static Vector3 s_tileCenter = new Vector3(0.5f, 0.5f, 0);

    private Teleport m_target = null;
    private bool m_isActive = true; 

    public Teleport Target
    {
        set => m_target = value;
        get => m_target;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!m_isActive)
        {
            m_isActive = true;
        }
    }

    protected override void Interact(PlayerController player)
    {
        if (m_isActive == true)
        {
            Target.m_isActive = false;
            // Teleporter collider
            player.transform.position = m_target.transform.position + s_tileCenter;
        }
    }
}
