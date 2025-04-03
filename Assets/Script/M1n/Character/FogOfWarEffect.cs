using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarEffect : MonoBehaviour
{
        public Material FogMaterial;
        public Player player; // Player를 참조

        void Start()
        {
            if (player == null)
            {
                Debug.LogError("FogOfWarEffect: Player가 할당되지 않았습니다!");
            }
        }

        void Update()
        {
            if (FogMaterial == null || player == null) return;

            // 플레이어 위치 업데이트
            FogMaterial.SetVector("_ViewPosition", player.transform.position);

            // 플레이어 주변 원형 시야 반경
            FogMaterial.SetFloat("_CircleRange", player.CircleRange);

            // 전방 시야 반경 및 각도
            FogMaterial.SetFloat("_ViewRadius", player.detectionRange);
            FogMaterial.SetFloat("_ViewAngle", player.angleLimit);

            // 플레이어의 전방 방향을 전달
            FogMaterial.SetVector("_ViewDirection", player.transform.forward);
        }
    }
