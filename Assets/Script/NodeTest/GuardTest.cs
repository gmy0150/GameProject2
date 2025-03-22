using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardTest : MonoBehaviour
{
    private TestNode behaviorTree;

    private void Start()
    {
        // Behavior Tree ����
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
                new TWaitSecond(this, 3f)  // 3�� ���� ��ٸ�
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
        // �÷��̾ �þ߿� ������ true ��ȯ (����)
        return false;  // �÷��̾ ������ ������ ����
    }

    public void ChasePlayer()
    {
        // �÷��̾� ���� ���� ����
        Debug.Log("�÷��̾ �����մϴ�.");
    }

    public void Patrol()
    {
        // ���� ���� ����
        Debug.Log("���� ��...");
    }
}
