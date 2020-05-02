using System;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class Brain : Agent {
    public event Action<Brain> onEpisodeBegin;
    public event Action<Brain> onAction;
    public event Action<Brain> onFall;
    public event Action<Brain> onCandy;

    [SerializeField]
    Movement movement = default;
    [SerializeField]
    Gatherer gatherer = default;

    public bool reset = false;

    public Vector3 localPosition {
        get => movement.transform.localPosition;
        set => movement.transform.localPosition = value;
    }

    public override void Initialize() {
        gatherer.onCollect += CollectListener;
    }
    void CollectListener(Interactable interactable) {
        if (interactable.isCandy) {
            interactable.gameObject.SetActive(false);
            onCandy?.Invoke(this);
        }
    }

    public override void OnEpisodeBegin() {
        reset = false;
        onEpisodeBegin?.Invoke(this);
    }
    public override void CollectObservations(VectorSensor sensor) {
        //sensor.AddObservation(movement.position);
        sensor.AddObservation(movement.velocity);
        //sensor.AddObservation(movement.rotation);
    }
    float[] thrustValues = new float[] { 0, 1 };
    float[] torqueValues = new float[] { -1, 0, 1 };
    public override void OnActionReceived(float[] actions) {
        onAction?.Invoke(this);

        movement.thrust = thrustValues[Mathf.RoundToInt(actions[0])];
        movement.torque = torqueValues[Mathf.RoundToInt(actions[1])];

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
    }

    public void Reset() {
        movement.Reset();
    }
}
