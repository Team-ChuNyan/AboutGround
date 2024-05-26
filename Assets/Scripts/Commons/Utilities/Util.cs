using System;
using System.Collections.Generic;
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

    public static Vector2Int Vector3XZToVector2Int(Vector3 vector3)
    {
        return new Vector2Int((int)vector3.x, (int)vector3.z);
    }

    public static Vector2 Vector3XZToVector2(Vector3 vector3)
    {
        return new Vector2(vector3.x, vector3.z);
    }

    public static Vector3 Vector2IntToVector3(Vector2Int vector2Int)
    {
        return new Vector3(vector2Int.x,0, vector2Int.y);
    }

    public static Vector3 FloorVector3(Vector3 vector3)
    {
        return new Vector3((int)vector3.x, (int)vector3.y, (int)vector3.z);
    }

    public static float DistanceNonSqrt(Vector2 a, Vector2 b)
    {
        float num = a.x - b.x;
        float num2 = a.y - b.y;
        return num * num + num2 * num2;
    }

    public static Dictionary<K, V> NewEnumKeyDictionary<K,V>() where K : Enum where V : class, new()
    {
        Dictionary<K, V> dictionary = new();
        var enumArray = (K[])Enum.GetValues(typeof(K));

        for (int i = 0; i < enumArray.Length; i++)
        {
            K type = enumArray[i];
            dictionary.Add(type, new V());
        }

        return dictionary;
    }

    public static bool IsNumberInRange(float value, float min, float max)
    {
        if (value < min || value > max)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public static void Swap<T>(ref T a, ref T b) where T : class
    {
        T temp = a;
        a = b;
        b = temp;
    }
}
