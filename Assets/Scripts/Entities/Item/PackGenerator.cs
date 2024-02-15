using System.Collections.Generic;
using UnityEngine;

public class PackGenerator : MonoBehaviour
{
    private Pack _prefab;

    private List<Pack> _activePack;
    private Queue<Pack> _inactivePacks;
    private Pack _newPack;

    private void Awake()
    {
        _prefab = Resources.Load<Pack>("Prefabs/Pack");
        _activePack = new List<Pack>();
        _inactivePacks = new Queue<Pack>();
    }

    public PackGenerator CreateNewItemPack(IPackable item)
    {
        TakeOutItemPackPooling(item);
        ChangeItemPackSprite();

        return this;
    }

    public PackGenerator SetPosition(Vector2Int pos)
    {
        _newPack.transform.position = new Vector3(pos.x, pos.y, 0);
        return this;
    }

    public Pack GetNewItemPack()
    {
        return _newPack;
    }

    private void TakeOutItemPackPooling(IPackable item)
    {
        if (!_inactivePacks.TryDequeue(out _newPack))
        {
            _newPack = Instantiate(_prefab);
        }
        _activePack.Add(_newPack);
        _newPack.SetItem(item);
    }

    private void ChangeItemPackSprite()
    {
        // TODO 이미지 변경
        _newPack.SpriteRenderer.color = Color.black;
    }
}
