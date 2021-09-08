using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// The script is created for handling UI in the "GraphViewer" scene
public class GraphViewerUIManager : MonoBehaviour
{
    [Tooltip("The dungeon map generator")]
    [SerializeField] private MapGenerator m_mapGenerator = null;

    private Simple2DCam m_camera = null;

    void Start()
    {
        // Dungeon Map Generator
        if(m_mapGenerator == null)
        {
#if DEBUG
            Debug.LogError("The reference of the dungeon generator cannot be null");
#endif
            return;
        }
        // Camera
        m_camera = Camera.main.GetComponent<Simple2DCam>();
        if (m_camera == null)
        {
#if DEBUG
            Debug.LogError("Initialization of the Simple2DCam failed");
#endif
            return;
        }
        m_mapGenerator.Generate();
    }
    
    // Reset the camera and generate a dungeon map
    public void Generate()
    {
        m_camera.Reset();
        m_mapGenerator.Generate();
    }

    public void SetNeighborsMode(int value)
    {
        m_mapGenerator.NeighborMode = value;
    }

}
