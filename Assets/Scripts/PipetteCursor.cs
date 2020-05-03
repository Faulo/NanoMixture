using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PipetteCursor : MonoBehaviour {
    [Serializable]
    class PositionEvent : UnityEvent<Vector3> { }

    [SerializeField, Range(0, 4)]
    int buttonToActivate = 0;

    [SerializeField, Range(0, 10)]
    float positionHeight = 5;
    [SerializeField, Range(0, 2)]
    float positionVariance = 1;
    [SerializeField, Range(0, 1)]
    float clickingPause = 1;

    [SerializeField]
    PositionEvent onClick = default;
    Plane plane;
    bool isClicking;
    void Awake() {
        // create a plane at 0,0,0 whose normal points to +Y:
        plane = new Plane(Vector3.up, transform.position);
    }
    IEnumerator Start() {
        while (true) {
            if (isClicking) {
                onClick.Invoke(transform.position + (Vector3.up * positionHeight) + (UnityEngine.Random.onUnitSphere * positionVariance));
                yield return new WaitForSeconds(clickingPause);
            } else {
                yield return null;
            }
        }
    }
    void LateUpdate() {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Plane.Raycast stores the distance from ray.origin to the hit point in this variable:
        // if the ray hits the plane...
        if (plane.Raycast(ray, out float distance)) {
            // get the hit point:
            transform.position = ray.GetPoint(distance);
        }


    }

    void Update() {
        isClicking = Input.GetMouseButton(buttonToActivate);
    }
}
