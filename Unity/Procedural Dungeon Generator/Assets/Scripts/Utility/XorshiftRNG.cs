using UnityEngine;
using UnityEngine.Assertions;

// A random number generator based on xorshift algorithm
// Algorithm Reference: https://en.wikipedia.org/wiki/Xorshift
[System.Serializable]
public class XorshiftRNG
{
    [Header("Seeding")]
    [Tooltip("If true we will get a random seed from Unity")]
    [SerializeField] private bool m_isUsingRandomSeed = true;

    [Tooltip("Seed")]
    [SerializeField] private Xorshift32State m_state = new Xorshift32State();

    static private readonly string s_lowHighErrorMessage = "The high value must be larger than or equal to the low value";
    static private readonly string s_zeroSeedErrorMessage = "The seed cannot be zero";
    // A wrapper class for the current value from the number sequence 
    [System.Serializable]
    private class Xorshift32State
    {
        [Tooltip("Seed")]
        [SerializeField] public uint m_value = 42;
    }

    // Seeding
    public void Initialize()
    {
        if (m_isUsingRandomSeed)
        {
            m_state.m_value = (uint)Random.Range(1, uint.MaxValue);
        }
    }

    // Getter & Setter for the state
    public uint Seed
    {
        get 
        { 
            return m_state.m_value; 
        }
        set
        {
#if DEBUG
            Assert.IsTrue(value != 0, s_zeroSeedErrorMessage);
#endif
            if (value != 0)
            {
                m_state.m_value = value;
            }
        }
    }

    // Get the next number from the sequence
    private uint Next()
    {
        uint x = m_state.m_value;
        x ^= x << 13;
        x ^= x >> 17;
        x ^= x << 5;
        return m_state.m_value = x;
    }

    // Get a random number as uint
    public uint GetUInt()
    {
        Next();
        return m_state.m_value;
    }

    // Get a random number as int
    public int GetInt()
    {
        Next();
        return (int)(m_state.m_value % int.MaxValue);
    }

    // Get a random number between low value and high value (both inclusive) as int 
    public int GetRange(int low, int high)
    {
#if DEBUG
        Assert.IsTrue(high >= low, s_lowHighErrorMessage);
#endif
        int randomValue = GetInt();
        return randomValue % (high + 1 - low) + low;
    }

    // Get a random number between low value and high value (both inclusive) as uint 
    public uint GetRange(uint low, uint high)
    {
#if DEBUG
        Assert.IsTrue(high >= low, s_lowHighErrorMessage);
#endif
        uint randomValue = GetUInt();
        return randomValue % (high + 1 - low) + low;
    }

    // Get a random float between 0 - 1
    public float GetNorm()
    {
        Next();
        return (float)m_state.m_value / (float)uint.MaxValue;
    }

    // Get a random number between low value and high value (both inclusive) as float 
    public float GetRange(float low, float high)
    {
#if DEBUG
        Assert.IsTrue(high >= low, s_lowHighErrorMessage);
#endif
        float delta = high - low;
        float value = GetNorm() * delta;
        return value + low;
    }

    // Get a random boolean
    public bool GetBool()
    {
        Next();
        return m_state.m_value % 2 == 0;
    }
}

