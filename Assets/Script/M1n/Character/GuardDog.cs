using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardDog : Enemy
{
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
    public override void StartChase(Player player)
    {
        base.StartChase(player);
        HomeSuccess = false;

    }
    public override void MakeNoise(GameObject obj, float radius, float stepsize)
    {
        Vector3 origin = obj.transform.position;
        origin.y = 1.5f;

        for (float anglestep = 0; anglestep < 360f; anglestep += stepsize)
        {
            float currentAngle = anglestep * Mathf.Deg2Rad;

            Vector3 direction = new Vector3(Mathf.Cos(currentAngle), 0, Mathf.Sin(currentAngle));
            Debug.DrawRay(origin, direction * radius, Color.red, 5f);

            RaycastHit[] hits = Physics.RaycastAll(origin, direction, radius);

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.GetComponent<GuardAI>())
                {
                    GuardAI guardAi = hit.collider.GetComponent<GuardAI>();
                    guardAi.ProbArea(origin);
                }
            }
        }
    }
    public float radius = 5;
    protected override void MoveToTarget(Vector3 newTarget)
    {
            aIPath.enabled = true;
        Vector3 targetPos = newTarget;
        Vector3 aipos = transform.position;
        float distance = Vector3.Distance(aipos, targetPos);
        if(distance < radius)
        {
            Debug.Log("sss");
            aIPath.isStopped = true;
            return;
        }
        else
        {
            Vector3 direction = (targetPos - aipos).normalized;
            targetPos = aipos + direction * radius;
            aIPath.destination = targetPos;

            aIPath.isStopped = false;

        }
        
        //aIPath.SearchPath();
    }
}
