using System;
using System.Collections.Generic;
using UnityEngine;

public class PackGenerator : MonoBehaviourSingleton<PackGenerator>
{
    private Pack _prefab;

    private List<Pack> _activePack;
    private Queue<Pack> _inactivePacks;
    private Pack _newPack;

    public List<Pack> ActivePack { get { return _activePack; } }

    private event Action<Pack> GeneratedPack;

    private void Awake()
    {
        _prefab = Resources.Load<Pack>("Prefabs/Pack");
        _activePack = new List<Pack>();
        _inactivePacks = new Queue<Pack>();
    }

    public void RegisterGenerated(Action<Pack> action)
    {
        GeneratedPack += action;
    }

    private void OnGeneratedPack()
    {
        GeneratedPack?.Invoke(_newPack);
    }

    public PackGenerator CreateNewItemPack(IPackable item)
    {
        ObjectPooling(item);
        ChangePackMesh();
        OnGeneratedPack();
        return this;
    }

    public PackGenerator SetPosition(Vector2Int pos)
    {
        _newPack.transform.position = Util.Vector2IntToVector3(pos);
        return this;
    }

    public PackGenerator SetPosition(Vector3 pos)
    {
        _newPack.transform.position = Util.FloorVector3(pos);
        return this;
    }

    public Pack GetNewItemPack()
    {
        return _newPack;
    }

    private void ObjectPooling(IPackable item)
    {
        if (!_inactivePacks.TryDequeue(out _newPack))
        {
            _newPack = Instantiate(_prefab);
        }
        else
        {
            _newPack.gameObject.SetActive(true);
        }
        _activePack.Add(_newPack);
        _newPack.SetItem(item);
    }

    private void ChangePackMesh()
    {
        // TODO 이미지 변경
        //_newPack.SetMesh();
    }

    public void DestoryPack(Pack pack)
    {
        _inactivePacks.Enqueue(pack);
        pack.gameObject.SetActive(false);
    }
}
