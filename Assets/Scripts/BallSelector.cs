using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BallSelector : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI ballNameText;
    public TextMeshProUGUI ballRarityText;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI coinBalanceText;
    public TextMeshProUGUI gemBalanceText;
    public Button          unlockButton;
    public Button          selectButton;

    [Header("Preview")]
    public Renderer   previewBallRenderer;
    public Material[] ballMaterials;

    private static readonly string[] BallNames    = { "NEBULA", "INFERNO", "GLACIER", "VORTEX",  "PHANTOM"   };
    private static readonly string[] BallRarities = { "COMMON", "RARE",    "RARE",    "EPIC",    "LEGENDARY" };

    [Header("Unlock Costs")]
    public int[] coinPrices = { 0, 200, 200, 0,  0  };
    public int[] gemPrices  = { 0, 0,   0,   5,  15 };

    private int    browseIndex;
    private int    activeIndex;
    private bool[] unlocked;

    void Start()
    {
        unlocked    = new bool[BallNames.Length];
        unlocked[0] = true;
        for (int i = 1; i < BallNames.Length; i++)
            unlocked[i] = PlayerPrefs.GetInt("Ball_" + i + "_Unlocked", 0) == 1;

        activeIndex = PlayerPrefs.GetInt("SelectedBall", 0);
        browseIndex = activeIndex;
        Refresh();
    }

    public void Next()
    {
        browseIndex = (browseIndex + 1) % BallNames.Length;
        Refresh();
    }

    public void Previous()
    {
        browseIndex = (browseIndex - 1 + BallNames.Length) % BallNames.Length;
        Refresh();
    }

    public void Select()
    {
        if (!unlocked[browseIndex]) return;
        activeIndex = browseIndex;
        PlayerPrefs.SetInt("SelectedBall", activeIndex);
        PlayerPrefs.Save();
        Refresh();
    }

    public void Unlock()
    {
        int i = browseIndex;
        if (unlocked[i]) return;

        int coins = PlayerPrefs.GetInt("TotalCoins", 0);
        int gems  = PlayerPrefs.GetInt("Gems", 0);

        if (coinPrices[i] > 0)
        {
            if (coins < coinPrices[i]) return;
            PlayerPrefs.SetInt("TotalCoins", coins - coinPrices[i]);
        }
        else if (gemPrices[i] > 0)
        {
            if (gems < gemPrices[i]) return;
            PlayerPrefs.SetInt("Gems", gems - gemPrices[i]);
        }

        unlocked[i] = true;
        PlayerPrefs.SetInt("Ball_" + i + "_Unlocked", 1);
        PlayerPrefs.Save();

        // Refresh main menu currencies
        MainMenu mm = FindObjectOfType<MainMenu>();
        if (mm != null) mm.RefreshAll();

        Refresh();
    }

    void Refresh()
    {
        if (ballNameText   != null) ballNameText.text   = BallNames[browseIndex];
        if (ballRarityText != null) ballRarityText.text = BallRarities[browseIndex];

        if (previewBallRenderer != null && ballMaterials != null && browseIndex < ballMaterials.Length)
            previewBallRenderer.material = ballMaterials[browseIndex];

        bool isUnlocked = unlocked[browseIndex];
        bool isActive   = browseIndex == activeIndex;

        int coins = PlayerPrefs.GetInt("TotalCoins", 0);
        int gems  = PlayerPrefs.GetInt("Gems", 0);

        if (coinBalanceText != null) coinBalanceText.text = coins + " coins";
        if (gemBalanceText  != null) gemBalanceText.text  = gems  + " gems";

        if (priceText != null)
        {
            if      (!isUnlocked && coinPrices[browseIndex] > 0) priceText.text = coinPrices[browseIndex] + " coins";
            else if (!isUnlocked && gemPrices[browseIndex]  > 0) priceText.text = gemPrices[browseIndex]  + " gems";
            else if (isActive)                                    priceText.text = "ACTIVE";
            else                                                  priceText.text = "";
        }

        if (unlockButton != null)
        {
            unlockButton.gameObject.SetActive(!isUnlocked);
            if (!isUnlocked)
            {
                bool canAfford = coinPrices[browseIndex] > 0
                    ? coins >= coinPrices[browseIndex]
                    : gems  >= gemPrices[browseIndex];
                unlockButton.interactable = canAfford;
            }
        }

        if (selectButton != null)
        {
            selectButton.gameObject.SetActive(isUnlocked);
            selectButton.interactable = !isActive;
        }
    }

    public int    GetSelectedIndex() => activeIndex;
    public string GetSelectedName()  => BallNames[activeIndex];
}
