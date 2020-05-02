using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class Brain : Agent {
    [SerializeField]
    Movement movement = default;
    [SerializeField]
    Gatherer gatherer = default;

    [Header("Rewards")]
    [SerializeField, Range(-1, 1)]
    float perStepReward = -0.001f;
    [SerializeField, Range(-1, 1)]
    float candyReward = 1;
    [SerializeField, Range(-1, 1)]
    float fallReward = -1;

    public bool reset = false;

    public override void Initialize() {
        gatherer.onCollect += CollectListener;
    }
    void CollectListener(Interactable interactable) {
        if (interactable.isCandy) {
            AddReward(candyReward);
            interactable.gameObject.SetActive(false);
            reset = true;
        }
    }

    public override void OnEpisodeBegin() {
        reset = false;
        movement.Reset();
    }
    public override void CollectObservations(VectorSensor sensor) {
        //sensor.AddObservation(movement.position);
        sensor.AddObservation(movement.velocity);
        //sensor.AddObservation(movement.rotation);
    }
    float[] thrustValues = new float[] { 0, 1 };
    float[] torqueValues = new float[] { -1, 0, 1 };
    public override void OnActionReceived(float[] actions) {
        AddReward(perStepReward);
        movement.thrust = thrustValues[Mathf.RoundToInt(actions[0])];
        movement.torque = torqueValues[Mathf.RoundToInt(actions[1])];

        if (transform.position.y < -1) {
            AddReward(fallReward);
            reset = true;
        }
        if (reset) {
            EndEpisode();
        }
    }
    public override void Heuristic(float[] actions) {
        actions[0] = Input.GetAxisRaw("Vertical") > 0.5f ? 1 : 0;
        actions[1] = Mathf.Round(Input.GetAxisRaw("Horizontal")) + 1;
    }
}
