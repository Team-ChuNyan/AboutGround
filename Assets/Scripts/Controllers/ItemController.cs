using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    private ItemGenerator _generator;
    private List<Item> _items;

    private void Awake()
    {
        _generator = new ItemGenerator();
        _items = new List<Item>();
    }

    public Item CreateNewItem(ItemType type, int stack = 1, int durability = int.MaxValue)
    {
        var newItem = _generator.SetNewItem(type, stack, durability)
                                .GetNewItem();

        _items.Add(newItem);
        return newItem;
    }
}
