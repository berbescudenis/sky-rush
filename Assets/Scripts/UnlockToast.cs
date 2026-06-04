using UnityEngine;
using TMPro;

// In-game toast notification for power-up unlocks.
// Attach to a Canvas GameObject that has:
//   - A TextMeshProUGUI child for the message
//   - A CanvasGroup component for fade control
// The GameObject should start invisible (CanvasGroup alpha = 0).
public class UnlockToast : MonoBehaviour
{
    public static UnlockToast instance;

    public TextMeshProUGUI toastText;
    public CanvasGroup     canvasGroup;

    void Awake()
    {
        instance = this;
        if (canvasGroup != null) canvasGroup.alpha = 0f;
    }

    public void ShowToast(string message)
    {
        StopAllCoroutines();
        StartCoroutine(Animate(message));
    }

    System.Collections.IEnumerator Animate(string message)
    {
        if (toastText != null) toastText.text = message;

        // Fade in
        float t = 0f;
        while (t < 0.3f)
        {
            t += Time.deltaTime;
            if (canvasGroup != null) canvasGroup.alpha = Mathf.Clamp01(t / 0.3f);
            yield return null;
        }

        // Hold
        yield return new WaitForSeconds(2.5f);

        // Fade out
        t = 0f;
        while (t < 0.5f)
        {
            t += Time.deltaTime;
            if (canvasGroup != null) canvasGroup.alpha = Mathf.Clamp01(1f - t / 0.5f);
            yield return null;
        }

        if (canvasGroup != null) canvasGroup.alpha = 0f;
    }
}
