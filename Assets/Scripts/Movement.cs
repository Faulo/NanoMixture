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
        set => thrustCache = Mathf.Clamp(value, 0, 1);
    }
    float thrustCache;
    public float torque {
        get => torqueCache;
        set => torqueCache = Mathf.Clamp(value, -1, 1);
    }

    public Vector2 position {
        get => new Vector2(transform.localPosition.x, transform.localPosition.z);
        set => transform.localPosition = new Vector3(value.x, transform.localPosition.y, value.y);
    }

    public float velocity => attachedRigidbody.velocity.magnitude;

    public float rotation => transform.rotation.eulerAngles.y / 360;

    public float angularVelocity => attachedRigidbody.angularVelocity.y;

    void Awake() {
        Reset();
    }

    public void Reset() {
        thrust = 0;
        torque = 0;
        position = 10 * Random.insideUnitCircle;
        transform.rotation = Quaternion.identity;
        attachedRigidbody.velocity = Vector3.zero;
        attachedRigidbody.angularVelocity = Vector3.zero;
    }

    float torqueCache;

    void FixedUpdate() {
        attachedRigidbody.velocity = transform.forward * thrust * maxForwardSpeed;
        transform.Rotate(transform.up * torque * maxTurnSpeed);
        //attachedRigidbody.AddForce(transform.forward * thrust * maxForwardSpeed * Time.deltaTime);
        //attachedRigidbody.AddTorque(transform.up * torque * maxTurnSpeed * Time.deltaTime);
    }
}
