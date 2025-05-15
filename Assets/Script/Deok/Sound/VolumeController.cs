using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeController : MonoBehaviour
{
    [Header("볼륨 유닛 연결")]
    public VolumeControlUnit masterVolumeUnit;
    public VolumeControlUnit bgmVolumeUnit;
    public VolumeControlUnit sfxVolumeUnit;
    public VolumeControlUnit uiSfxVolumeUnit;

    private void Awake()
    {
        if (VolumeManager.Instance != null)
        {
            Debug.Log($"[VolumeController] {gameObject.scene.name} - VolumeManager에 BindUI 연결");
            VolumeManager.Instance.BindUI(masterVolumeUnit, bgmVolumeUnit, sfxVolumeUnit, uiSfxVolumeUnit);
        }
        else
        {
            Debug.LogError("[VolumeController] VolumeManager 인스턴스를 찾을 수 없습니다.");
            //체크
        }
    }
}