using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AilionAI : Enemy
{
    protected override void Update()
    {
        base.Update();
        if (GameManager.Instance.isGameOver) return;
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
    public Vector3 stapos;
    void Start()
    {
        base.Start();
        startPos = stapos;
    }
}
