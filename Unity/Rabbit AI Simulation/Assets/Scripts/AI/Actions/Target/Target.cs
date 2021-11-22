using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] protected List<ActionScriptableObject > actions_array;
    public List<ActionScriptableObject > ActionsArray { get => actions_array; }
}
