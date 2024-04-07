using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Convey information between actions. For now, it will just provide the agent reference
public class Context
{
    private Agent agent_;
    public Agent Agent { get => agent_;} 
    public Context(Agent agent) { agent_ = agent; }
}
