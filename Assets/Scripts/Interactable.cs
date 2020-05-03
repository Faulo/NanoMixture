using System;
using UnityEngine;
using UnityEngine.Assertions;

public class Interactable : MonoBehaviour {
    enum InteractableType {
        Candy,
        Antimatter,
        Matter,
        Static,
    }

    public static event Action<Interactable> onInstantiate;

    public event Action<Vector3> onAnnihalate;
    public event Action<Interactable> onDisable;
    public event Action<Interactable> onDestroy;
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
        if (!isActive) {
            return;
        }
        if (collision.gameObject.TryGetComponent<Interactable>(out var other)) {
            if (isAntimatter && other.isMatter) {
                gameObject.SetActive(false);
                other.gameObject.SetActive(false);
                onAnnihalate?.Invoke(collision.GetContact(0).point);
            }
        }
    }

    bool isActive = false;
    void Start() {
        isActive = true;
        onInstantiate?.Invoke(this);
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

    void OnDisable() {
        onDisable?.Invoke(this);
    }
    void OnDestroy() {
        onDestroy?.Invoke(this);
    }
}
