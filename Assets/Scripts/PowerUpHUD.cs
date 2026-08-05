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

    // Cache last-shown state — text only rebuilds when something actually changes
    private int  lastShields      = -1;
    private int  lastMagnets      = -1;
    private int  lastClocks       = -1;
    private bool lastShieldLocked = true;   // assume locked → forces first-frame update
    private bool lastMagnetLocked = true;
    private bool lastClockLocked  = true;

    void Update()
    {
        var pm = PowerUpManager.instance;
        if (pm == null) return;

        bool shieldLocked = !pm.IsShieldUnlocked;
        bool magnetLocked = !pm.IsMagnetUnlocked;
        bool clockLocked  = !pm.IsClockUnlocked;

        int shields = pm.ShieldCount;
        int magnets = pm.MagnetCount;
        int clocks  = pm.ClockCount;

        // Shield
        if (shieldCountText != null && (shieldLocked != lastShieldLocked || shields != lastShields))
        {
            lastShieldLocked = shieldLocked;
            lastShields      = shields;
            shieldCountText.text = shieldLocked ? "Ph.2" : shields.ToString();
        }
        // Magnet
        if (magnetCountText != null && (magnetLocked != lastMagnetLocked || magnets != lastMagnets))
        {
            lastMagnetLocked = magnetLocked;
            lastMagnets      = magnets;
            magnetCountText.text = magnetLocked ? "Ph.3" : magnets.ToString();
        }
        // Clock
        if (clockCountText != null && (clockLocked != lastClockLocked || clocks != lastClocks))
        {
            lastClockLocked = clockLocked;
            lastClocks      = clocks;
            clockCountText.text = clockLocked ? "Ph.4" : clocks.ToString();
        }

        if (shieldButton != null) shieldButton.interactable = !shieldLocked && shields > 0 && !pm.IsShieldActive;
        if (magnetButton != null) magnetButton.interactable = !magnetLocked && magnets > 0 && !pm.IsMagnetActive;
        if (clockButton  != null) clockButton.interactable  = !clockLocked  && clocks  > 0 && !pm.IsClockActive;

        if (magnetTimerBar != null)
            magnetTimerBar.value = pm.IsMagnetActive ? pm.MagnetTimeLeft / pm.magnetDuration : 0f;

        if (clockTimerBar != null)
            clockTimerBar.value = pm.IsClockActive ? pm.ClockTimeLeft / pm.clockDuration : 0f;
    }

    public void OnShieldPressed() { PowerUpManager.instance?.ActivateShield(); }
    public void OnMagnetPressed() { PowerUpManager.instance?.ActivateMagnet(); }
    public void OnClockPressed()  { PowerUpManager.instance?.ActivateClock();  }
}
