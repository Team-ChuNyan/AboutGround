using System;
using System.Collections.Generic;

public class PropsContainer
{
    private Dictionary<RaceType, List<Unit>> _playerUnits;
    private Dictionary<RaceType, List<Unit>> _npcUnits;
    private List<Pack> _packs;

    public Dictionary<RaceType, List<Unit>> PlayerUnits { get { return _playerUnits; } }
    public Dictionary<RaceType, List<Unit>> NPCUnits { get { return _npcUnits; } }
    public List<Pack> Packs { get { return _packs; } }

    public event Action<PropsContainer> ChangedUnitArray;
    public event Action<PropsContainer> ChangedPackArray;

    public PropsContainer SetUnits(Dictionary<RaceType, List<Unit>> units, Dictionary<RaceType, List<Unit>> npcs)
    {
        _playerUnits = units;
        _npcUnits = npcs;
        ChangedUnitArray?.Invoke(this);
        return this;
    }

    public PropsContainer SetPacks(List<Pack> packs)
    {
        _packs = packs;
        ChangedPackArray?.Invoke(this);
        return this;
    }
}
