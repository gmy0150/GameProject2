using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : Character
{
    public override void Action()
    {
        throw new System.NotImplementedException();
    }

    public override float ReturnSpeed()
    {
        return MoveSpeed;
    }
    [Range(0, 360)]
    public float angle = 90f;  // ��ä�� ����
    public float radius = 5f;   // ��ä�� ������
    [Range(2, 64)]
    public int segments = 16;   // ��ä�� ���� ��
    public Color fanColor = Color.gray; // ��ä�� ����

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    void Start()
    {
        fanRenderer = transform.GetChild(0).GetComponent<TestOne>();
    }


    void Update()
    {
        
    }

    

    // ����� �׸��� (���� ����)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);

        Vector3 left = Quaternion.Euler(0, -angle / 2, 0) * transform.forward;
        Vector3 right = Quaternion.Euler(0, angle / 2, 0) * transform.forward;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + left * radius);
        Gizmos.DrawLine(transform.position, transform.position + right * radius);
    }
    

    INode SettingBT()
    {
        return new SelectorNode(new List<INode>()
        {
            new SequenceNode(new List<INode>()
            {
                new ActionNode(CheckDetectPlayer),
                new ActionNode(ChasePlayer),
            }),
            new SequenceNode(new List<INode>()
            {
                new ActionNode(FollowPath),
                new ActionNode(WaitAtPoint)
            })
        });
    }

    private INode.ENodeState WaitAtPoint()
    {
        throw new NotImplementedException();
    }

    private INode.ENodeState FollowPath()
    {
        throw new NotImplementedException();
    }

    private INode.ENodeState ChasePlayer()
    {
        throw new NotImplementedException();
    }

    private INode.ENodeState CheckDetectPlayer()
    {
        Transform enemyTrnas = this.transform;

        GameObject player = GameObject.FindAnyObjectByType<Player>().gameObject;
        if (player == null) return INode.ENodeState.ENS_Failure;

        Vector3 directionToPlayer = (player.transform.position - enemyTrnas.position).normalized;
        float distanceTOPlayer = Vector3.Distance(enemyTrnas.position, player.transform.position);

        if(distanceTOPlayer > radius)
            return INode.ENodeState.ENS_Failure;

        float angle = Vector3.Angle(enemyTrnas.forward, directionToPlayer);
        if ((angle > angle / 2f))
            return INode.ENodeState.ENS_Failure;

        RaycastHit hit;
        if (Physics.Raycast(enemyTrnas.position, directionToPlayer, out hit, radius))
        {
            if (hit.collider.GetComponent<Player>())
            {
                return INode.ENodeState.ENS_Success;
            }
            else
            {
                return INode.ENodeState.ENS_Failure;
            }
        }
        return INode.ENodeState.ENS_Failure;
    }
    public TestOne fanRenderer; // FanRenderer ��ũ��Ʈ�� ���� ����

    // �þ� �˻� ����� ������ ����ü


    public struct VisibilityResult
    {
        public List<Vector3> visiblePoints; // ���̴� ���� ���
        public List<Vector3> blockedPoints; // ���� ���� ���
    }


    // �þ� �˻� �Լ�
    public VisibilityResult CheckVisibility(int rayCount)
    {
        Transform enemyTransform = transform;
        VisibilityResult result = new VisibilityResult();
        result.visiblePoints = new List<Vector3>();
        result.blockedPoints = new List<Vector3>();

        GameObject player = GameObject.FindGameObjectWithTag("Player"); // tag�� ã�°� ����.
        if (player == null)
        {
            return result; // �÷��̾ ������ �� ��� ��ȯ
        }


        // ��ä�� ������ ���� ���� Raycast�� ��
        for (int i = 0; i <= rayCount; i++)
        {
            float currentAngle = -fanRenderer.angle / 2 + fanRenderer.angle * (i / (float)rayCount);
            Quaternion rotation = Quaternion.Euler(0, currentAngle, 0);
            Vector3 rayDirection = rotation * enemyTransform.forward;

            // ������� ���� Ray �׸��� (Scene �信�� Ȯ��)
            Debug.DrawRay(enemyTransform.position, rayDirection * fanRenderer.radius, Color.yellow);


            RaycastHit hit;

            // Raycast ����
            if (Physics.Raycast(enemyTransform.position, rayDirection, out hit, fanRenderer.radius))
            {
                // Raycast�� Player�� �¾����� ���̴� ����
                if (hit.collider.CompareTag("Player"))
                {
                    result.visiblePoints.Add(hit.point);
                    //Debug.Log("visible");
                }
                // �ٸ� ��ü�� �¾����� ���� ����
                else
                {
                    result.blockedPoints.Add(hit.point);
                    //Debug.Log("blocked");
                }
            }
            else // Ray�� �ƹ��Ϳ��� ���� �ʾҴٸ�, �ִ� �Ÿ� ������ ��� (��ä�� ���)
            {

                result.visiblePoints.Add(enemyTransform.position + rayDirection * fanRenderer.radius);

            }
        }

        return result; // ���̴�/���� ���� ��� ��ȯ
    }
}
