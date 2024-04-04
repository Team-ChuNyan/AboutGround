using System.Collections.Generic;

public class PackController
{
    private List<Pack> _packs;

    public List<Pack> Packs { get { return _packs; } }

    public PackController()
    {
        _packs = new();
        PackGenerator.Instance.RegisterGenerated(AddPacks);
    }

    private void AddPacks(Pack pack)
    {
        _packs.Add(pack);
    }

    public static void CarryAll(List<Pack> packs)
    {
        foreach (Pack pack in packs) 
        {
            pack.CreateCarryWork();
        }
    }

    public static void CarryAll(List<ISelectable> packs)
    {
        foreach (ISelectable selectable in packs)
        {
            if (selectable is not Pack pack)
            {
                UnityEngine.Debug.LogWarning("is not Pack");
                break;
            }

            pack.CreateCarryWork();
        }
    }
}
