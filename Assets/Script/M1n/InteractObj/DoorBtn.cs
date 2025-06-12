using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class DoorBtn : UseageInteract
{
    public GameObject AilenRoom;
    public override void Interact(Player character, IController controller)
    {
        if (!GameManager.Instance.AbleButton())
            return;
        base.Interact(character, controller);
        AilenRoom.SetActive(true);

        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.CompleteSecretButtonMission();
        }
        character.SetAudio("Button");
<<<<<<< Updated upstream
        
        
=======
        Debug.Log("작동함");
        director.Play();

>>>>>>> Stashed changes
    }
    void Update()
    {
        if (isHide && director.state != PlayState.Playing)
        {
            GameManager.Instance.ActPlay(true);
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