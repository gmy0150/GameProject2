using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureInteract : MonoBehaviour
{

    public Material BaseMesh;
    public Material invMesh;
    public void ShowShape()
    {

        GetComponentInChildren<MeshRenderer>().material = BaseMesh;

    }

    public void HideShape()
    {

        GetComponentInChildren<MeshRenderer>().material = invMesh;

    }
    void Awake()
    {

        BaseMesh = GetComponentInChildren<MeshRenderer>().material;
        invMesh = Resources.Load<Material>("Material/invisible");

        HideShape();
    }
}
