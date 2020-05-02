using UnityEngine;

public class SFX : MonoBehaviour {
    [SerializeField]
    AudioSource source = default;
    public void InstantiateAt(Vector3 position) {
        var obj = Instantiate(gameObject, position, Quaternion.identity);
        Destroy(obj, source.clip.length);
    }
}
