using UnityEngine;
using TMPro;

public class BallSelector : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI ballNameText;
    public TextMeshProUGUI ballRarityText;

    private static readonly string[] BallNames     = { "NEBULA", "INFERNO", "GLACIER", "VORTEX", "PHANTOM" };
    private static readonly string[] BallRarities  = { "COMMON", "RARE",    "RARE",    "EPIC",   "LEGENDARY" };

    private int selectedIndex;

    void Start()
    {
        selectedIndex = PlayerPrefs.GetInt("SelectedBall", 0);
        Refresh();
    }

    public void Next()
    {
        selectedIndex = (selectedIndex + 1) % BallNames.Length;
        Save();
    }

    public void Previous()
    {
        selectedIndex = (selectedIndex - 1 + BallNames.Length) % BallNames.Length;
        Save();
    }

    void Save()
    {
        PlayerPrefs.SetInt("SelectedBall", selectedIndex);
        PlayerPrefs.Save();
        Refresh();
    }

    void Refresh()
    {
        if (ballNameText != null)   ballNameText.text   = BallNames[selectedIndex];
        if (ballRarityText != null) ballRarityText.text = BallRarities[selectedIndex];
    }

    public int GetSelectedIndex() => selectedIndex;
    public string GetSelectedName() => BallNames[selectedIndex];
}
