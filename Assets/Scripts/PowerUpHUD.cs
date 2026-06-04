using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerUpHUD : MonoBehaviour
{
    [Header("Shield")]
    public Button           shieldButton;
    public TextMeshProUGUI  shieldCountText;

    [Header("Magnet")]
    public Button           magnetButton;
    public TextMeshProUGUI  magnetCountText;
    public Slider           magnetTimerBar;

    [Header("Clock")]
    public Button           clockButton;
    public TextMeshProUGUI  clockCountText;
    public Slider           clockTimerBar;

    // Cache last-shown counts — text only rebuilds when a count actually changes
    private int lastShields = -1;
    private int lastMagnets = -1;
    private int lastClocks  = -1;

    void Update()
    {
        var pm = PowerUpManager.instance;
        if (pm == null) return;

        // Read from in-memory cache — no PlayerPrefs reads per frame
        int shields = pm.ShieldCount;
        int magnets = pm.MagnetCount;
        int clocks  = pm.ClockCount;

        if (shieldCountText != null && shields != lastShields) { lastShields = shields; shieldCountText.text = shields.ToString(); }
        if (magnetCountText != null && magnets != lastMagnets) { lastMagnets = magnets; magnetCountText.text = magnets.ToString(); }
        if (clockCountText  != null && clocks  != lastClocks)  { lastClocks  = clocks;  clockCountText.text  = clocks.ToString();  }

        if (shieldButton != null) shieldButton.interactable = shields > 0 && !pm.IsShieldActive;
        if (magnetButton != null) magnetButton.interactable = magnets > 0 && !pm.IsMagnetActive;
        if (clockButton  != null) clockButton.interactable  = clocks  > 0 && !pm.IsClockActive;

        if (magnetTimerBar != null)
            magnetTimerBar.value = pm.IsMagnetActive ? pm.MagnetTimeLeft / pm.magnetDuration : 0f;

        if (clockTimerBar != null)
            clockTimerBar.value = pm.IsClockActive ? pm.ClockTimeLeft / pm.clockDuration : 0f;
    }

    public void OnShieldPressed() { PowerUpManager.instance?.ActivateShield(); }
    public void OnMagnetPressed() { PowerUpManager.instance?.ActivateMagnet(); }
    public void OnClockPressed()  { PowerUpManager.instance?.ActivateClock();  }
}
