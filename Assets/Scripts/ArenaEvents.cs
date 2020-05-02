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

    void Start() {
        observedArena.onAnnihalateMatter += onAnnihalateMatter.Invoke;
        observedArena.onCollectCandy += onCollectCandy.Invoke;
    }
}
