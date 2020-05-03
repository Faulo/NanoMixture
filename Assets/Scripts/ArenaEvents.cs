using System;
using UnityEngine;
using UnityEngine.Events;

public class ArenaEvents : MonoBehaviour {
    [Serializable]
    class PositionEvent : UnityEvent<Vector3> { }
    [SerializeField]
    Arena observedArena = default;

    [SerializeField]
    PositionEvent onAnnihalateMatter = default;
    [SerializeField]
    PositionEvent onCollectCandy = default;
    [SerializeField]
    UnityEvent onWin = default;
    [SerializeField]
    UnityEvent onTilt = default;

    void Start() {
        observedArena.onAnnihalateMatter += onAnnihalateMatter.Invoke;
        observedArena.onCollectCandy += onCollectCandy.Invoke;
        observedArena.onWin += onWin.Invoke;
        observedArena.onTilt += onTilt.Invoke;
    }
}
