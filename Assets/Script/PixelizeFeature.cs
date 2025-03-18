using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PixelizeFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class CustomPassSettings
    {
        public RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        public int screenHeight = 144;//최종 렌더링 이미지의 픽셀화된 해상도를 계산하는데 사용할 특정 화면의 높이
    }
    [SerializeField]private CustomPassSettings settings;
    private PixelizePass customPass;




    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
#if UNITY_EDITOR
        if (renderingData.cameraData.isSceneViewCamera) return;//씬뷰에서 보고 싶지않기 때문에 씬뷰에선 return하게 함
#endif
        renderer.EnqueuePass(customPass);//렌더링 대기열에 추가
    }

    public override void Create()
    {
        customPass = new PixelizePass(settings);//생성자 호출
    }


}
