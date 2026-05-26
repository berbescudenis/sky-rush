using UnityEngine;

public class FakePath : MonoBehaviour
{
    public float warningDuration = 1.5f;
    public Renderer laneRenderer;

    private bool isActive = false;
    private float timer = 0f;
    private Color warningColor = new Color(1f, 0.2f, 0.2f, 1f);
    private Color normalColor = new Color(0.9f, 0.9f, 0.9f, 1f);
    private bool isFlashing = false;
    private float flashTimer = 0f;
    private bool isCurrentlyRed = false;

    void Start()
    {
        if (laneRenderer != null)
            laneRenderer.material.color = normalColor;
        StartCoroutine(ActivateSequence());
    }

    System.Collections.IEnumerator ActivateSequence()
    {
        // Flash red for warning duration
        isFlashing = true;
        float elapsed = 0f;

        while (elapsed < warningDuration)
        {
            elapsed += Time.deltaTime;
            flashTimer += Time.deltaTime;

            if (flashTimer > 0.2f)
            {
                flashTimer = 0f;
                if (laneRenderer != null)
                {
                    isCurrentlyRed = !isCurrentlyRed;
                    laneRenderer.material.color = isCurrentlyRed ? warningColor : normalColor;
                }
            }
            yield return null;
        }

        // Lock to red — now active and deadly
        isFlashing = false;
        isActive = true;
        if (laneRenderer != null)
            laneRenderer.material.color = warningColor;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isActive)
        {
            GameManager.instance.GameOver();
        }
    }
}