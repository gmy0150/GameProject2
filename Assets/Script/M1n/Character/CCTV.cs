using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTV : Enemy
{
    public Material BaseOneMesh;

    protected override void Start()
    {
        base.Start();
        if (node != null)
        {
            node.SetRunner(this);
        }
    }

    // Update is called once per frame
    protected override void Update()
    {

        node.Evaluate();
    }

}
