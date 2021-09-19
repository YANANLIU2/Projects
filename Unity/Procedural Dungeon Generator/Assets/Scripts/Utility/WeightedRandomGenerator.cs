using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A weighted random generator 
// Reference: https://gamedev.stackexchange.com/questions/162976/how-do-i-create-a-weighted-collection-and-then-pick-a-random-element-from-it
// I changed it from generic to using integar to represent an item to adapt to the project 
// Also I use binary search for finding a random entry instead of looping all entries
public class WeightedRandomGenerator
{
    // There's no valid entry 
    public readonly static int s_invalidEntry = -1;
    private class Entry
    {
        // The accumulated weight of all the previous entries and itself 
        float m_accumulatedWeight;

        // The id of the item it represents
        int m_id;

        // Getters & Setters
        public float AccumulatedWeight { get => m_accumulatedWeight; set => m_accumulatedWeight = value; }
        public int Id { get => m_id; }

        // Constructor
        public Entry(float weight, int id)
        {
            m_accumulatedWeight = weight;
            m_id = id;
        }
    }
    // The accumulated weight of all the entries
    private float m_totalWeight;
    // A list of all entries
    private List<Entry> m_entries;
    // A random number generator 
    private XorshiftRNG m_rng;
    // Constructor
    public WeightedRandomGenerator()
    {
        m_rng = new XorshiftRNG();
        m_rng.Initialize();
        m_entries = new List<Entry>();
        m_totalWeight = 0;
    }

    // Add an entry
    public void AddEntry(int id, float weight)
    {
        m_totalWeight += weight;
        m_entries.Add(new Entry(m_totalWeight, id));
    }

    // Get a random entry's id
    public int GetRandom()
    {
        // Get a random value between 0 - total weight
        float randomValue =  m_rng.GetNorm() * m_totalWeight;
        return FindEntry(randomValue);
    }

    // Use binary search to find the random entry
    int FindEntry(float targetWeight)
    {
        int size = m_entries.Count;
        if(size <= 0)
        {
            return s_invalidEntry;
        }
        else if(size == 1)
        {
            return m_entries[0].Id;
        }
        int start = 0, end = size - 1;
        while(start + 1 < end)
        {
            int mid = (start + end) / 2;
            // If weight >= target and the preivous entry's weight < target unless there's no previous entry
            if(m_entries[mid].AccumulatedWeight >= targetWeight && (mid ==0 || m_entries[mid-1].AccumulatedWeight < targetWeight))
            {
                return m_entries[mid].Id;
            }
            // Search in the later half
            else if(m_entries[mid].AccumulatedWeight < targetWeight)
            {
                start = mid + 1;
            }
            // Search in the first half
            else
            {
                end = mid - 1;
            }
        }
        return m_entries[start].AccumulatedWeight >= targetWeight ? m_entries[start].Id : m_entries[end].Id;
    }

    // Reset the weighted random generator
    public void Reset()
    {
        // Reset the entry list
        m_entries.Clear();
        // Reset the total weight
        m_totalWeight = 0;
    }
}
