using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public Material invMesh;
    public Material BaseMesh;

    public override void Action()
    {
        throw new System.NotImplementedException();
    }

    public override float ReturnSpeed()
    {
        throw new System.NotImplementedException();
    }

    public virtual void ProbArea(Vector3 pos) { }
    public virtual void ShowOutline(){}
    public virtual void HideOutline(){}
    public virtual void ShowShape() {
        GetComponent<MeshRenderer>().material = BaseMesh;
        if (GetComponentInChildren<TestOne>())
        {
            TestOne t1 = GetComponentInChildren<TestOne>();
            t1.ShowMesh();
        }
    }

    public virtual void HideShape(){ 
        
        GetComponent<MeshRenderer>().material = invMesh;
        if (GetComponentInChildren<TestOne>())
        {
            TestOne t1 = GetComponentInChildren<TestOne>();
            t1.InvMeshRen();
        }
    }
}
