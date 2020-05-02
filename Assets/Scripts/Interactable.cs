using System;
using UnityEngine;

public class Interactable : MonoBehaviour {
    enum InteractableType {
        Candy,
        White,
        Black,
    }

    public event Action<Interactable> onDisable;
    public event Action<Interactable> onAnnihalate;
    public Vector2 position => new Vector2(transform.localPosition.x, transform.localPosition.z);

    [SerializeField]
    InteractableType type = default;
    public bool isCandy => type == InteractableType.Candy;
    public bool isWhite => type == InteractableType.White;
    public bool isBlack => type == InteractableType.Black;
    void OnDisable() {
        onDisable?.Invoke(this);
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.TryGetComponent<Interactable>(out var other)) {
            if (isCandy || other.isCandy) {
                return;
            }
            if (type != other.type) {
                onAnnihalate?.Invoke(this);
                gameObject.SetActive(false);
            }
        }
    }
}
