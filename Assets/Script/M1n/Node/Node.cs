using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public abstract class Node :ScriptableObject
    {
        public abstract Node Clone();
        public enum NodeState
        {
            SUCCESS, FAILURE, RUNNING
        }
        protected Enemy runner;
        public virtual void SetRunner(Enemy runner)
        {
            this.runner = runner;
            if (this is CompositeNode compositeNode)
            {
                foreach (var child in compositeNode.nodes)
                {
                    child.SetRunner(runner);
                }
            }
        }
        public void NewNode()
        {
            Node game = Instantiate(this);
            Debug.Log(game.GetType());
        }

        public abstract NodeState Evaluate();
    }
}
