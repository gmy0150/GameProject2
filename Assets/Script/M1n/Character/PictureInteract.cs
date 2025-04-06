using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureInteract : MonoBehaviour, IShapeToggle
{

    public Material BaseMesh;
    public Material invMesh;
    public void ShowShape()
    {

        GetComponentInChildren<MeshRenderer>().material = BaseMesh;
                Debug.Log("?111??");

    }

    public void HideShape()
    {

        GetComponentInChildren<MeshRenderer>().material = invMesh;
                Debug.Log("?222??");

    }
    void Awake()
    {

        BaseMesh = GetComponentInChildren<MeshRenderer>().material;

                Debug.Log("?2222222222222??");
        invMesh = Resources.Load<Material>("Material/invisible");
    }
}
