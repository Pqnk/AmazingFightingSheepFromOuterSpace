using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.5f; // Durée totale du shake
    public float shakeIntensity = 1.0f; // Intensité du shake en rotation

    private Quaternion originalRotation;

    private void Start()
    {
        originalRotation = transform.localRotation; // Enregistre la rotation initiale
    }

    public void StartShake()
    {
        StopAllCoroutines(); // Arrête tout shake en cours
        StartCoroutine(ShakeCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeIntensity;
            float y = Random.Range(-1f, 1f) * shakeIntensity;
            float z = Random.Range(-1f, 1f) * shakeIntensity;

            transform.localRotation = Quaternion.Euler(new Vector3(x, y, z)) * originalRotation;

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localRotation = originalRotation; // Remet la rotation d'origine
    }
}
