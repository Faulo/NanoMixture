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
    float candyReward = 1;
    [SerializeField, Range(-10, 10)]
    float fallReward = -1;

    Interactable nearestCandy;
    Interactable nearestWhite;
    Interactable nearestBlack;

    public bool reset = false;

    public override void Initialize() {
        base.Initialize();
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
        sensor.AddObservation(movement.rotation);
        /*
        sensor.AddObservation(nearestCandy ? nearestCandy.position : Vector2.zero);
        sensor.AddObservation(nearestWhite ? nearestWhite.position : Vector2.zero);
        sensor.AddObservation(nearestBlack ? nearestBlack.position : Vector2.zero);
        //*/
    }
    float[] thrustValues = new float[] { 0, 1 };
    float[] torqueValues = new float[] { -1, 0, 1 };
    public override void OnActionReceived(float[] actions) {
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

    void Update() {
        var interactables = arena.interactables
            .OrderBy(interactable => Vector2.Distance(interactable.position, movement.position));
        nearestCandy = interactables.FirstOrDefault(i => i.isCandy);
        nearestWhite = interactables.FirstOrDefault(i => i.isWhite);
        nearestBlack = interactables.FirstOrDefault(i => i.isBlack);
    }
}
