using UnityEngine;
using System.Collections.Generic;

public class ItemFinding
{
    private Dictionary<ItemType, List<Item>> _items;

    public ItemFinding(Dictionary<ItemType, List<Item>> items)
    {
        _items = items;
    }

    public bool TryNearbySearch(ItemType type, Vector2 point, out Item searchItem)
    {
        var list = _items[type];
        var minDistance = float.MaxValue;
        searchItem = null;

        foreach (var item in list)
        {
            if (item.LocalStatus.IsPublicAccess == false)
                continue;

            var itemPos = Util.Vector3XZToVector2(item.Position());
            float distance = Util.DistanceNonSqrt(itemPos, point);

            if (minDistance > distance)
            {
                minDistance = distance;
                searchItem = item;
            }
        }

        if (searchItem is null)
            return false;
        else
            return true;
    }
}
