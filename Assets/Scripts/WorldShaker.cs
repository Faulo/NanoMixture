using System.Collections;
using Cinemachine;
using Slothsoft.UnityExtensions;
using UnityEngine;

public class WorldShaker : MonoBehaviour {
    [Header("Rigidbody settings")]
    [SerializeField, Range(0, 1000)]
    float explosionMinForce = 100;
    [SerializeField, Range(0, 1000)]
    float explosionMaxForce = 100;
    [SerializeField, Range(0, 1000)]
    float explosionRadius = 100;
    [SerializeField, Range(0, 1)]
    float explosionUpwardsModifier = 0;
    [SerializeField, Range(1, 1000)]
    int explosionsPerFrame = 10;
    [SerializeField, Range(1, 10)]
    int explosionPasses = 1;

    [Header("Camera settings")]
    [SerializeField, Range(0, 100)]
    float shakeIntensity = 5f;
    [SerializeField, Range(0, 1)]
    float shakeIntensityDecay = 0.99f;
    [SerializeField, Range(0, 10)]
    float shakeFrequency = 0.5f;
    [SerializeField, Range(0, 10)]
    float shakeDuration = 0.5f;

    CinemachineBasicMultiChannelPerlin virtualCameraNoise;
    public void ShakeRigidbodies() {
        StartCoroutine(CreateRigidbodiesRoutine());
    }
    IEnumerator CreateRigidbodiesRoutine() {
        var bodies = FindObjectsOfType<Rigidbody>().Shuffle();
        for (int j = 0; j < explosionPasses; j++) {
            var explosionOffset = UnityEngine.Random.insideUnitCircle;
            int i = 0;
            foreach (var body in bodies) {
                if (body && body.gameObject.activeInHierarchy) {
                    var explosionPosition = body.position;
                    explosionPosition.x += explosionOffset.x;
                    explosionPosition.z += explosionOffset.y;
                    body.AddExplosionForce(Random.Range(explosionMinForce, explosionMaxForce), explosionPosition, explosionRadius, explosionUpwardsModifier, ForceMode.Impulse);
                    i++;
                    if (i > explosionsPerFrame) {
                        i -= explosionsPerFrame;
                        yield return null;
                    }
                }
            }
            yield return null;
        }
    }

    public void ShakeCamera() {
        StartCoroutine(CreateCameraRoutine(shakeIntensity, shakeFrequency, shakeDuration));
    }
    IEnumerator CreateCameraRoutine(float intensity, float frequency, float duration) {
        while (duration > 0) {
            Noise(intensity, frequency);
            yield return null;
            duration -= Time.deltaTime;
            intensity *= shakeIntensityDecay;
        }
        Noise(0, 0);
    }
    void Noise(float amplitudeGain, float frequencyGain) {
        if (!virtualCameraNoise) {
            virtualCameraNoise = FindObjectOfType<CinemachineVirtualCamera>()
                .GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
        virtualCameraNoise.m_AmplitudeGain = amplitudeGain;
        virtualCameraNoise.m_FrequencyGain = frequencyGain;
    }
}
