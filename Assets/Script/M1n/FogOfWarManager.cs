using UnityEngine;

public class FogOfWarManager : MonoBehaviour
{
    public Shader fogOfWarShader;  // 사용자 정의 쉐이더
    private Material fogMaterial;  // 생성된 Material
    public Transform player;       // 플레이어 오브젝트
    public float viewRange = 10.0f; // 시야 범위
    public Texture fogTexture;    // Fog of War 텍스처

    void Start()
    {
        // 씬의 모든 Renderer를 찾아서 쉐이더를 적용
        Renderer[] renderers = FindObjectsOfType<Renderer>();

        foreach (var renderer in renderers)
        {
            // 각 오브젝트에 Material 적용
            Material material = new Material(fogOfWarShader);
            renderer.material = material;

            // 텍스처와 시야 범위 설정
            material.SetTexture("_FogTex", fogTexture);
            material.SetFloat("_ViewRange", viewRange);
        }
    }

    void Update()
    {
        // 실시간으로 플레이어 위치 업데이트
        foreach (var renderer in FindObjectsOfType<Renderer>())
        {
            Material material = renderer.material;
            material.SetVector("_ViewPosition", player.position);
        }
    }
}
