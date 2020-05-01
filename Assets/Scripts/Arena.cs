using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour {
    public ISet<Interactable> interactables;
    void Start() {
        interactables = new HashSet<Interactable>();

        foreach (var interactable in GetComponentsInChildren<Interactable>()) {
            interactable.onDestroy += i => interactables.Remove(i);
            interactables.Add(interactable);
        }
    }
}
