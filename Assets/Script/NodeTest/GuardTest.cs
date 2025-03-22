using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardTest : MonoBehaviour
{
    private TestNode behaviorTree;

    private void Start()
    {
        // Behavior Tree 구성
        behaviorTree = new TestSelector(new List<TestNode>
        {
            new TestSequence(new List<TestNode>
            {
                new TCheckPlayer(this),
                new TChasePlayer(this)
            }),
            new TestSequence(new List<TestNode>
            {
                new TPatrol(this),
                new TWaitSecond(this, 3f)  // 3초 동안 기다림
            })
        });
    }
    // Update is called once per frame
    void Update()
    {
        behaviorTree.Evaluate();
    }
    public bool IsPlayerInSight()
    {
        // 플레이어가 시야에 있으면 true 반환 (예시)
        return false;  // 플레이어가 보이지 않으면 실패
    }

    public void ChasePlayer()
    {
        // 플레이어 추적 로직 구현
        Debug.Log("플레이어를 추적합니다.");
    }

    public void Patrol()
    {
        // 순찰 로직 구현
        Debug.Log("순찰 중...");
    }
}
