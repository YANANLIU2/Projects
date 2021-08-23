using UnityEngine;
using System.Collections.Generic;
public abstract class NeighborModeBase 
{
    public enum ENeighborModeType
    {
        kFourNeighbors,
        kEightNeighbors,
        kNum
    }
    protected  List<Vector3Int> m_neighbors;
    public int Count { get => m_neighbors.Count; }
    public Vector3Int this[int key] { get => m_neighbors[key]; }
    static public NeighborModeBase CreateMode(int typeIndex)
    {
        ENeighborModeType type = (ENeighborModeType)typeIndex;
        switch (type)
        {
            case ENeighborModeType.kFourNeighbors:
                return new FourNeighborMode();
            case ENeighborModeType.kEightNeighbors:
                return new EightNeighborMode();
            case ENeighborModeType.kNum:
                break;
            default:
                break;
        }
        return null;
    }
}