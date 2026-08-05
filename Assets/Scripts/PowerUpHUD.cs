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
    private int  lastShields         = -1;
    private int  lastMagnets         = -1;
    private int  lastClocks          = -1;
    private bool lastShieldLocked    = true;
    private bool lastMagnetLocked    = true;
    private bool lastClockLocked     = true;
    private int  lastMagnetCooldown  = -1;
    private int  lastClockCooldown   = -1;

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

        bool magnetOnCooldown = pm.IsMagnetOnCooldown;
        bool clockOnCooldown  = pm.IsClockOnCooldown;
        int  magnetCdSec      = Mathf.CeilToInt(pm.MagnetCooldownLeft);
        int  clockCdSec       = Mathf.CeilToInt(pm.ClockCooldownLeft);

        // Shield
        if (shieldCountText != null && (shieldLocked != lastShieldLocked || shields != lastShields))
        {
            lastShieldLocked = shieldLocked;
            lastShields      = shields;
            shieldCountText.text = shieldLocked ? "Ph.2" : shields.ToString();
        }
        // Magnet — locked > cooldown > count
        if (magnetCountText != null)
        {
            if (magnetLocked != lastMagnetLocked || magnets != lastMagnets || magnetCdSec != lastMagnetCooldown)
            {
                lastMagnetLocked   = magnetLocked;
                lastMagnets        = magnets;
                lastMagnetCooldown = magnetCdSec;
                if      (magnetLocked)    magnetCountText.text = "Ph.3";
                else if (magnetOnCooldown) magnetCountText.text = magnetCdSec + "s";
                else                      magnetCountText.text = magnets.ToString();
            }
        }
        // Clock — locked > cooldown > count
        if (clockCountText != null)
        {
            if (clockLocked != lastClockLocked || clocks != lastClocks || clockCdSec != lastClockCooldown)
            {
                lastClockLocked   = clockLocked;
                lastClocks        = clocks;
                lastClockCooldown = clockCdSec;
                if      (clockLocked)    clockCountText.text = "Ph.4";
                else if (clockOnCooldown) clockCountText.text = clockCdSec + "s";
                else                      clockCountText.text = clocks.ToString();
            }
        }

        if (shieldButton != null) shieldButton.interactable = !shieldLocked && shields > 0 && !pm.IsShieldActive;
        if (magnetButton != null) magnetButton.interactable = !magnetLocked && magnets > 0 && !pm.IsMagnetActive && !magnetOnCooldown;
        if (clockButton  != null) clockButton.interactable  = !clockLocked  && clocks  > 0 && !pm.IsClockActive  && !clockOnCooldown;

        if (magnetTimerBar != null)
        {
            if      (pm.IsMagnetActive)   magnetTimerBar.value = pm.MagnetTimeLeft     / pm.magnetDuration;
            else if (magnetOnCooldown)    magnetTimerBar.value = pm.MagnetCooldownLeft / pm.magnetCooldownDuration;
            else                          magnetTimerBar.value = 0f;
        }

        if (clockTimerBar != null)
        {
            if      (pm.IsClockActive)   clockTimerBar.value = pm.ClockTimeLeft     / pm.clockDuration;
            else if (clockOnCooldown)    clockTimerBar.value = pm.ClockCooldownLeft / pm.clockCooldownDuration;
            else                         clockTimerBar.value = 0f;
        }
    }

    public void OnShieldPressed() { PowerUpManager.instance?.ActivateShield(); }
    public void OnMagnetPressed() { PowerUpManager.instance?.ActivateMagnet(); }
    public void OnClockPressed()  { PowerUpManager.instance?.ActivateClock();  }
}
