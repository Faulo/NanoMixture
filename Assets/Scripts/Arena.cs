using System;
using System.Collections.Generic;
using System.Linq;
using Slothsoft.UnityExtensions;
using UnityEngine;

public class Arena : MonoBehaviour {
    [SerializeField, Range(-1, 1)]
    float annihilationReward = 1;
    [SerializeField, Range(-1, 1)]
    float clearReward = 1;

    public IEnumerable<Interactable> interactables => m_interactables
            .Where(interactable => interactable.gameObject.activeSelf);
    ISet<Interactable> m_interactables;
    public ISet<Brain> bots;

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

    void Start() {
        for (int i = 0; i < botCount; i++) {
            Instantiate(botPrefab, transform);
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
            interactable.onDisable += NoticeDisable;
            interactable.onAnnihalate += NoticeAnnihilation;
            m_interactables.Add(interactable);
        }

        bots = new HashSet<Brain>(GetComponentsInChildren<Brain>());
    }

    void Update() {
        if (!interactables.Any(i => i.isCandy)) {
            bots.ForAll(bot => bot.AddReward(clearReward));

            m_interactables.ForAll(i => i.Reset());
        }
    }
    void NoticeDisable(Interactable interactable) {
    }

    void NoticeAnnihilation(Interactable interactable) {
        bots.ForAll(bot => bot.AddReward(annihilationReward));
        bots.ForAll(bot => bot.reset = true);
    }
}
