using Slothsoft.UnityExtensions;
using UnityEngine;

public class SFX : MonoBehaviour {
    [SerializeField]
    AudioSource sourcePrefab = default;
    [SerializeField]
    AudioClip[] clips = default;
    [SerializeField, Range(-3, 3)]
    float minPitch = 1;
    [SerializeField, Range(-3, 3)]
    float maxPitch = 1;
    public void InstantiateAt(Vector3 position) {
        var source = Instantiate(sourcePrefab, position, Quaternion.identity);
        source.clip = clips.RandomElement();
        source.pitch = Random.Range(minPitch, maxPitch);
        source.Play();
        Destroy(source.gameObject, source.clip.length);
    }
}
