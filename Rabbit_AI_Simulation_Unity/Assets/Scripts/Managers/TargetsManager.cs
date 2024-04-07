using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Provide a list of all targets (except self-targets)
public class TargetsManager : MonoBehaviour
{
    // Singleton
    static TargetsManager instance_;
    public static TargetsManager Instance
    {
        get
        {
            if(instance_ == null)
            {
                instance_ = FindObjectOfType<TargetsManager>();
                if(instance_ == null)
                {
                    GameObject obj = new GameObject("TargetsManager");
                    instance_ = obj.AddComponent<TargetsManager>();
                }
            }
            return instance_;
        }
    }

    [SerializeField] private List<Target> targets_array_;

    // Getters & Setters
    public List<Target> TargetsArray { get => targets_array_; }
}

