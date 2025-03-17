using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarController : MonoBehaviour
{
    public Shader fogOfWarShader;
    Material fogMaterial;
        public Transform player;
    public float viewRange = 10;
    public Texture fogTexture;
    private void Start()
    {
        fogMaterial = new Material(fogOfWarShader);
    }
    private void Update()
    {
        fogMaterial.SetVector("_ViewPosition",player.position);
        fogMaterial.SetFloat("_ViewRange", viewRange);
        fogMaterial.SetTexture("_FogTex",fogTexture);

    }
    private void OnRenderObject()
    {
        fogMaterial.SetPass(0);
    }
}
