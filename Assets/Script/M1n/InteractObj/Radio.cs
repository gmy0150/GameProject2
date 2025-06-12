using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Radio : UseageInteract
{
    bool Active;
    [SerializeField]float Radiotiemr;
    float timer;
    [SerializeField]Image uiimage;
    [SerializeField]GameObject otherobj;
    public override void UpdateTime(float time)
    {
        
    }
    public override bool IsOneTimeInteraction()
    {
        return true;
    }
    public override void Interact(Player character, IController controller)
    {
        base.Interact(character, controller);
        TunOn();
        character.SetAudio("Radio");
        
    }
    void Update()
    {
        if(Active)
        timer += Time.deltaTime;
        if(Radiotiemr < timer){
            TunOff();
        }
    }
    [SerializeField] float radius;
    bool ContactEnemy = false;
    void TunOff(){
        ContactEnemy = false;
        Active = false;
        uiimage.gameObject.SetActive(false);
    }
    void TunOn()
{
    Active = true;

    Vector3 origin = transform.position;
    if (otherobj)
        origin = otherobj.transform.position;
    origin.y = 1f;
    uiimage.gameObject.SetActive(true);

    float closestDistOverall = Mathf.Infinity;
    Enemy closestEnemyOverall = null;
    Vector3 closestHitPos = Vector3.zero;

    for (float anglestep = 0; anglestep < 360f; anglestep += 10)
    {
        float currentAngle = anglestep * Mathf.Deg2Rad;
        Vector3 direction = new Vector3(Mathf.Cos(currentAngle), 0, Mathf.Sin(currentAngle));
        Debug.DrawRay(origin, direction * radius, Color.green, 1f);

        RaycastHit[] hits = Physics.RaycastAll(origin, direction, radius);

        foreach (var hit in hits)
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                float dist = Vector3.Distance(origin, hit.point);
                if (dist < closestDistOverall)
                {
                    closestDistOverall = dist;
                    closestEnemyOverall = enemy;
                    closestHitPos = hit.point;
                }
            }
        }
    }

    if (closestEnemyOverall != null)
    {
        Vector3 hittrans = (closestEnemyOverall.transform.position - origin).normalized;
        Vector3 hitpos = origin + hittrans;
        Debug.Log(hitpos);
        closestEnemyOverall.ProbArea(hitpos);
        ContactEnemy = true;
    }
}


}
