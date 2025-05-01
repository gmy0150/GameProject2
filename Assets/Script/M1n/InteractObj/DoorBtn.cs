using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class DoorBtn : UseageInteract
{
    public GameObject AilenRoom;
    public override void Interact(Player character, IController controller)
    {
        if(!GameManager.Instance.AbleButton())
        return;
            base.Interact(character, controller);
            AilenRoom.SetActive(true);
            Debug.Log("버튼눌림");
        
    }
    public override void InteractAgain()
    {
        enabled = false;
    }
    public override bool IsOneTimeInteraction()
    {
        return true;
    } 
    public override void UpdateTime(float time)
    {
        
    }
}
