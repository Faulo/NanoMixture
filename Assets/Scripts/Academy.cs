using UnityEngine;

public class Academy : MonoBehaviour {
    [SerializeField]
    Arena arenaPrefab = default;
    [SerializeField, Range(0, 100)]
    int arenaRows = 1;
    [SerializeField, Range(0, 100)]
    int arenaColumns = 1;

    void Awake() {
        var offset = 2 * arenaPrefab.size;
        for (int x = 0; x < arenaColumns; x++) {
            for (int y = 0; y < arenaRows; y++) {
                var arena = Instantiate(arenaPrefab, transform);
                arena.transform.localPosition = new Vector3(x * offset.x, 0, y * offset.y);
            }
        }
    }
}
