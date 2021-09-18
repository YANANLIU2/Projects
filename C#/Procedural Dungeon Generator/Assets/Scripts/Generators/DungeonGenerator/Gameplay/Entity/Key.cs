using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Entity
{
    public enum EKeyType
    {
        kSilver,
        kGold,
        kNum
    }

    [SerializeField] private EKeyType m_type;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If it is player 
        if (IsPlayer(collision))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player)
            {
                // Pick up the key
#if DEBUG
                Debug.Log(m_debugMsg);
#endif
                player.AddKey(m_type);
                Clear();
            }
        }
    }
}
