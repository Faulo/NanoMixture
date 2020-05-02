using System;
using System.Collections.Generic;
using System.Linq;
using Slothsoft.UnityExtensions;
using UnityEngine;

public class Arena : MonoBehaviour {
    [Header("Arena")]
    [SerializeField, Range(0, 100)]
    int width = 10;
    [SerializeField, Range(0, 100)]
    int height = 10;

    [Header("Rewards")]
    [SerializeField, Range(-1, 1)]
    float annihilationReward = 1;
    [SerializeField, Range(-1, 1)]
    float clearReward = 1;
    [SerializeField, Range(-1, 1)]
    float perStepReward = -0.001f;
    [SerializeField, Range(-1, 1)]
    float candyReward = 1;
    [SerializeField, Range(-1, 1)]
    float fallReward = -1;
    [SerializeField, Range(0, 10000)]
    int maxStepsUntilReset = 0;


    public IEnumerable<Interactable> interactables => m_interactables
            .Where(interactable => interactable.gameObject.activeSelf);
    ISet<Interactable> m_interactables;
    public ISet<Brain> bots;

    [Header("Prefabs")]
    [SerializeField]
    GameObject botPrefab = default;
    [SerializeField, Range(0, 100)]
    int botCount = 0;

    [SerializeField]
    Interactable candyPrefab = default;
    [SerializeField, Range(0, 100)]
    int candyCount = 0;

    [SerializeField]
    Interactable whitePrefab = default;
    [SerializeField, Range(0, 100)]
    int whiteCount = 0;

    [SerializeField]
    Interactable blackPrefab = default;
    [SerializeField, Range(0, 100)]
    int blackCount = 0;

    Vector3 GetRandomPointInsideArena() {
        float x = UnityEngine.Random.Range(0, width) - width / 2;
        float y = UnityEngine.Random.Range(0, height) - height / 2;
        return new Vector3(x, 0, y);
    }

    void Start() {
        for (int i = 0; i < botCount; i++) {
            var bot = Instantiate(botPrefab, transform);
            var brain = bot.GetComponentInChildren<Brain>();
            brain.MaxStep = maxStepsUntilReset;
            brain.onReset += BrainResetListener;
            brain.onFall += BrainFallListener;
            brain.onAction += BrainActionListener;
            brain.onCandy += BrainCandyListener;
        }
        for (int i = 0; i < candyCount; i++) {
            Instantiate(candyPrefab, transform);
        }
        for (int i = 0; i < whiteCount; i++) {
            Instantiate(whitePrefab, transform);
        }
        for (int i = 0; i < blackCount; i++) {
            Instantiate(blackPrefab, transform);
        }

        m_interactables = new HashSet<Interactable>();

        foreach (var interactable in GetComponentsInChildren<Interactable>()) {
            interactable.onReset += InteractableResetListener;
            interactable.onDisable += InteractableDisableListener;
            interactable.onAnnihalate += InteractableAnnihilationListener;
            m_interactables.Add(interactable);
        }

        bots = new HashSet<Brain>(GetComponentsInChildren<Brain>());
    }

    void BrainResetListener(Brain brain) {
        brain.localPosition = GetRandomPointInsideArena();
    }
    void BrainFallListener(Brain brain) {
        brain.AddReward(fallReward);
        brain.reset = true;
    }
    void BrainActionListener(Brain brain) {
        brain.AddReward(perStepReward);
    }
    void BrainCandyListener(Brain brain) {
        brain.AddReward(candyReward);
        brain.reset = true;
    }

    void InteractableResetListener(Interactable interactable) {
        if (!interactable.isStatic) {
            interactable.transform.localPosition = GetRandomPointInsideArena();
        }
    }
    void InteractableDisableListener(Interactable interactable) {
    }
    void InteractableAnnihilationListener(Interactable interactable) {
        bots.ForAll(bot => bot.AddReward(annihilationReward));
        bots.ForAll(bot => bot.reset = true);
    }

    void Update() {
        if (!interactables.Any(i => i.isCandy)) {
            bots.ForAll(bot => bot.AddReward(clearReward));

            m_interactables.ForAll(i => i.Reset());
        }
    }
}
