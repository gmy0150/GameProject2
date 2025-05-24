
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public InventorySlot[] slots = new InventorySlot[5];
    private int selectedIndex = -1;
    public StorageItem Cam;
    private void Awake()
    {
        Instance = this;
        slots[0].SetItem(Cam);
        // Cam.inititem();
    }


    public void HandleSlotSelection()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SelectSlot(i);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            UseSelectedItem();
        }
    }
    
    void SelectSlot(int index)
    {
        if (selectedIndex == index)
        {
            if(GetActiveItem() != null)
                GetActiveItem().SetHandActive(false);
            ExitSlot();
            return;
        }
        else
        {
            selectedIndex = index;

                for (int i = 0; i < slots.Length; i++)
                {
                    slots[i].SetSelected(i == selectedIndex);
                }
            InitSlot();



        }
    }

    public void ExitSlot()
    {
        
        selectedIndex = -1;
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetSelected(false);
        }
        return;
    }

    public StorageItem GetActiveItem()
    {
        if (selectedIndex != -1)
        {
            var ReturnValue = slots[selectedIndex];

            return ReturnValue.GetItem();

        }
        return null;
    }
    public InventorySlot GetSlot(){
        return slots[selectedIndex];
    }
    public int GetSlotIndex()
    {
        if (selectedIndex > 0)
        {
            return selectedIndex;
        }
        return 0;
    }
    public void InitSlot()
    {
        if (GetActiveItem() != null)
        {
            GetActiveItem().inititem();

        }
    }

    void UseSelectedItem()
    {
        if (selectedIndex >= 0 && selectedIndex < slots.Length)
        {
            slots[selectedIndex].UseItem();
            
        }
    }

    public bool AddItemToInventory(StorageItem item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (!slots[i].HasItem())
            {
                slots[i].SetItem(item);
                return true;
            }
        }
        return false;
    }

    public void RemoveItem()
    {
        if (selectedIndex != -1)
            slots[selectedIndex].ClearItem();

    }
}