using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager instance;

    [Header("Durations")]
    public float magnetDuration = 10f;
    public float clockDuration  = 5f;

    private bool  shieldActive = false;
    private bool  magnetActive = false;
    private bool  clockActive  = false;
    private float magnetTimer  = 0f;
    private float clockTimer   = 0f;

    // Cached in Awake — PowerUpHUD reads these instead of PlayerPrefs per-frame
    private int shieldCount;
    private int magnetCount;
    private int clockCount;

    public bool  IsShieldActive  => shieldActive;
    public bool  IsMagnetActive  => magnetActive;
    public bool  IsClockActive   => clockActive;
    public float MagnetTimeLeft  => magnetTimer;
    public float ClockTimeLeft   => clockTimer;
    public int   ShieldCount     => shieldCount;
    public int   MagnetCount     => magnetCount;
    public int   ClockCount      => clockCount;

    void Awake()
    {
        instance = this;
        if (!PlayerPrefs.HasKey("PowerUp_Shield")) PlayerPrefs.SetInt("PowerUp_Shield", 3);
        if (!PlayerPrefs.HasKey("PowerUp_Magnet")) PlayerPrefs.SetInt("PowerUp_Magnet", 3);
        if (!PlayerPrefs.HasKey("PowerUp_Clock"))  PlayerPrefs.SetInt("PowerUp_Clock",  3);
        // Cache counts — PowerUpHUD reads these, no PlayerPrefs per frame
        shieldCount = PlayerPrefs.GetInt("PowerUp_Shield", 0);
        magnetCount = PlayerPrefs.GetInt("PowerUp_Magnet", 0);
        clockCount  = PlayerPrefs.GetInt("PowerUp_Clock",  0);
        PlayerPrefs.Save();
    }

    void Update()
    {
        if (magnetActive)
        {
            magnetTimer -= Time.deltaTime;
            if (magnetTimer <= 0f) magnetActive = false;
        }
        if (clockActive)
        {
            clockTimer -= Time.unscaledDeltaTime;
            if (clockTimer <= 0f) DeactivateClock();
        }
    }

    // ── Shield ───────────────────────────────────────────────
    public bool ActivateShield()
    {
        if (shieldCount <= 0 || shieldActive) return false;
        shieldCount--;
        PlayerPrefs.SetInt("PowerUp_Shield", shieldCount);
        PlayerPrefs.Save();
        shieldActive = true;
        if (BallController.instance != null) BallController.instance.ShowShield(true);
        return true;
    }

    // Called by GameManager when the ball is hit.
    // Returns true if the shield absorbed the hit (player survives).
    public bool ConsumeShield()
    {
        if (!shieldActive) return false;
        shieldActive = false;
        if (BallController.instance != null) BallController.instance.ShowShield(false);
        return true;
    }

    // ── Magnet ───────────────────────────────────────────────
    public bool ActivateMagnet()
    {
        if (magnetCount <= 0) return false;
        magnetCount--;
        PlayerPrefs.SetInt("PowerUp_Magnet", magnetCount);
        PlayerPrefs.Save();
        magnetActive = true;
        magnetTimer  = magnetDuration;
        return true;
    }

    // ── Clock ────────────────────────────────────────────────
    public bool ActivateClock()
    {
        if (clockCount <= 0 || clockActive) return false;
        clockCount--;
        PlayerPrefs.SetInt("PowerUp_Clock", clockCount);
        PlayerPrefs.Save();
        clockActive = true;
        clockTimer  = clockDuration;
        Time.timeScale      = 0.3f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        return true;
    }

    void DeactivateClock()
    {
        clockActive         = false;
        Time.timeScale      = 1f;
        Time.fixedDeltaTime = 0.02f;
    }
}
