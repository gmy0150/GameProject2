using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Playables;

public class TutoCloset : Closet
{
    public PlayableDirector director;
    public override void Interact(Player character, IController controller)
    {
            base.Interact(character, controller);
        InventoryManager.Instance.ExitSlot();

        if (!isHide)
        {
            Hide();
        }
        GameManager.Instance.ActPlay(false);
        if(!first)
        director.Play();
    }
    bool first = false;
    void Update()
    {
        if (isHide && director.state != PlayState.Playing)
        {
            GameManager.Instance.ActPlay(true);
            first = true;
        }

    }
    public override void InteractAgain()
    {
        if (isHide && GameManager.Instance.CanPlayerMove())
        {
            Hide();
            // 수정
            
        }
    }
}


