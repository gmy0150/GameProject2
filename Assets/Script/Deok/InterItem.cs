// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;

// public class InterItem : MonoBehaviour
// {
//     public string itemName;
//     public Sprite itemIcon;

//     protected int inventorySlotIndex = -1;

//     // ✅ 추가된 부분
//     public virtual void Interact()
//     {
//         // 기본 상호작용 (필요시 덮어쓰기)
//     }

//     public virtual void PickUpItem()
//     {
//         bool added = InventoryManager.Instance.AddItemToInventory(itemIcon, this);
//         if (added)
//         {
//             inventorySlotIndex = GetMyInventoryIndex();
//             Debug.Log($"{itemName}을(를) 인벤토리에 추가함");
//         }
//         else
//         {
//             Debug.Log("인벤토리가 가득 찼습니다.");
//         }
//     }

//     public virtual void RemoveFromInventory()
//     {
//         if (inventorySlotIndex >= 0)
//         {
//             InventoryManager.Instance.RemoveItem(inventorySlotIndex);
//             inventorySlotIndex = -1;
//         }
//     }

//     public virtual void UseItem()
//     {
//         Debug.Log($"{itemName}을(를) 사용했습니다.");
//     }

//     protected int GetMyInventoryIndex()
//     {
//         var slots = InventoryManager.Instance.slots;
//         for (int i = 0; i < slots.Length; i++)
//         {
//             if (slots[i].HasItem() && slots[i].icon.sprite == itemIcon)
//                 return i;
//         }
//         return -1;
//     }
// }
