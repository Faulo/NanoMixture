﻿using UnityEngine;

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

    public Vector2 position => new Vector2(transform.localPosition.x, transform.localPosition.z);
    public Vector2 velocity {
        get {
            var localVelocity = transform.InverseTransformDirection(attachedRigidbody.velocity);
            return new Vector2(localVelocity.x, localVelocity.z);
        }
    }

    public float rotation => transform.rotation.eulerAngles.y / 360;

    public float angularVelocity => attachedRigidbody.angularVelocity.y;

    public void Reset() {
        thrust = 0;
        torque = 0;
        transform.localPosition = Vector3.zero;
        transform.rotation = Quaternion.identity;
        attachedRigidbody.velocity = Vector3.zero;
        attachedRigidbody.angularVelocity = Vector3.zero;
    }

    float torqueCache;

    void FixedUpdate() {
        attachedRigidbody.velocity = transform.forward * thrust * maxForwardSpeed;
        attachedRigidbody.angularVelocity = transform.up * torque * maxTurnSpeed;
        //attachedRigidbody.AddForce(transform.forward * thrust * maxForwardSpeed * Time.deltaTime);
        //attachedRigidbody.AddTorque(transform.up * torque * maxTurnSpeed * Time.deltaTime);
    }
}
