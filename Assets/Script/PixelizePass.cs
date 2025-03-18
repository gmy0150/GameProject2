using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PixelizePass : ScriptableRenderPass
{
    PixelizeFeature.CustomPassSettings settings;
    RenderTargetIdentifier colorBuffer, pixelBuffer;//카메라 색상 텍스쳐, 픽셀화 버퍼 입력
    int pixelBufferID = Shader.PropertyToID("_PixelBuffer");//쉐이더 아이디로 불러옴

    Material material;
    int pixelScreenHeight, pixelScreenWidth;
    public PixelizePass(PixelizeFeature.CustomPassSettings settings)//
    {
        this.settings = settings;
        this.renderPassEvent = settings.renderPassEvent;
        if (material == null) material = CoreUtils.CreateEngineMaterial("Hidden/Pixelize");
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        throw new System.NotImplementedException();
    }


}
