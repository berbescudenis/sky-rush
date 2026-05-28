using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [Header("Player Profile")]
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI levelText;
    public UnityEngine.UI.Slider xpBar;
    public TextMeshProUGUI xpText;

    [Header("Currencies")]
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI gemText;
    public TextMeshProUGUI crystalText;

    [Header("Best Score")]
    public TextMeshProUGUI bestScoreText;

    [Header("Missions (progress texts)")]
    public TextMeshProUGUI missionCoinsText;
    public TextMeshProUGUI missionJumpsText;
    public TextMeshProUGUI missionSurviveText;
    public UnityEngine.UI.Slider missionCoinsBar;
    public UnityEngine.UI.Slider missionJumpsBar;
    public UnityEngine.UI.Slider missionSurviveBar;

    [Header("Power-Up Counts")]
    public TextMeshProUGUI shieldCountText;
    public TextMeshProUGUI magnetCountText;
    public TextMeshProUGUI clockCountText;

    [Header("Panels (assign in Inspector, start disabled)")]
    public GameObject shopPanel;
    public GameObject ballsPanel;
    public GameObject worldsPanel;
    public GameObject eventsPanel;

    // Mission targets (fixed for now, can be data-driven later)
    private const int CoinTarget    = 500;
    private const int JumpTarget    = 20;
    private const int SurviveTarget = 180; // seconds

    void Start()
    {
        RefreshAll();
    }

    public void RefreshAll()
    {
        RefreshProfile();
        RefreshCurrencies();
        RefreshBestScore();
        RefreshMissions();
        RefreshPowerUps();
    }

    void RefreshProfile()
    {
        var p = PlayerProfile.instance;
        if (p == null) return;

        if (playerNameText != null) playerNameText.text = p.PlayerName;
        if (levelText != null)      levelText.text      = p.Level.ToString();

        float xpProgress = p.XPToNextLevel > 0 ? (float)p.XP / p.XPToNextLevel : 0f;
        if (xpBar != null)  xpBar.value    = xpProgress;
        if (xpText != null) xpText.text    = p.XP + " / " + p.XPToNextLevel;
    }

    void RefreshCurrencies()
    {
        int coins    = PlayerPrefs.GetInt("TotalCoins", 0);
        int gems     = PlayerProfile.instance != null ? PlayerProfile.instance.Gems     : 0;
        int crystals = PlayerProfile.instance != null ? PlayerProfile.instance.Crystals : 0;

        if (coinText    != null) coinText.text    = coins.ToString("N0");
        if (gemText     != null) gemText.text     = gems.ToString("N0");
        if (crystalText != null) crystalText.text = crystals.ToString("N0");
    }

    void RefreshBestScore()
    {
        int best = PlayerPrefs.GetInt("BestScore", 0);
        if (bestScoreText != null) bestScoreText.text = best.ToString("N0");
    }

    void RefreshMissions()
    {
        int coins   = PlayerPrefs.GetInt("TotalCoins", 0);
        int jumps   = PlayerPrefs.GetInt("TotalJumps", 0);
        int survive = PlayerPrefs.GetInt("BestScore", 0) / 10; // score is 10 pts/sec

        if (missionCoinsText  != null) missionCoinsText.text  = coins   + " / " + CoinTarget;
        if (missionJumpsText  != null) missionJumpsText.text  = jumps   + " / " + JumpTarget;
        if (missionSurviveText != null)
        {
            int sMin = survive / 60, sSec = survive % 60;
            int tMin = SurviveTarget / 60, tSec = SurviveTarget % 60;
            missionSurviveText.text = $"{sMin}:{sSec:D2} / {tMin}:{tSec:D2}";
        }

        if (missionCoinsBar   != null) missionCoinsBar.value   = Mathf.Clamp01((float)coins   / CoinTarget);
        if (missionJumpsBar   != null) missionJumpsBar.value   = Mathf.Clamp01((float)jumps   / JumpTarget);
        if (missionSurviveBar != null) missionSurviveBar.value = Mathf.Clamp01((float)survive / SurviveTarget);
    }

    void RefreshPowerUps()
    {
        if (shieldCountText != null) shieldCountText.text = PlayerPrefs.GetInt("PowerUp_Shield", 3).ToString();
        if (magnetCountText != null) magnetCountText.text = PlayerPrefs.GetInt("PowerUp_Magnet", 3).ToString();
        if (clockCountText  != null) clockCountText.text  = PlayerPrefs.GetInt("PowerUp_Clock",  3).ToString();
    }

    // ── Buttons ──────────────────────────────────────────────

    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void OpenShop()   => ShowPanel(shopPanel);
    public void OpenBalls()  => ShowPanel(ballsPanel);
    public void OpenWorlds() => ShowPanel(worldsPanel);
    public void OpenEvents() => ShowPanel(eventsPanel);
    public void ClosePanel() => ShowPanel(null);

    void ShowPanel(GameObject target)
    {
        if (shopPanel   != null) shopPanel.SetActive(shopPanel   == target);
        if (ballsPanel  != null) ballsPanel.SetActive(ballsPanel  == target);
        if (worldsPanel != null) worldsPanel.SetActive(worldsPanel == target);
        if (eventsPanel != null) eventsPanel.SetActive(eventsPanel == target);
    }
}
