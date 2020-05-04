using System;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class Brain : Agent {
    public event Action<Brain> onEpisodeBegin;
    public event Action<Brain> onAction;
    public event Action<Brain> onFall;
    public event Action<Brain, Vector3> onCandy;

    [SerializeField]
    Movement movement = default;
    [SerializeField]
    Gatherer gatherer = default;
    [SerializeField]
    Grabber grabber = default;

    public bool reset = false;

    public Vector3 localPosition {
        get => movement.transform.localPosition;
        set => movement.transform.localPosition = value;
    }

    public override void Initialize() {
        gatherer.onCollect += CollectListener;
    }
    void CollectListener(Interactable interactable, Vector3 position) {
        if (interactable.isCandy) {
            interactable.gameObject.SetActive(false);
            onCandy?.Invoke(this, position);
        }
    }

    public override void OnEpisodeBegin() {
        reset = false;
        onEpisodeBegin?.Invoke(this);
    }
    public override void CollectObservations(VectorSensor sensor) {
        //sensor.AddObservation(movement.position);
        sensor.AddObservation(movement.velocity);

        if (grabber.hasInteractable) {
            sensor.AddObservation(grabber.heldInteractable.isMatter ? 1 : -1);
        } else {
            sensor.AddObservation(0);
        }

        //sensor.AddObservation(movement.rotation);
    }
    float[] thrustValues = new float[] { 0, 1 };
    float[] torqueValues = new float[] { -1, 0, 1 };
    bool[] holdValues = new bool[] { false, true };
    public override void OnActionReceived(float[] actions) {
        onAction?.Invoke(this);

        movement.thrust = thrustValues[Mathf.RoundToInt(actions[0])];
        movement.torque = torqueValues[Mathf.RoundToInt(actions[1])];
        grabber.hold = holdValues[Mathf.RoundToInt(actions[2])];

        if (transform.position.y < -1) {
            onFall?.Invoke(this);
        }
        if (reset) {
            EndEpisode();
        }
    }
    public override void Heuristic(float[] actions) {
        actions[0] = Input.GetAxisRaw("Vertical") > 0.5f ? 1 : 0;
        actions[1] = Mathf.Round(Input.GetAxisRaw("Horizontal")) + 1;
        actions[2] = Input.GetKey(KeyCode.Space) ? 1 : 0;
    }

    public void Reset() {
        movement.Reset();
    }
    public void Stop() {
        movement.Stop();
        enabled = false;
    }
}
