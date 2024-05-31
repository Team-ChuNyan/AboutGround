using System;
using System.Collections.Generic;
using UnityEngine;

public class PackGenerator : MonoBehaviourSingleton<PackGenerator>, IObjectGenerator<Pack>
{
    private Pack _prefab;

    private Queue<Pack> _inactives;
    private Transform _root;
    private Pack _newPack;

    private event Action<Pack> Generated;
    private event Action<Pack> Destroyed;

    private void Awake()
    {
        _prefab = Resources.Load<Pack>("Prefabs/Pack");
        _root = new GameObject("Units").transform;
        _inactives = new Queue<Pack>();
    }

    public PackGenerator Prepare(IPackable item)
    {
        _newPack = GetNewPack();
        _newPack.SetItem(item);
        ChangePackMesh();
        return this;
    }

    private Pack GetNewPack()
    {
        if (_inactives.TryDequeue(out var pack))
        {
            pack.gameObject.SetActive(true);
        }
        else
        {
            pack = Instantiate(_prefab, _root);
            pack.name = "Pack";
        }
        return pack;
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

    private void ChangePackMesh()
    {
        // TODO 이미지 변경
        //_newPack.SetMesh();
    }

    public Pack Generate()
    {
        Generated?.Invoke(_newPack);
        return _newPack;
    }

    public void OnDestroyed(Pack pack)
    {
        Destroyed?.Invoke(pack);
        _inactives.Enqueue(pack);
        pack.gameObject.SetActive(false);
    }

    #region Register
    public void RegisterGenerated(Action<Pack> action)
    {
        Generated += action;
    }

    public void RegisterDestroyed(Action<Pack> action)
    {
        Destroyed += action;
    }
    #endregion
}
