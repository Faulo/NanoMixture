using System;
using System.Collections.Generic;
using System.Linq;
using Slothsoft.UnityExtensions;
using UnityEngine;

public class Arena : MonoBehaviour {
    [SerializeField, Range(-100, 100)]
    float annihilationReward = 1;
    [SerializeField, Range(-100, 100)]
    float clearReward = 1;

    public IEnumerable<Interactable> interactables => m_interactables
            .Where(interactable => interactable.gameObject.activeSelf);
    ISet<Interactable> m_interactables;
    public ISet<Brain> bots;

    void Start() {
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

            m_interactables.ForAll(i => i.gameObject.SetActive(true));
        }
    }
    void NoticeDisable(Interactable interactable) {
    }

    void NoticeAnnihilation(Interactable interactable) {
        bots.ForAll(bot => bot.AddReward(annihilationReward));
        bots.ForAll(bot => bot.reset = true);
    }
}
