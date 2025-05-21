
using UnityEngine; 

public class SceneMoveBtn : UseageInteract
{

    public override void Interact(Player character, IController controller)
    {
        base.Interact(character, controller);
        Debug.Log("눌림");
        GameObject prefab = Resources.Load<GameObject>("Prefabs/Second_Scene");
        if(prefab != null){
            Instantiate(prefab);
        }
    }
    public override bool IsOneTimeInteraction()
    {
        return true;
    }
    public override void UpdateTime(float time)
    {
        throw new System.NotImplementedException();
    }

}
