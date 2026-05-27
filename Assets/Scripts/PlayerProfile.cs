using UnityEngine;

public class PlayerProfile : MonoBehaviour
{
    public static PlayerProfile instance;

    public string PlayerName => PlayerPrefs.GetString("PlayerName", "RUSHER");
    public int Level => PlayerPrefs.GetInt("Level", 1);
    public int XP => PlayerPrefs.GetInt("XP", 0);
    public int XPToNextLevel => Level * 1000;
    public int Gems => PlayerPrefs.GetInt("Gems", 0);
    public int Crystals => PlayerPrefs.GetInt("Crystals", 0);

    void Awake()
    {
        instance = this;
    }

    public void AddXP(int amount)
    {
        int xp = XP + amount;
        int level = Level;
        while (xp >= level * 1000)
        {
            xp -= level * 1000;
            level++;
        }
        PlayerPrefs.SetInt("XP", xp);
        PlayerPrefs.SetInt("Level", level);
        PlayerPrefs.Save();
    }

    public void AddGems(int amount)
    {
        PlayerPrefs.SetInt("Gems", Gems + amount);
        PlayerPrefs.Save();
    }

    public void SpendCoins(int amount)
    {
        int current = PlayerPrefs.GetInt("TotalCoins", 0);
        PlayerPrefs.SetInt("TotalCoins", Mathf.Max(0, current - amount));
        PlayerPrefs.Save();
    }
}
