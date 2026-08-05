using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    [Header("Prices (coins)")]
    public int shieldPrice = 50;
    public int magnetPrice = 75;
    public int clockPrice  = 100;

    [Header("Max charges per power-up")]
    public int maxCharges = 10;

    [Header("Coin Balance")]
    public TextMeshProUGUI coinBalanceText;

    [Header("Shield Row")]
    public TextMeshProUGUI shieldCountText;
    public TextMeshProUGUI shieldPriceText;
    public Button          buyShieldButton;

    [Header("Magnet Row")]
    public TextMeshProUGUI magnetCountText;
    public TextMeshProUGUI magnetPriceText;
    public Button          buyMagnetButton;

    [Header("Clock Row")]
    public TextMeshProUGUI clockCountText;
    public TextMeshProUGUI clockPriceText;
    public Button          buyClockButton;

    [Header("Feedback")]
    public TextMeshProUGUI feedbackText;

    void Awake()
    {
        instance = this;
    }

    // Auto-refresh every time the shop panel is opened
    void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        int coins   = PlayerPrefs.GetInt("TotalCoins",    0);
        int shields = PlayerPrefs.GetInt("PowerUp_Shield", 0);
        int magnets = PlayerPrefs.GetInt("PowerUp_Magnet", 0);
        int clocks  = PlayerPrefs.GetInt("PowerUp_Clock",  0);

        // Read unlock state directly from PlayerPrefs (PowerUpManager lives in game scene, not here)
        bool shieldUnlocked = PlayerPrefs.GetInt("Shield_Unlocked", 0) == 1;
        bool magnetUnlocked = PlayerPrefs.GetInt("Magnet_Unlocked", 0) == 1;
        bool clockUnlocked  = PlayerPrefs.GetInt("Clock_Unlocked",  0) == 1;

        if (coinBalanceText != null) coinBalanceText.text = coins.ToString("N0") + " coins";

        if (shieldCountText != null) shieldCountText.text = shieldUnlocked ? "x" + shields : "Locked";
        if (magnetCountText != null) magnetCountText.text = magnetUnlocked ? "x" + magnets : "Locked";
        if (clockCountText  != null) clockCountText.text  = clockUnlocked  ? "x" + clocks  : "Locked";

        if (shieldPriceText != null) shieldPriceText.text = shieldUnlocked ? shieldPrice + " coins" : "Reach Phase 2";
        if (magnetPriceText != null) magnetPriceText.text = magnetUnlocked ? magnetPrice + " coins" : "Reach Phase 3";
        if (clockPriceText  != null) clockPriceText.text  = clockUnlocked  ? clockPrice  + " coins" : "Reach Phase 4";

        if (buyShieldButton != null) buyShieldButton.interactable = shieldUnlocked && coins >= shieldPrice && shields < maxCharges;
        if (buyMagnetButton != null) buyMagnetButton.interactable = magnetUnlocked && coins >= magnetPrice && magnets < maxCharges;
        if (buyClockButton  != null) buyClockButton.interactable  = clockUnlocked  && coins >= clockPrice  && clocks  < maxCharges;

        if (feedbackText != null) feedbackText.text = "";
    }

    // ── Buy methods ──────────────────────────────────────────

    public void BuyShield()
    {
        if (!TrySpend(shieldPrice)) { ShowFeedback("Not enough coins!"); return; }
        PlayerPrefs.SetInt("PowerUp_Shield", PlayerPrefs.GetInt("PowerUp_Shield", 0) + 1);
        PlayerPrefs.Save();
        ShowFeedback("Shield purchased!");
        Refresh();
        RefreshMainMenu();
    }

    public void BuyMagnet()
    {
        if (!TrySpend(magnetPrice)) { ShowFeedback("Not enough coins!"); return; }
        PlayerPrefs.SetInt("PowerUp_Magnet", PlayerPrefs.GetInt("PowerUp_Magnet", 0) + 1);
        PlayerPrefs.Save();
        ShowFeedback("Magnet purchased!");
        Refresh();
        RefreshMainMenu();
    }

    public void BuyClock()
    {
        if (!TrySpend(clockPrice)) { ShowFeedback("Not enough coins!"); return; }
        PlayerPrefs.SetInt("PowerUp_Clock", PlayerPrefs.GetInt("PowerUp_Clock", 0) + 1);
        PlayerPrefs.Save();
        ShowFeedback("Clock purchased!");
        Refresh();
        RefreshMainMenu();
    }

    // ── Helpers ──────────────────────────────────────────────

    bool TrySpend(int price)
    {
        int coins = PlayerPrefs.GetInt("TotalCoins", 0);
        if (coins < price) return false;
        PlayerPrefs.SetInt("TotalCoins", coins - price);
        PlayerPrefs.Save();
        return true;
    }

    void ShowFeedback(string msg)
    {
        if (feedbackText != null) feedbackText.text = msg;
    }

    void RefreshMainMenu()
    {
        MainMenu mm = FindObjectOfType<MainMenu>();
        if (mm != null) mm.RefreshAll();
    }
}
