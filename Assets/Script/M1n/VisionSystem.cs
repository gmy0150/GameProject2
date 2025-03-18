using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionSystem : MonoBehaviour
{
    public float viewRange = 30;
    public float viewAngle = 90f;
    private void Update()
    {
        DetectVisibleObjects();
    }
    void DetectVisibleObjects()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, viewRange);
        foreach(var collider in colliders)
        {
            Vector3 directionToTarget = collider.transform.position - transform.position;
            float angle = Vector3.Angle(transform.forward,directionToTarget);
            if (angle < viewAngle / 2)
            {
                RaycastHit hit;
                if(Physics.Raycast(transform.position,directionToTarget.normalized,out hit, viewRange))
                {
                    if(hit.collider == collider)
                        Debug.Log("visible: "+collider.name);
                }
            }
        }
    }
}
