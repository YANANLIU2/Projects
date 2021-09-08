using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class MyTile : Tile
{
#if UNITY_EDITOR
    // The following is a helper that adds a menu item to create an ItemTile Asset
    [MenuItem("Assets/Create/MyTile")]
    public static void CreateItemTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Tile", "New Tile", "Asset", "Save Tile", "Assets");
        if (path == "")
        {
            return;
        }
        AssetDatabase.CreateAsset(CreateInstance<MyTile>(), path);
    }
#endif
}