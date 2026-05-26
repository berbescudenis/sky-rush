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

    void Update()
    {
        if (ScoreManager.instance != null)
        {
            if (scoreText != null)    scoreText.text    = ScoreManager.instance.GetScore().ToString();
            if (bestScoreText != null) bestScoreText.text = "BEST: " + ScoreManager.instance.GetBestScore();
            if (distanceText != null) distanceText.text = ScoreManager.instance.GetDistanceString();
        }
        if (CoinManager.instance != null && coinText != null)
            coinText.text = "COINS: " + CoinManager.instance.GetCoinsThisRun();
        if (PhaseManager.instance != null && phaseText != null)
            phaseText.text = PhaseManager.instance.GetPhaseName();
        if (ComboManager.instance != null && comboText != null)
            comboText.text = "x" + ComboManager.instance.GetCombo();
    }
}