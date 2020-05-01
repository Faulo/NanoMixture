using System;
using UnityEngine;

public class Interactable : MonoBehaviour {
    public event Action<Interactable> onDestroy;
    public Vector2 position => new Vector2(transform.localPosition.x, transform.localPosition.z);
    void OnDestroy() {
        onDestroy?.Invoke(this);
    }
}
