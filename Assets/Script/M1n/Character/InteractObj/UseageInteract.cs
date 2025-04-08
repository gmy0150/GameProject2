using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseageInteract : MonoBehaviour, IInterActerable,IShapeToggle
{
    protected bool isHide = false;
    protected Player character;
    protected IController controller;
    public bool shoot = false;
    public bool hasCoin = false;
    public Material BaseMesh;
    public Material invMesh;
    void Awake()
    {
        if(GetComponent<MeshRenderer>()){
        BaseMesh = GetComponent<MeshRenderer>().material;

        }else{
            BaseMesh = GetComponentInChildren<MeshRenderer>().material;
        }
        invMesh = Resources.Load<Material>("Material/invisible");
    }
    void Start()
    {
        
    }
    public bool isShoot()
    {
        return shoot;
    }

    public bool isCoin()
    {
        return hasCoin;
    }
    public bool GetHide()
    {
        return this.isHide;
    }

    public virtual void Interact(Player character, IController controller)
    {
        this.character = character;
        this.controller = controller;
    }

    public virtual void InteractAgain()
    {
        throw new System.NotImplementedException();
    }

    public void ShowShape()
    {
        if(GetComponent<MeshRenderer>()){
        GetComponent<MeshRenderer>().material = BaseMesh;

        }else{
        GetComponentInChildren<MeshRenderer>().material = BaseMesh;
        }
    }

    public void HideShape()
    {
        if(GetComponent<MeshRenderer>()){
        GetComponent<MeshRenderer>().material = invMesh;

        }else{
        GetComponentInChildren<MeshRenderer>().material = invMesh;
        }
    }
}
