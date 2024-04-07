using UnityEngine;

// One behavior means a one - two execution lines. The designer can only set the type and raw value in the Unity editor.
// When the game begins, it will initialize the corresponding child behavior types and parse the raw values.
// One or multiple behaviors compose an action. 
[System.Serializable]
public class Behavior
{
    // When create a new child behavior, add its type here
    public enum EType
    {
        kPlayAnimation,
        kRestoreHungerPerSecond,
        kRestoreThirstPerSecond,
        kRestoreSleepPerSecond,
        kRestoreBladderPerSecond,
    }

    [Tooltip("The desired type of the behavior")]
    [SerializeField] private EType type_;

    [Tooltip("The desired value of the behavior")]
    [SerializeField] protected string raw_value_; // 

    // Getters & Setters
    public EType Type { get => type_; }

    // Factory method: create the real bahavior types by calling this at the beginning of the game
    public static Behavior Create(Behavior holder)
    {
        switch (holder.Type)
        {
            case EType.kPlayAnimation:
                return new PlayAnimation(holder.raw_value_);

            case EType.kRestoreHungerPerSecond:
                return new RestoreHungerPerSecond(holder.raw_value_);

            case EType.kRestoreThirstPerSecond:
                return new RestoreThirstPerSecond(holder.raw_value_);

            case EType.kRestoreSleepPerSecond:
                return new RestoreSleepPerSecond(holder.raw_value_);

            case EType.kRestoreBladderPerSecond:
                return new RestoreBladderPerSecond(holder.raw_value_);

            default:
                break;
        }
        return null;
    }

    // Constructor
    public Behavior(string raw_value) { raw_value_ = raw_value; }

    // To be Implemented in children
    public virtual void Execute(Agent agent) { }
}