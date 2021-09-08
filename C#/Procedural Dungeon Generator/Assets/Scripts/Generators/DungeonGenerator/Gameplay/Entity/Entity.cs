using UnityEngine;
using UnityEngine.Tilemaps;

// A base class for entities, including items and monsters
public abstract class Entity : MonoBehaviour
{
    static string s_playerTag = "Player";

    // What will happen when the player enters the tile 
    protected abstract void Interact(PlayerController player);

    // Trigger enter
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If it's player
        if(collision.CompareTag(s_playerTag))
        {
            // Get player component
            PlayerController player = collision.GetComponent<PlayerController>();
            if(player)
            {
                // Call class-defined function
                Interact(player);
            }
        }
    }

    protected void Clear()
    {
        TilemapManager.Instance.SetTile(new Vector3Int((int)transform.position.x, (int)transform.position.y, 0), null, DungeonGenerator.kEntityLayer);
        Destroy(this);
    }
}
