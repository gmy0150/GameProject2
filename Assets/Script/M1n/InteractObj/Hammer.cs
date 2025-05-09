using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : StorageItem
{
    public override void inititem()
    {
        interact = character.Enemy;
    }
    

    public override void UseItem()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, interact))
        {
            Debug.Log($"충돌전 :");

            Vector3 hitPos = hit.point;
            hitPos.y = character.transform.position.y;
            float distance = Vector3.Distance(character.transform.position,hitPos);
            if(distance > interactDis){//interact보다 distance가 크면 return
                return;
            }
            Enemy enemy = hit.collider.gameObject.GetComponentInParent<Enemy>();
            Debug.Log($"충돌gn : {enemy}");
            enemy.HitEnemy();
            InventoryManager.Instance.GetSlot().ClearItem();


        }
    }


}
