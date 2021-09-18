using UnityEngine;
using System.Collections.Generic;
public abstract class NeighborMode 
{
    public enum EType
    {
        kFourNeighbors,
        kEightNeighbors,
        kNum
    }
    protected  List<Vector3Int> m_neighbors;
    public int Count { get => m_neighbors.Count; }
    public Vector3Int this[int key] { get => m_neighbors[key]; }
}