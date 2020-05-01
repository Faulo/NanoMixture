using System;
using UnityEngine;

public class Gatherer : MonoBehaviour {

    public event Action<GameObject> onCollect;

    [SerializeField]
    string tagToCollect = "";

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag(tagToCollect)) {
            onCollect?.Invoke(collision.gameObject);
            //Destroy(collision.gameObject);
        }
    }
}
