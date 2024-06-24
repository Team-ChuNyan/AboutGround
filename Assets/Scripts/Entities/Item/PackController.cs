using System.Collections.Generic;

public class PackController
{
    private List<Pack> _packs;

    public List<Pack> Packs { get { return _packs; } }

    public PackController()
    {
        _packs = new();
        PackGenerator.Instance.RegisterGenerated((pack) => { _packs.Add(pack); });
        PackGenerator.Instance.RegisterDestroyed((pack) => { _packs.Remove(pack); });
    }

    public static void CarryAll(IEnumerable<ISelectable> packs)
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
