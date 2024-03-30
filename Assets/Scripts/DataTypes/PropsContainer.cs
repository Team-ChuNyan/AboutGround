using System.Collections.Generic;

public class PropsContainer
{
    private Dictionary<RaceType, List<Unit>> _playerUnits;
    private Dictionary<RaceType, List<Unit>> _npcUnits;
    private List<Pack> _packs;

    public Dictionary<RaceType, List<Unit>> PlayerUnits { get { return _playerUnits; } }
    public Dictionary<RaceType, List<Unit>> NPCUnits { get { return _npcUnits; } }
    public List<Pack> Packs { get { return _packs; } }

    public PropsContainer SetPlayerUnits(Dictionary<RaceType, List<Unit>> units)
    {
        _playerUnits = units;
        return this;
    }

    public PropsContainer SetNpcUnits(Dictionary<RaceType, List<Unit>> units)
    {
        _npcUnits = units;
        return this;
    }

    public PropsContainer SetPacks(List<Pack> packs)
    {
        _packs = packs;
        return this;
    }
}
