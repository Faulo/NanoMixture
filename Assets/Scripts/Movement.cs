using UnityEngine;

public class Movement : MonoBehaviour {
    [SerializeField]
    Rigidbody attachedRigidbody = default;

    [SerializeField, Range(0, 1000)]
    float maxForwardSpeed = 100;
    [SerializeField, Range(0, 1000)]
    float maxTurnSpeed = 100;

    public float thrust {
        get => thrustCache;
        set => thrustCache = Mathf.Clamp(value, -0.5f, 1);
    }
    float thrustCache;
    public float torque {
        get => torqueCache;
        set => torqueCache = Mathf.Clamp(value, -1, 1);
    }

    public Vector2 position => new Vector2(transform.localPosition.x, transform.localPosition.z);
    public Vector2 velocity => new Vector2(attachedRigidbody.velocity.x, attachedRigidbody.velocity.z);
    public float angularVelocity => attachedRigidbody.angularVelocity.y;

    public void Reset() {
        thrust = 0;
        torque = 0;
        transform.localPosition = Vector3.zero;
        attachedRigidbody.velocity = Vector3.zero;
        attachedRigidbody.angularVelocity = Vector3.zero;
    }

    float torqueCache;

    void FixedUpdate() {
        attachedRigidbody.AddTorque(transform.up * torque * maxTurnSpeed * Time.deltaTime);
        attachedRigidbody.AddForce(transform.forward * thrust * maxForwardSpeed * Time.deltaTime);
    }
}
