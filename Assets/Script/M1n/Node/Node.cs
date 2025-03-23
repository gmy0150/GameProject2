using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public abstract class Node 
    {
        public enum NodeState
        {
            SUCCESS, FAILURE, RUNNING
        }
        public abstract NodeState Evaluate();
    }
}
