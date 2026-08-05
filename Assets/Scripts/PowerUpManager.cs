using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager instance;

    [Header("Durations")]
    public float magnetDuration = 10f;
    public float clockDuration  = 5f;

    [Header("Cooldowns")]
    public float magnetCooldownDuration = 30f;
    public float clockCooldownDuration  = 60f;

    private bool  shieldActive = false;
    private bool  magnetActive = false;
    private bool  clockActive  = false;
    private float magnetTimer         = 0f;
    private float clockTimer          = 0f;
    private float magnetCooldownTimer = 0f;
    private float clockCooldownTimer  = 0f;

    // Cached in Awake — PowerUpHUD reads these instead of PlayerPrefs per-frame
    private int  shieldCount;
    private int  magnetCount;
    private int  clockCount;

    // Permanent unlock flags — set when phase is reached for the first time
    private bool shieldUnlocked;
    private bool magnetUnlocked;
    private bool clockUnlocked;

    public bool  IsShieldActive         => shieldActive;
    public bool  IsMagnetActive         => magnetActive;
    public bool  IsClockActive          => clockActive;
    public bool  IsMagnetOnCooldown     => magnetCooldownTimer > 0f;
    public bool  IsClockOnCooldown      => clockCooldownTimer  > 0f;
    public float MagnetTimeLeft         => magnetTimer;
    public float ClockTimeLeft          => clockTimer;
    public float MagnetCooldownLeft     => magnetCooldownTimer;
    public float ClockCooldownLeft      => clockCooldownTimer;
    public int   ShieldCount            => shieldCount;
    public int   MagnetCount            => magnetCount;
    public int   ClockCount             => clockCount;
    public bool  IsShieldUnlocked       => shieldUnlocked;
    public bool  IsMagnetUnlocked       => magnetUnlocked;
    public bool  IsClockUnlocked        => clockUnlocked;

    void Awake()
    {
        instance = this;
        // New installs start with 0 charges — player earns them through the shop
        if (!PlayerPrefs.HasKey("PowerUp_Shield")) PlayerPrefs.SetInt("PowerUp_Shield", 0);
        if (!PlayerPrefs.HasKey("PowerUp_Magnet")) PlayerPrefs.SetInt("PowerUp_Magnet", 0);
        if (!PlayerPrefs.HasKey("PowerUp_Clock"))  PlayerPrefs.SetInt("PowerUp_Clock",  0);
        // Cache counts — PowerUpHUD reads these, no PlayerPrefs per frame
        shieldCount = PlayerPrefs.GetInt("PowerUp_Shield", 0);
        magnetCount = PlayerPrefs.GetInt("PowerUp_Magnet", 0);
        clockCount  = PlayerPrefs.GetInt("PowerUp_Clock",  0);
        // Load permanent unlock state
        shieldUnlocked = PlayerPrefs.GetInt("Shield_Unlocked", 0) == 1;
        magnetUnlocked = PlayerPrefs.GetInt("Magnet_Unlocked", 0) == 1;
        clockUnlocked  = PlayerPrefs.GetInt("Clock_Unlocked",  0) == 1;
        PlayerPrefs.Save();
    }

    // Called by PhaseManager when a new phase is reached.
    // Returns the display name of the newly unlocked power-up, or null if nothing new.
    public string UnlockForPhase(int phase)
    {
        switch (phase)
        {
            case 2:
                if (!shieldUnlocked)
                {
                    shieldUnlocked = true;
                    PlayerPrefs.SetInt("Shield_Unlocked", 1);
                    PlayerPrefs.Save();
                    return "SHIELD";
                }
                break;
            case 3:
                if (!magnetUnlocked)
                {
                    magnetUnlocked = true;
                    PlayerPrefs.SetInt("Magnet_Unlocked", 1);
                    PlayerPrefs.Save();
                    return "MAGNET";
                }
                break;
            case 4:
                if (!clockUnlocked)
                {
                    clockUnlocked = true;
                    PlayerPrefs.SetInt("Clock_Unlocked", 1);
                    PlayerPrefs.Save();
                    return "CLOCK";
                }
                break;
        }
        return null;
    }

    void Update()
    {
        if (magnetActive)
        {
            magnetTimer -= Time.deltaTime;
            if (magnetTimer <= 0f) { magnetActive = false; magnetCooldownTimer = magnetCooldownDuration; }
        }
        else if (magnetCooldownTimer > 0f)
        {
            magnetCooldownTimer -= Time.deltaTime;
            if (magnetCooldownTimer < 0f) magnetCooldownTimer = 0f;
        }

        if (clockActive)
        {
            clockTimer -= Time.unscaledDeltaTime;
            if (clockTimer <= 0f) DeactivateClock();
        }
        else if (clockCooldownTimer > 0f)
        {
            clockCooldownTimer -= Time.unscaledDeltaTime;
            if (clockCooldownTimer < 0f) clockCooldownTimer = 0f;
        }
    }

    // ── Shield ───────────────────────────────────────────────
    public bool ActivateShield()
    {
        if (!shieldUnlocked || shieldCount <= 0 || shieldActive) return false;
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
        if (!magnetUnlocked || magnetCount <= 0 || magnetCooldownTimer > 0f) return false;
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
        if (!clockUnlocked || clockCount <= 0 || clockActive || clockCooldownTimer > 0f) return false;
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
        clockCooldownTimer  = clockCooldownDuration;
    }
}
