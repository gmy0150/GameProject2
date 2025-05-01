
using UnityEngine; 

public class SceneMoveBtn : UseageInteract
{
    public FadeScript fadeScript;  // 인스펙터에 연결
    public string SceneName;
    public override void Interact(Player character, IController controller)
    {
        base.Interact(character, controller);
        Debug.Log("눌림");
        fadeScript.FadeToScene(SceneName);
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
