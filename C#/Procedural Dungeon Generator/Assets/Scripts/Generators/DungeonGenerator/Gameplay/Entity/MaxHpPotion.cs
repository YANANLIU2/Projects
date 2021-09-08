using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHpPotion : Entity
{
    [Tooltip("The amount of the empowering power")]
    [SerializeField] int m_value = 5;
    protected override void Interact(PlayerController player)
    {
        // Pick up the coin 
        player.MaxHp += m_value;
        // Destroy itself
        Clear();
    }
}
