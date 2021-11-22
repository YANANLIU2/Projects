using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Behavior
{
    public enum EType
    {
        kPlayAnimation,
        kRestoreHungerPerSecond,
        kRestoreThirstPerSecond,
        kRestoreSleepPerSecond,
        kRestoreBladderPerSecond,
    }

    [SerializeField] private EType type_;
    [SerializeField] protected string raw_value_;

    // Getters & Setters
    public EType Type { get => type_; }

    // Factory method
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