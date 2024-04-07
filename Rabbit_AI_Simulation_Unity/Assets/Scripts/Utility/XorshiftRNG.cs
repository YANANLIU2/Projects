using UnityEngine;
using UnityEngine.Assertions;

// A random number generator based on xorshift algorithm
[System.Serializable]
public class XorshiftRNG
{
    [Header("Seed")]
    [Tooltip("Set true to use a random seed")]
    [SerializeField] private bool b_is_using_random_seed_ = true;

    [SerializeField] private Xorshift32State state_ = new Xorshift32State();

    static private readonly string INVALID_RANDOM_RANGE = "The high value cannot be less than the low value";
    static private readonly string ZERO_SEED_ERROR_MSG = "The seed cannot be zero";

    // A wrapper class for the current value from the number sequence 
    [System.Serializable]
    private class Xorshift32State { [SerializeField] public uint value_ = 42; }

    // Seeding
    public void Initialize() { if (b_is_using_random_seed_) { state_.value_ = (uint)Random.Range(1, uint.MaxValue); } }

    // Getter & Setter for the state
    public uint Seed
    {
        get { return state_.value_; }
        set
        {
#if DEBUG
            Assert.IsTrue(value != 0, ZERO_SEED_ERROR_MSG);
#endif
            if (value != 0) { state_.value_ = value; }
        }
    }

    // Get the next number from the sequence
    private uint Next()
    {
        uint x = state_.value_;
        x ^= x << 13;
        x ^= x >> 17;
        x ^= x << 5;
        return state_.value_ = x;
    }

    // Get a random number as uint
    public uint GetUInt()
    {
        Next();
        return state_.value_;
    }

    // Get a random number as int
    public int GetInt()
    {
        Next();
        return (int)(state_.value_ % int.MaxValue);
    }

    // Get a random number between low value and high value (both inclusive) as int 
    public int GetRange(int low, int high)
    {
#if DEBUG
        Assert.IsTrue(high >= low, INVALID_RANDOM_RANGE);
#endif
        int value = GetInt();
        return value % (high + 1 - low) + low;
    }

    // Get a random number between low value and high value (both inclusive) as uint 
    public uint GetRange(uint low, uint high)
    {
#if DEBUG
        Assert.IsTrue(high >= low, INVALID_RANDOM_RANGE);
#endif
        uint value = GetUInt();
        return value % (high + 1 - low) + low;
    }

    // Get a random float between 0 - 1
    public float GetNorm()
    {
        Next();
        return (float)state_.value_ / uint.MaxValue;
    }

    // Get a random number between low value and high value (both inclusive) as float 
    public float GetRange(float low, float high)
    {
#if DEBUG
        Assert.IsTrue(high >= low, INVALID_RANDOM_RANGE);
#endif
        float delta = high - low;
        float value = GetNorm() * delta;
        return value + low;
    }

    // Get a random boolean
    public bool GetBool()
    {
        Next();
        return state_.value_ % 2 == 0;
    }
}

