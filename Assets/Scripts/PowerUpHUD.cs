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

    void Update()
    {
        var pm = PowerUpManager.instance;
        if (pm == null) return;

        int shields = PlayerPrefs.GetInt("PowerUp_Shield", 0);
        int magnets = PlayerPrefs.GetInt("PowerUp_Magnet", 0);
        int clocks  = PlayerPrefs.GetInt("PowerUp_Clock",  0);

        if (shieldCountText != null) shieldCountText.text = shields.ToString();
        if (magnetCountText != null) magnetCountText.text = magnets.ToString();
        if (clockCountText  != null) clockCountText.text  = clocks.ToString();

        if (shieldButton != null) shieldButton.interactable = shields > 0 && !pm.IsShieldActive;
        if (magnetButton != null) magnetButton.interactable = magnets > 0 && !pm.IsMagnetActive;
        if (clockButton  != null) clockButton.interactable  = clocks  > 0 && !pm.IsClockActive;

        if (magnetTimerBar != null)
            magnetTimerBar.value = pm.IsMagnetActive
                ? pm.MagnetTimeLeft / PowerUpManager.instance.magnetDuration
                : 0f;

        if (clockTimerBar != null)
            clockTimerBar.value = pm.IsClockActive
                ? pm.ClockTimeLeft / PowerUpManager.instance.clockDuration
                : 0f;
    }

    public void OnShieldPressed() { PowerUpManager.instance?.ActivateShield(); }
    public void OnMagnetPressed() { PowerUpManager.instance?.ActivateMagnet(); }
    public void OnClockPressed()  { PowerUpManager.instance?.ActivateClock();  }
}
