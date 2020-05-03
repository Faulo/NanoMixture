using System.Linq;
using Slothsoft.UnityExtensions;
using UnityEngine;

public class Grabber : MonoBehaviour {
    [SerializeField, Range(0, 10)]
    float grabRadius = 1;
    [SerializeField, Range(1, 1000)]
    int colliderCountMaximum = 10;
    [SerializeField]
    LayerMask grabLayers = default;

    [Header("Debug output")]
    public Interactable heldInteractable;
    public bool hasInteractable => heldInteractable && heldInteractable.gameObject && heldInteractable.gameObject.activeInHierarchy;
    public Transform previousParent;
    public bool hold;

    void Awake() {
        colliders = new Collider[colliderCountMaximum];
    }

    int colliderCount;
    Collider[] colliders;

    void FixedUpdate() {
        if (hold) {
            if (hasInteractable) {
                heldInteractable.transform.position = transform.position;
            } else {
                colliderCount = Physics.OverlapSphereNonAlloc(transform.position, grabRadius, colliders, grabLayers);
                colliders
                    .Take(colliderCount)
                    .SelectMany(collider => collider.GetComponents<Interactable>())
                    .Where(interactable => interactable.attachedRigidbody)
                    .Where(interactable => interactable.isMatter || interactable.isAntimatter)
                    .Shuffle()
                    .Take(1)
                    .ForAll(Grab);
            }
        } else {
            if (hasInteractable) {
                Release();
            }
        }
    }

    void LateUpdate() {
        if (hasInteractable) {
            heldInteractable.transform.position = transform.position;
        }
    }

    void Grab(Interactable interactable) {
        heldInteractable = interactable;
        heldInteractable.attachedRigidbody.isKinematic = true;
        //previousParent = heldInteractable.transform.parent;
        //heldInteractable.transform.parent = transform;
        //heldInteractable.transform.localPosition = Vector3.zero;
    }
    void Release() {
        //heldInteractable.transform.parent = previousParent;
        heldInteractable.attachedRigidbody.isKinematic = false;
        heldInteractable = null;
    }
}
