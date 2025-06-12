using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AilionAI : Enemy
{
    protected override void Update()
    {
        base.Update();
        if (goChase)
            StartChase(Player);
    }
    bool goChase;
    Player Player;
    public void ChaseStart(Player player)
    {
        goChase = true;
        this.Player = player;
    }
    public override void missPlayer()
    {

    }
    void Start()
    {
        base.Start();
        startPos = transform.position;
        AstarPath.active.Scan();

    }

}
