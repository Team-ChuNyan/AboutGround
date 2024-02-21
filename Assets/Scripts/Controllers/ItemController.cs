using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    private ItemGenerator _generator;
    private PackGenerator _packGenerator;
    private List<Item> _items;
    private List<Pack> _packs;

    private void Awake()
    {
        _generator = new ItemGenerator();
        _items = new List<Item>();
        _packs = new List<Pack>();
    }

    public void Initialize(PackGenerator packGenerator)
    {
        _packGenerator = packGenerator;
    }

    public Item CreateNewItem(ItemType type, int stack = 1, int durability = int.MaxValue)
    {
        var newItem = _generator.SetNewItem(type, stack, durability)
                                .GetNewItem();

        _items.Add(newItem);
        return newItem;
    }

    public Pack CreateNewPack(IPackable pack, Vector2Int pos = new ())
    {
        var newPack = _packGenerator.CreateNewItemPack(pack)
                                    .SetPosition(pos)
                                    .GetNewItemPack();
        _packs.Add(newPack);
        return newPack;
    }
}
