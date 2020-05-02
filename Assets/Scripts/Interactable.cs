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
                transform.position += UnityEngine.Random.onUnitSphere;
                onAnnihalate?.Invoke(this);
                gameObject.SetActive(false);
            }
        }
    }

    void Awake() {
        Reset();
    }

    void Update() {
        if (transform.position.y < -1) {
            gameObject.SetActive(false);
        }
    }

    public void Reset() {
        var position = UnityEngine.Random.insideUnitCircle;
        transform.localPosition = 10 * new Vector3(position.x, 0, position.y);
        gameObject.SetActive(true);
    }
}
