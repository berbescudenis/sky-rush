using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;
    private int coinsThisRun = 0;
    private int totalCoins = 0;

    void Awake()
    {
        instance = this;
        totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
    }

    public void AddCoin()
    {
        coinsThisRun++;
        totalCoins++;
        // Saved to PlayerPrefs at game over, not per-coin, to avoid per-frame stutter
    }

    public void SaveCoins()
    {
        PlayerPrefs.SetInt("TotalCoins", totalCoins);
        PlayerPrefs.Save();
    }

    public int GetCoinsThisRun() { return coinsThisRun; }
    public int GetTotalCoins() { return totalCoins; }
}