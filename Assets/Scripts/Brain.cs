using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class Brain : Agent {
    [SerializeField]
    Movement movement = default;
    [SerializeField]
    Gatherer gatherer = default;

    [Header("Rewards")]
    [SerializeField, Range(-10, 10)]
    float collectReward = 1;
    [SerializeField, Range(-10, 10)]
    float fallReward = -1;

    void Start() {
        gatherer.onCollect += CollectListener;
    }

    void CollectListener(GameObject obj) {
        AddReward(collectReward);
        EndEpisode();
    }

    void Update() {
        movement.thrust = Input.GetAxis("Vertical");
        movement.torque = Input.GetAxis("Horizontal");
    }
    public override void OnEpisodeBegin() {
        movement.Reset();
    }
    public override void CollectObservations(VectorSensor sensor) {
        sensor.AddObservation(movement.position);
        sensor.AddObservation(movement.velocity);
        sensor.AddObservation(movement.angularVelocity);
    }
    public override void OnActionReceived(float[] actions) {
        movement.thrust = actions[0];
        movement.torque = actions[1];

        if (transform.position.y < -1) {
            AddReward(fallReward);
            EndEpisode();
        }
    }
    public override void Heuristic(float[] actions) {
        actions[0] = Input.GetAxis("Horizontal");
        actions[1] = Input.GetAxis("Vertical");
    }
}
