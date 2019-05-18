using UnityEngine;
using System.Collections;

public interface IObserver {

    void OnNotify(params object[] actions);
    GameObject ObserverGameobject();
}
