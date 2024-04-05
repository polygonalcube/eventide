using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    //test behavior -- remove later
    [SerializeField]
    [Tooltip("List of items used to test the inventory system")]
    private List<Item> items = new();
    //
    [SerializeField]
    [Tooltip("Inventory items are held by inventory slots.")]
    private GameObject inventoryItemPrefab;
    [SerializeField]
    [Tooltip("Each accessible inventory slot.")]
    private List<InventorySlot> inventorySlots = new();
    [SerializeField]
    [Range(1, 99)]
    [Tooltip("The max amount of items in a stack")]
    private int maxStackSize = 1;
    private int selectedSlot = -1;


    private void Start()
    {
        ChangeSelectedSlot(0);
    }

    private void Update()
    {
        //test behavior -- remove later
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddItem(items[Random.Range(0, items.Count)]);
        }
        //
    }

    void ChangeSelectedSlot(int newValue)
    {
        if (selectedSlot >= 0)
        {
            inventorySlots[selectedSlot].Deselect();
        }
        inventorySlots[newValue].Select();
        selectedSlot = newValue;
    }

    public bool AddItem(Item item)
    {

        for (int i = 0; i < inventorySlots.Count; i++)
        {
            var slot = inventorySlots[i];
            var itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item == item && itemInSlot.Count < maxStackSize && itemInSlot.item.Stackable)
            {
                itemInSlot.Count++;
                itemInSlot.RefreshCount();
                return true;
            }
            else if (itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                return true;
            }
        }
        return false;
    }

    void SpawnNewItem(Item item, InventorySlot slot)
    {
        var newItemGO = Instantiate(inventoryItemPrefab, slot.transform);
        var inventoryItem = newItemGO.GetComponent<InventoryItem>();
        inventoryItem.InitializeItem(item);
    }
}
