using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ObjectPool<T>
{
    public delegate T FactoryMethod();  //Devuelve el tipo <T>

    private List<T> _currentStock;
    private FactoryMethod _factoryMethod;
    private bool _isDynamic;
    private Action<T> _turnOnCallback;
    private Action<T> _turnOffCallback;

    /// <summary>
    /// Creates an Object Pool
    /// </summary>
    /// <param name="factoryMethod">Factory Method used to create stock</param>
    /// <param name="turnOnCallback">Turn on callback for the stock</param>
    /// <param name="turnOffCallback">Turn of callback for the stock</param>
    /// <param name="initialStock">Creates an initial stock (default 0)</param>
    /// <param name="isDynamic">Says if the pool is dynamic (default true)</param>
    public ObjectPool(FactoryMethod factoryMethod, Action<T> turnOnCallback, Action<T> turnOffCallback, int initialStock = 0, bool isDynamic = true)
    {
        _factoryMethod = factoryMethod;
        _isDynamic = isDynamic;

        _turnOffCallback = turnOffCallback;
        _turnOnCallback = turnOnCallback;

        _currentStock = new List<T>();

        for (int i = 0; i < initialStock; i++)
        {
            var o = _factoryMethod();
            _turnOffCallback(o);
            _currentStock.Add(o);
        }
    }

    /// <summary>
    /// Gives an unused object from the pool
    /// </summary>
    /// <returns>Object from the pool</returns>
    public T GetObject()
    {
        var result = default(T);
        if (_currentStock.Count > 0)
        {
            result = _currentStock[0];
            _currentStock.RemoveAt(0);
        }
        else if (_isDynamic)
            result = _factoryMethod();
        _turnOnCallback(result);
        return result;
    }

    /// <summary>
    /// Returns an object to the pool
    /// </summary>
    /// <param name="o">Object to return</param>
    public void ReturnObject(T o)
    {
        _turnOffCallback(o);
        _currentStock.Add(o);
    }
}
