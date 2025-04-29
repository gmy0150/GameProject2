using System.Collections;
using System.Collections.Generic;
using Pathfinding.Examples;
using UnityEngine;

public class ComputerBtn : UseageInteract
{
    public GameObject go;
    public override void Interact(Player character, IController controller)
    {
        if(GameManager.Instance.AbleComputer()){
            base.Interact(character, controller);
            go.gameObject.SetActive(false);
            InteractAgain();
        }
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
