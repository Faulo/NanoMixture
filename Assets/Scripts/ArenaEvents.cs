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
    [SerializeField]
    UnityEvent onF1 = default;
    [SerializeField]
    UnityEvent onF2 = default;

    void Start() {
        observedArena.onAnnihalateMatter += onAnnihalateMatter.Invoke;
        observedArena.onCollectCandy += onCollectCandy.Invoke;
        observedArena.onWin += onWin.Invoke;
        observedArena.onTilt += onTilt.Invoke;
        observedArena.onF1 += onF1.Invoke;
        observedArena.onF2 += onF2.Invoke;
    }
}
