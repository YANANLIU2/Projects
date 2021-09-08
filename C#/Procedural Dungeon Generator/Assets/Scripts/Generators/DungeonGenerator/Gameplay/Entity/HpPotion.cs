using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpPotion : Entity
{
    [Tooltip("The amount of the healing power")]
    [SerializeField] int m_value = 5;
    protected override void Interact(PlayerController player)
    {
        // Pick up the coin 
        player.Hp += m_value;
        // Destroy itself
        Clear();
    }
}
