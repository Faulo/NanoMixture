using System;
using UnityEngine;

public class Gatherer : MonoBehaviour {

    public event Action<Interactable, Vector3> onCollect;

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.TryGetComponent<Interactable>(out var interactable)) {
            onCollect?.Invoke(interactable, collision.GetContact(0).point);
        }
    }
}
