using System.Collections.Generic;
using System;
using UnityEngine;

public static class Util
{
    public static ItemCategory SwitchItemTypeToItemCategory(ItemType itemType)
    {
        return (int)itemType switch
        {
            >= (int)ItemCategory.Resource => ItemCategory.Resource,
            >= (int)ItemCategory.Consumable => ItemCategory.Consumable,
            >= (int)ItemCategory.Equipment => ItemCategory.Equipment,
            _ => ItemCategory.Resource,
        };
    }

    public static Vector2Int Vector3ToVector2Int(Vector3 vector3)
    {
        return new Vector2Int((int)vector3.x, (int)vector3.y);
    }

    public static Vector3 Vector2IntToVector3(Vector2Int vector2Int)
    {
        return new Vector3(vector2Int.x, vector2Int.y, 0);
    }


    public static Dictionary<K, V> NewEnumKeyDictionary<K,V>() where K : Enum where V : class, new()
    {
        Dictionary<K, V> dictionary = new();
        var enumArray = (K[])Enum.GetValues(typeof(K));

        // 02.22 체크해볼것
        for (int i = 0; i < enumArray.Length; i++)
        {
            K type = enumArray[i];
            dictionary.Add(type, new V());
        }

        return dictionary;
    }
}
