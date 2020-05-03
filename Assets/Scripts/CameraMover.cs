using UnityEngine;

public class CameraMover : MonoBehaviour {
    [SerializeField, Range(0, 100)]
    float cameraSpeed = 1;

    void LateUpdate() {
        transform.position += Time.deltaTime * cameraSpeed * new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }
}
