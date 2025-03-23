using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTV : Enemy
{
    Node CCTVNode;
    void Start()
    {

        CCTVNode = new Selector(new List<Node>()
        {
            new Sequence(new List<Node>()
            {
                new CheckPlayerInSight(this),
                new ifCheckPlayer(this),
            } ),
            new CCTVMove(this),
        });
    }

    // Update is called once per frame
    void Update()
    {
        CCTVNode.Evaluate();
    }
}
