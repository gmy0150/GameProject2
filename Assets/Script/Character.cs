using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public float MoveSpeed;

    public abstract void Action();

    public abstract float ReturnSpeed();

}
