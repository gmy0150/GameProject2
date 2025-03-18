using UnityEngine;

public class FogOfWarManager : MonoBehaviour
{
    public Shader fogOfWarShader;  // ����� ���� ���̴�
    private Material fogMaterial;  // ������ Material
    public Transform player;       // �÷��̾� ������Ʈ
    public float viewRange = 10.0f; // �þ� ����
    public Texture fogTexture;    // Fog of War �ؽ�ó

    void Start()
    {
        // ���� ��� Renderer�� ã�Ƽ� ���̴��� ����
        Renderer[] renderers = FindObjectsOfType<Renderer>();

        foreach (var renderer in renderers)
        {
            // �� ������Ʈ�� Material ����
            Material material = new Material(fogOfWarShader);
            renderer.material = material;

            // �ؽ�ó�� �þ� ���� ����
            material.SetTexture("_FogTex", fogTexture);
            material.SetFloat("_ViewRange", viewRange);
        }
    }

    void Update()
    {
        // �ǽð����� �÷��̾� ��ġ ������Ʈ
        foreach (var renderer in FindObjectsOfType<Renderer>())
        {
            Material material = renderer.material;
            material.SetVector("_ViewPosition", player.position);
        }
    }
}
