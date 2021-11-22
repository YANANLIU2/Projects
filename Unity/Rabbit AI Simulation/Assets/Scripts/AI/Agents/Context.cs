using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Context
{
    private Agent agent_;
    public Agent Agent { get => agent_;} 
    public Context(Agent agent) { agent_ = agent; }
}
