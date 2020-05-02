using System;
using UnityEngine;

public class Interactable : MonoBehaviour {
    enum InteractableType {
        Candy,
        Antimatter,
        Matter,
        Static,
    }

    public event Action<Interactable> onAnnihalate;
    public Vector2 position => new Vector2(transform.localPosition.x, transform.localPosition.z);

    [SerializeField]
    InteractableType type = default;
    [SerializeField]
    Rigidbody attachedRigidbody = default;

    public bool isCandy => type == InteractableType.Candy;
    public bool isAntimatter => type == InteractableType.Antimatter;
    public bool isMatter => type == InteractableType.Matter;
    public bool isStatic => type == InteractableType.Static;

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.TryGetComponent<Interactable>(out var other)) {
            if (isAntimatter && other.isMatter) {
                gameObject.SetActive(false);
                other.gameObject.SetActive(false);
                onAnnihalate?.Invoke(this);
            }
        }
    }

    void Update() {
        if (transform.position.y < -1) {
            gameObject.SetActive(false);
        }
    }

    public void Reset() {
        if (attachedRigidbody) {
            attachedRigidbody.velocity = Vector3.zero;
        }
        gameObject.SetActive(true);
    }
}
