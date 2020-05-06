using UnityEngine;

public class Movement : MonoBehaviour {
    [SerializeField]
    Rigidbody attachedRigidbody = default;

    [SerializeField, Range(0, 1000)]
    float maxForwardSpeed = 100;
    [SerializeField, Range(0, 1)]
    float forwardLerp = 1;
    [SerializeField, Range(0, 1000)]
    float maxTurnSpeed = 100;
    [SerializeField, Range(0, 1)]
    float turnLerp = 1;
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
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        attachedRigidbody.velocity = Vector3.zero;
        attachedRigidbody.angularVelocity = Vector3.zero;
    }

    float torqueCache;

    void FixedUpdate() {
        attachedRigidbody.velocity = Vector3.Lerp(attachedRigidbody.velocity, transform.forward * thrust * maxForwardSpeed, forwardLerp);
        attachedRigidbody.angularVelocity = Vector3.Lerp(attachedRigidbody.angularVelocity, transform.up * torque * maxTurnSpeed, turnLerp);
        
        //transform.Rotate(transform.up * torque * maxTurnSpeed);
        //attachedRigidbody.AddForce(transform.forward * thrust * maxForwardSpeed * Time.deltaTime);
        //attachedRigidbody.AddTorque(transform.up * torque * maxTurnSpeed * Time.deltaTime);
    }

    public void Stop() {
        attachedRigidbody.velocity = Vector3.zero;
        attachedRigidbody.angularVelocity = Vector3.zero;
        enabled = false;
    }
}
