using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarEffect : MonoBehaviour
{
        public Material FogMaterial;
        public Player player; // Player�� ����

        void Start()
        {
            if (player == null)
            {
                Debug.LogError("FogOfWarEffect: Player�� �Ҵ���� �ʾҽ��ϴ�!");
            }
        }

        void Update()
        {
            if (FogMaterial == null || player == null) return;

            // �÷��̾� ��ġ ������Ʈ
            FogMaterial.SetVector("_ViewPosition", player.transform.position);

            // �÷��̾� �ֺ� ���� �þ� �ݰ�
            FogMaterial.SetFloat("_CircleRange", player.CircleRange);

            // ���� �þ� �ݰ� �� ����
            FogMaterial.SetFloat("_ViewRadius", player.detectionRange);
            FogMaterial.SetFloat("_ViewAngle", player.angleLimit);

            // �÷��̾��� ���� ������ ����
            FogMaterial.SetVector("_ViewDirection", player.transform.forward);
        }
    }
