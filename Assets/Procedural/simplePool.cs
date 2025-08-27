using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Placed on every instance to remember who pooled it.
/// </summary>
public class PoolMember : MonoBehaviour
{
    public SimplePool Pool;
}

/// <summary>
/// A simple GameObject pool keyed by a prefab.
/// </summary>
public class SimplePool
{
    private GameObject _prefab;
    private Transform _parent;
    private Stack<GameObject> _pool = new Stack<GameObject>();

    public SimplePool(GameObject prefab, int initialCapacity = 0, Transform parent = null)
    {
        _prefab = prefab;
        _parent = parent;
        for (int i = 0; i < initialCapacity; i++)
        {
            var go = Object.Instantiate(_prefab, _parent);
            go.SetActive(false);
            AttachPoolMember(go);
            _pool.Push(go);
        }
    }

    /// <summary>
    /// Grab an instance from the pool (or create new if empty).
    /// </summary>
    public GameObject Get()
    {
        GameObject inst;
        if (_pool.Count > 0)
        {
            inst = _pool.Pop();
        }
        else
        {
            inst = Object.Instantiate(_prefab, _parent);
            AttachPoolMember(inst);
        }

        inst.SetActive(true);
        return inst;
    }

    /// <summary>
    /// Return an instance back into the pool.
    /// </summary>
    public void Release(GameObject instance)
    {
        instance.SetActive(false);
        _pool.Push(instance);
    }

    private void AttachPoolMember(GameObject go)
    {
        var pm = go.GetComponent<PoolMember>();
        if (pm == null) pm = go.AddComponent<PoolMember>();
        pm.Pool = this;
    }
}
