using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class DoorBtn : UseageInteract
{
    public GameObject AilenRoom;
    public GameObject Wall;
    public override void Interact(Player character, IController controller)
    {
        if (!GameManager.Instance.AbleButton())
            return;
        base.Interact(character, controller);
        AilenRoom.SetActive(true);
        Wall.SetActive(false);

        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.CompleteSecretButtonMission();
        }
        character.SetAudio("Button");
        
        
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