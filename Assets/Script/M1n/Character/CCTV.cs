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
    public override void ShowShape()
    {

        MeshRenderer[] mesh = GetComponentsInParent<MeshRenderer>();
        for (int i = 0; i < mesh.Length; i++)
        {
            mesh[1].material = BaseOneMesh;
            mesh[0].material =BaseMesh;

}
        if (GetComponentInChildren<TestOne>())
        {
            TestOne t1 = GetComponentInChildren<TestOne>();
            t1.ShowMesh();
        }
    }

    public override void HideShape()
    {
        Debug.Log("?>");
        MeshRenderer[] mesh = GetComponentsInParent<MeshRenderer>();
        for (int i = 0; i < mesh.Length; i++)
        {
            mesh[i].material = invMesh;
        }
        if (GetComponentInChildren<TestOne>())
        {
            TestOne t1 = GetComponentInChildren<TestOne>();
            t1.InvMeshRen();
        }
    }
}
