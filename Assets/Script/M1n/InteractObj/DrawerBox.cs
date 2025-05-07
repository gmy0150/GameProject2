using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class DrawerBox : UseageInteract
{
    bool isopen;
    public GameObject GBox;
    
    float openPos;
    float closePos;
    [SerializeField] float openy;
    public override void Interact(Player character, IController controller)
    {
        base.Interact(character, controller);
        if (isopen) CloseBox();
        else OpenBox();
    }
    void OpenBox(){
        
        GBox.transform.DOLocalMoveY(openPos,0.5f);
        isopen = true;
    }
    void CloseBox(){
        

        GBox.transform.DOLocalMoveY(closePos,0.5f);
        isopen = false; 
    }
    public override void UpdateTime(float time)
    {
        
    }
    void Start()
    {
        closePos = GBox.transform.localPosition.y;
        openPos = closePos + openy;
    }   
     void Update()
    {

    }
}
