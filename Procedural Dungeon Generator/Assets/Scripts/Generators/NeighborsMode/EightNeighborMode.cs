
using UnityEngine;
using System.Collections.Generic;
class EightNeighborMode : NeighborModeBase
{
    public EightNeighborMode()
    {
        m_neighbors = new List<Vector3Int>
        {
            new Vector3Int(1, 0, 0), new Vector3Int(1, -1, 0), 
            new Vector3Int(1, 1, 0), new Vector3Int(0, -1, 0),
            new Vector3Int(0, 1, 0), new Vector3Int(-1, -1, 0),
            new Vector3Int(-1, 0, 0), new Vector3Int(-1, 1, 0)
        };
    }
}
