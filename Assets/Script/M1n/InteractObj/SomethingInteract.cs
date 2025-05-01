using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SomethingInteract : UseageInteract
{
    public Enemy ailion;
    public override void UpdateTime(float time)
    {
        throw new System.NotImplementedException();
    }
    public override bool IsOneTimeInteraction()
    {
        return true;
    }
    void Update()
    {
        ailion.StartChase(character);
        
    }
    bool isAction;
    public override void Interact(Player character, IController controller)
    {
        base.Interact(character, controller);
        isAction = true;
    }
}
