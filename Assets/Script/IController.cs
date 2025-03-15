using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IController
{
    public void OnPosessed(Character controllerableCharacter);
    public void Tick(float deltaTime);

}
