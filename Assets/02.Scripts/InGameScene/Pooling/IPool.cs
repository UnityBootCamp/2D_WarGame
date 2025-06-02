using System;
using System.Collections.Generic;
using UnityEngine;

public interface IPool
{
    Transform Trans { get; set; }


    GameObject Get(Action<GameObject> onGet = null);

    void Release(GameObject obj, Action<GameObject> onRelease = null);

    void Preload(Func<GameObject> factory, int count);
}
