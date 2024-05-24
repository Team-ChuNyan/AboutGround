using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    private List<Item> _items;

    private void Awake()
    {
        _items = new List<Item>();
    }

    public Item CreateNewItem(ItemType type, int stack = 1, int durability = int.MaxValue)
    {
        var newItem = ItemGenerator.Instance.SetNewItem(type)
                                            .SetLocalData(stack, durability)
                                            .Generate();

        _items.Add(newItem);
        return newItem;
    }
}
