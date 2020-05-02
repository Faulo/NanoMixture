﻿using System;
using System.Collections.Generic;
using System.Linq;
using Slothsoft.UnityExtensions;
using UnityEngine;

public class Arena : MonoBehaviour {
    enum ArenaMode {
        Playing,
        Training,
    }
    enum ArenaState {
        Cleared,
        Beginning,
        Running
    }
    public event Action<Vector3> onAnnihalateMatter;
    public event Action<Vector3> onCollectCandy;


    [Header("Arena")]
    [SerializeField, Range(0, 100)]
    int width = 10;
    [SerializeField, Range(0, 100)]
    int height = 10;
    public Vector2Int size => new Vector2Int(width, height);
    [SerializeField]
    ArenaMode mode = default;

    [Header("Geometry")]
    [SerializeField, Range(0, 10)]
    int wallSize = 0;
    [SerializeField]
    Transform ground = default;
    [SerializeField]
    Transform[] walls = new Transform[4];

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


    ISet<Transform> obstacles;
    ISet<Interactable> interactables;
    bool hasCandies => interactables
            .Where(interactable => interactable.gameObject.activeSelf)
            .Any(interactable => interactable.isCandy);
    bool hasAntimatter => interactables
            .Where(interactable => interactable.gameObject.activeSelf)
            .Any(interactable => interactable.isAntimatter);
    bool hasMatter => interactables
            .Where(interactable => interactable.gameObject.activeSelf)
            .Any(interactable => interactable.isMatter);
    ISet<Brain> bots;

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

    [SerializeField]
    Transform wallPrefab = default;
    [SerializeField, Range(0, 100)]
    int wallCount = 0;

    [SerializeField]
    Transform antimatterWallPrefab = default;
    [SerializeField, Range(0, 100)]
    int antimatterWallCount = 0;

    [SerializeField]
    Transform matterWallPrefab = default;
    [SerializeField, Range(0, 100)]
    int matterWallCount = 0;


    bool isPlaying => mode == ArenaMode.Playing;
    bool isTraining => mode == ArenaMode.Training;

    [Header("Debug output")]
    [SerializeField]
    ArenaState state = ArenaState.Cleared;

    Vector3 GetRandomPointInsideArena() {
        float x = UnityEngine.Random.Range(0, width) - width / 2;
        float y = UnityEngine.Random.Range(0, height) - height / 2;
        return new Vector3(x, 0, y);
    }

    void OnValidate() {
        var size = new Vector2Int(width + wallSize + wallSize, height + wallSize + wallSize);
        SetGround(ground, size);
        if (walls.Length != 4) {
            Array.Resize(ref walls, 4);
        }
        SetWall(walls[0], new Rect((width + wallSize) / 2f, 0, wallSize, size.y));
        SetWall(walls[1], new Rect((width + wallSize) / -2f, 0, wallSize, size.y));
        SetWall(walls[2], new Rect(0, (height + wallSize) / 2f, size.x, wallSize));
        SetWall(walls[3], new Rect(0, (height + wallSize) / -2f, size.x, wallSize));
    }

    void Start() {

        if (isTraining) {
            obstacles = new HashSet<Transform>();
            for (int i = 0; i < wallCount; i++) {
                var wall = Instantiate(wallPrefab, transform);
                obstacles.Add(wall);
            }
            for (int i = 0; i < antimatterWallCount; i++) {
                var wall = Instantiate(antimatterWallPrefab, transform);
                obstacles.Add(wall);
            }
            for (int i = 0; i < matterWallCount; i++) {
                var wall = Instantiate(matterWallPrefab, transform);
                obstacles.Add(wall);
            }
            for (int i = 0; i < botCount; i++) {
                var bot = Instantiate(botPrefab, transform);
                var brain = bot.GetComponentInChildren<Brain>();
                brain.MaxStep = maxStepsUntilReset;
                brain.onEpisodeBegin += OnEpisodeBeginListener;
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

            interactables = new HashSet<Interactable>();

            foreach (var interactable in GetComponentsInChildren<Interactable>()) {
                interactable.onAnnihalate += InteractableAnnihilationListener;
                interactables.Add(interactable);
            }

            bots = new HashSet<Brain>(GetComponentsInChildren<Brain>());
        }
    }

    void SetGround(Transform ground, Vector2Int dimension) {
        if (!ground) {
            return;
        }
        ground.localScale = new Vector3(dimension.x, 1, dimension.y);
    }

    void SetWall(Transform wall, Rect rect) {
        if (!wall) {
            return;
        }
        wall.localPosition = new Vector3(rect.x, 0, rect.y);
        wall.localScale = new Vector3(rect.width, 1, rect.height);
    }
    void OnEpisodeBeginListener(Brain brain) {
        state = ArenaState.Beginning;

        brain.Reset();
        brain.localPosition = GetRandomPointInsideArena();

        obstacles.ForAll(ResetObstacle);
        interactables.ForAll(ResetInteractable);
    }
    void BrainFallListener(Brain brain) {
        brain.AddReward(fallReward);
        brain.reset = true;
    }
    void BrainActionListener(Brain brain) {
        brain.AddReward(perStepReward);
    }
    void BrainCandyListener(Brain brain, Vector3 position) {
        brain.AddReward(candyReward);
        onCollectCandy?.Invoke(position);
    }

    void ResetInteractable(Interactable interactable) {
        interactable.Reset();
        if (!interactable.isStatic) {
            interactable.transform.localPosition = GetRandomPointInsideArena();
        }
    }
    void InteractableAnnihilationListener(Vector3 position) {
        if (state == ArenaState.Running) {
            bots.ForAll(bot => bot.AddReward(annihilationReward));
        }
        onAnnihalateMatter?.Invoke(position);
    }

    void ResetObstacle(Transform obstacle) {
        obstacle.localPosition = GetRandomPointInsideArena();
        obstacle.localScale = new Vector3(UnityEngine.Random.Range(1, (width + height) / 4), 1, 1);
        obstacle.localRotation = Quaternion.Euler(Vector3.up * UnityEngine.Random.Range(0, 360));
    }

    void Update() {
        if (state == ArenaState.Beginning) {
            state = ArenaState.Running;
        }
        if (isTraining) {
            if (!hasCandies || !hasMatter || !hasAntimatter) {
                Clear();
            }
        }
    }

    void Clear() {
        if (state == ArenaState.Running) {
            state = ArenaState.Cleared;
            bots.ForAll(bot => bot.AddReward(clearReward));
            bots.ForAll(bot => bot.reset = true);
        }
    }
}
