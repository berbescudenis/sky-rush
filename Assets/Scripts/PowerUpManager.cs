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

    public bool  IsShieldActive  => shieldActive;
    public bool  IsMagnetActive  => magnetActive;
    public bool  IsClockActive   => clockActive;
    public float MagnetTimeLeft  => magnetTimer;
    public float ClockTimeLeft   => clockTimer;

    void Awake()
    {
        instance = this;
        if (!PlayerPrefs.HasKey("PowerUp_Shield")) PlayerPrefs.SetInt("PowerUp_Shield", 3);
        if (!PlayerPrefs.HasKey("PowerUp_Magnet")) PlayerPrefs.SetInt("PowerUp_Magnet", 3);
        if (!PlayerPrefs.HasKey("PowerUp_Clock"))  PlayerPrefs.SetInt("PowerUp_Clock",  3);
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
        int count = PlayerPrefs.GetInt("PowerUp_Shield", 0);
        if (count <= 0 || shieldActive) return false;
        PlayerPrefs.SetInt("PowerUp_Shield", count - 1);
        PlayerPrefs.Save();
        shieldActive = true;
        BallController ball = FindObjectOfType<BallController>();
        if (ball != null) ball.ShowShield(true);
        return true;
    }

    // Called by GameManager when the ball is hit.
    // Returns true if the shield absorbed the hit (player survives).
    public bool ConsumeShield()
    {
        if (!shieldActive) return false;
        shieldActive = false;
        BallController ball = FindObjectOfType<BallController>();
        if (ball != null) ball.ShowShield(false);
        return true;
    }

    // ── Magnet ───────────────────────────────────────────────
    public bool ActivateMagnet()
    {
        int count = PlayerPrefs.GetInt("PowerUp_Magnet", 0);
        if (count <= 0) return false;
        PlayerPrefs.SetInt("PowerUp_Magnet", count - 1);
        PlayerPrefs.Save();
        magnetActive = true;
        magnetTimer  = magnetDuration;
        return true;
    }

    // ── Clock ────────────────────────────────────────────────
    public bool ActivateClock()
    {
        int count = PlayerPrefs.GetInt("PowerUp_Clock", 0);
        if (count <= 0 || clockActive) return false;
        PlayerPrefs.SetInt("PowerUp_Clock", count - 1);
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
