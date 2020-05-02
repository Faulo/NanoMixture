using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrugSpawner : MonoBehaviour
{
    [SerializeField] GameObject drugPrefab1;
    private Vector3 clickPosition;

    public void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            SpawnDrug();
        }
    }

    public void SpawnDrug() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // create a plane at 0,0,0 whose normal points to +Y:
        Plane hPlane = new Plane(Vector3.up, Vector3.zero);
        // Plane.Raycast stores the distance from ray.origin to the hit point in this variable:
        float distance = 0;
        // if the ray hits the plane...
        if (hPlane.Raycast(ray, out distance)) {
            // get the hit point:
            clickPosition = ray.GetPoint(distance);
            clickPosition.y = 5;
        };

        Instantiate(drugPrefab1, clickPosition, Quaternion.identity);
    }

}
