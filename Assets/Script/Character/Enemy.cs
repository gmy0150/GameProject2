using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public override void Action()
    {
        throw new System.NotImplementedException();
    }

    public override float ReturnSpeed()
    {
        throw new System.NotImplementedException();
    }

    public virtual void ProbArea(Vector3 pos) { }

}
