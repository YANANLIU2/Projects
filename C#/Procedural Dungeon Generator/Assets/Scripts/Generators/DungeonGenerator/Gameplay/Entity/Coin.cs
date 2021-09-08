using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Entity
{
    protected override void Interact(PlayerController player)
    {
        // Pick up the coin 
        player.Coins++;
        // Destroy itself
        Clear();
    }
}
