using UnityEngine;

// Attach to any RectTransform panel that should stay inside the device safe area.
// Works on notched/punch-hole screens (Samsung, iPhone, etc.).
// The panel must use a full-stretch anchor (min 0,0 / max 1,1) before this runs.
[RequireComponent(typeof(RectTransform))]
public class SafeAreaFitter : MonoBehaviour
{
    void Awake()
    {
        Apply();
    }

    void Apply()
    {
        RectTransform rt     = GetComponent<RectTransform>();
        Rect          safe   = Screen.safeArea;
        Vector2       screen = new Vector2(Screen.width, Screen.height);

        Vector2 anchorMin = safe.position / screen;
        Vector2 anchorMax = (safe.position + safe.size) / screen;

        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }
}
