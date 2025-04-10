using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public InventorySlot[] slots = new InventorySlot[5];
    private int selectedIndex = -1;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        HandleSlotSelection();
    }

    void HandleSlotSelection()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SelectSlot(i);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            UseSelectedItem();
        }
    }

    void SelectSlot(int index)
    {
        selectedIndex = index;

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetSelected(i == index);
        }

        Debug.Log($"슬롯 {index + 1} 선택됨");
    }

    void UseSelectedItem()
    {
        if (selectedIndex >= 0 && selectedIndex < slots.Length)
        {
            slots[selectedIndex].UseItem();
        }
    }

    public bool AddItemToInventory(Sprite icon, InterItem item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (!slots[i].HasItem())
            {
                slots[i].SetItem(icon, item);
                return true;
            }
        }
        return false;
    }

    public void RemoveItem(int index)
    {
        if (index >= 0 && index < slots.Length)
        {
            slots[index].ClearItem();
        }
    }
}