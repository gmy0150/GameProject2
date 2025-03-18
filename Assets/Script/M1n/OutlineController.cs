using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineController : MonoBehaviour
{
    public Material normalMaterial;
    public Material outlineMaterial;
    private Renderer objRenderer;
    private bool isHidden = false;

    void Start()
    {
        objRenderer = GetComponent<Renderer>();
        objRenderer.material = normalMaterial;
    }

    void Update()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

        RaycastHit hit;
        bool isBlocked = Physics.Linecast(transform.position, Camera.main.transform.position, out hit);

        if (isBlocked && onScreen && !isHidden)
        {
            objRenderer.material = outlineMaterial; // �������� �ƿ����� ���̴� ����
            isHidden = true;
        }
        else if (!isBlocked && isHidden)
        {
            objRenderer.material = normalMaterial; // �ٽ� ���̸� ���� ���̴��� ����
            isHidden = false;
        }
    }
}
