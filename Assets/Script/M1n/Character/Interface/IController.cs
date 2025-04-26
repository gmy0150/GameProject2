using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IController
{
    public void OnPosessed(Player controllerableCharacter);
    public void Tick(float deltaTime);
    public void SetHide(bool x);
    public void SetNoise(bool x);
    public void LateTick(float deltaTime);
    void RunningCancel();
}
