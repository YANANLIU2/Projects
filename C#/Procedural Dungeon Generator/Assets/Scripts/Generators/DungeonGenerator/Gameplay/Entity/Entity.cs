using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
#if DEBUG
    [SerializeField] protected string m_debugMsg = "You opened a box";
#endif
    protected bool IsPlayer(Collider2D collision)
    {
        return collision.CompareTag(DungeonGenerator.s_playerTag);
    }

    public void Clear()
    {
        Destroy(gameObject);
    }
}
