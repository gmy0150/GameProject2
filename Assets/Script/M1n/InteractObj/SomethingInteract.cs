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
    public override void Interact(Player character, IController controller)
    {
        base.Interact(character, controller);
        ailion.StartChase(character);
    }
}
