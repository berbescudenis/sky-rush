using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bestScoreText;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI phaseText;
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI distanceText;

    // Cache last-shown values — text only updates when the value actually changes
    private int   lastScore    = -1;
    private int   lastBest     = -1;
    private int   lastCoins    = -1;
    private int   lastCombo    = -1;
    private int   lastDistance = -1;
    private string lastPhase   = null;

    void Update()
    {
        if (ScoreManager.instance != null)
        {
            int score = ScoreManager.instance.GetScore();
            if (scoreText != null && score != lastScore)
            {
                lastScore = score;
                scoreText.text = score.ToString();
            }

            int best = ScoreManager.instance.GetBestScore();
            if (bestScoreText != null && best != lastBest)
            {
                lastBest = best;
                bestScoreText.text = "BEST: " + best;
            }

            int dist = ScoreManager.instance.GetDistanceMeters();
            if (distanceText != null && dist != lastDistance)
            {
                lastDistance = dist;
                distanceText.text = ScoreManager.instance.GetDistanceString();
            }
        }

        if (CoinManager.instance != null && coinText != null)
        {
            int coins = CoinManager.instance.GetCoinsThisRun();
            if (coins != lastCoins)
            {
                lastCoins = coins;
                coinText.text = "COINS: " + coins;
            }
        }

        if (PhaseManager.instance != null && phaseText != null)
        {
            string phase = PhaseManager.instance.GetPhaseName();
            if (phase != lastPhase)
            {
                lastPhase = phase;
                phaseText.text = phase;
            }
        }

        if (ComboManager.instance != null && comboText != null)
        {
            int combo = ComboManager.instance.GetCombo();
            if (combo != lastCombo)
            {
                lastCombo = combo;
                comboText.text = "x" + combo;
            }
        }
    }
}
