using System.Linq;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class Brain : Agent {
    [SerializeField]
    Arena arena = default;
    [SerializeField]
    Movement movement = default;
    [SerializeField]
    Gatherer gatherer = default;

    [Header("Rewards")]
    [SerializeField, Range(-10, 10)]
    float collectReward = 1;
    [SerializeField, Range(-10, 10)]
    float fallReward = -1;

    Interactable nearestInteractable;

    bool reset = false;

    public override void Initialize() {
        base.Initialize();
        gatherer.onCollect += CollectListener;
    }
    void CollectListener(GameObject obj) {
        AddReward(collectReward);
        Destroy(obj);
        //reset = true;
    }

    public override void OnEpisodeBegin() {
        reset = false;
        movement.Reset();
    }
    public override void CollectObservations(VectorSensor sensor) {
        sensor.AddObservation(movement.position);
        sensor.AddObservation(movement.velocity);
        sensor.AddObservation(movement.angularVelocity);
        sensor.AddObservation(nearestInteractable ? nearestInteractable.position : Vector2.zero);
    }
    public override void OnActionReceived(float[] actions) {
        movement.thrust = actions[0];
        movement.torque = actions[1];

        if (transform.position.y < -1) {
            AddReward(fallReward);
            reset = true;
        }
        if (reset) {
            EndEpisode();
        }
    }
    public override void Heuristic(float[] actions) {
        actions[0] = Input.GetAxis("Vertical");
        actions[1] = Input.GetAxis("Horizontal");
    }

    void Update() {
        nearestInteractable = arena.interactables
            .OrderBy(interactable => Vector2.Distance(interactable.position, movement.position))
            .FirstOrDefault();
    }
}
