using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TestNode 
{
    public enum NodeState
    {
        SUCCESS, FAILURE, RUNNING
    }
    public abstract NodeState Evaluate();
}
