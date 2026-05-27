using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;
    private Vector3 originalLocalPosition;

    void Awake()
    {
        instance = this;
        originalLocalPosition = transform.localPosition;
    }

    public void Shake(float duration = 0.45f, float magnitude = 0.25f)
    {
        StartCoroutine(DoShake(duration, magnitude));
    }

    IEnumerator DoShake(float duration, float magnitude)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            transform.localPosition = new Vector3(
                originalLocalPosition.x + x,
                originalLocalPosition.y + y,
                originalLocalPosition.z
            );
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        transform.localPosition = originalLocalPosition;
    }
}
