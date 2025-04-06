using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarEffect : MonoBehaviour
{
        public Material FogMaterial;
        public Player player; // Player 

        void Start()
        {
            if (player == null)
            {
                Debug.LogError("FogOfWarEffect: Player Ҵ ʾҽϴ!");
            }
        }

        void Update()
        {
            if (FogMaterial == null || player == null) return;

            //
            FogMaterial.SetVector("_ViewPosition", player.transform.position);

            // 
            FogMaterial.SetFloat("_CircleRange", player.CircleRange);

            // (변경된 프로퍼티 이름 사용)
            FogMaterial.SetFloat("_DetectionRange", player.detectionRange); // _ViewRadius -> _DetectionRange
            FogMaterial.SetFloat("_AngleLimit", player.angleLimit);         // _ViewAngle -> _AngleLimit

            // 
            FogMaterial.SetVector("_ViewDirection", player.transform.forward);
        }
    }
