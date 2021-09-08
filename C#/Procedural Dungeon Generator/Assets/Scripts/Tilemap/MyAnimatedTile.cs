using UnityEngine.Tilemaps;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MyAnimatedTile : AnimatedTile
{
    public GameObject m_gameObject;

#if UNITY_EDITOR
    // The following is a helper that adds a menu item to create an ItemTile Asset
    [MenuItem("Assets/Create/MyAnimatedTile")]
    public static void CreateItemTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Tile", "New Tile", "Asset", "Save Tile", "Assets");
        if (path == "")
        {
            return;
        }
        AssetDatabase.CreateAsset(CreateInstance<MyAnimatedTile>(), path);
    }
#endif

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        base.StartUp(position, tilemap, go);
        if(m_gameObject)
        {
            Instantiate(m_gameObject, position, Quaternion.identity);
        }
        return true;
    }
}
