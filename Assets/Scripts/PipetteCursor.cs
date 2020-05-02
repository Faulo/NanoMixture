using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipetteCursor : MonoBehaviour
{
    private Vector3 mousePosition;
    [SerializeField] float cursorPositionOffset = 1f;

    // Update is called once per frame
    void Update() {
        MoveCursor();
    }

    public void MoveCursor() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // create a plane at 0,0,0 whose normal points to +Y:
        Plane hPlane = new Plane(Vector3.up, Vector3.zero);
        // Plane.Raycast stores the distance from ray.origin to the hit point in this variable:
        float distance = 0;
        // if the ray hits the plane...
        if (hPlane.Raycast(ray, out distance)) {
            // get the hit point:
            mousePosition = ray.GetPoint(distance);
            mousePosition.y = cursorPositionOffset;
            ;

            gameObject.transform.position = mousePosition;
        }
    }
}
