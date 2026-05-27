using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    public static GameOverUI instance;
    public GameObject gameOverPanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bestText;
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI distanceText;
    public TextMeshProUGUI comboText;

    void Awake()
    {
        instance = this;
        if (gameOverPanel != null)
        {
            RectTransform rt = gameOverPanel.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Debug.Log("GameOverPanel activated");
        }
        else
        {
            Debug.Log("GameOverPanel is NULL");
        }

        int score = ScoreManager.instance != null ? ScoreManager.instance.GetScore() : 0;
        int best = ScoreManager.instance != null ? ScoreManager.instance.GetBestScore() : 0;
        int coins = CoinManager.instance != null ? CoinManager.instance.GetCoinsThisRun() : 0;

        string distance = ScoreManager.instance != null ? ScoreManager.instance.GetDistanceString() : "0m";
        int combo = ComboManager.instance != null ? ComboManager.instance.GetCombo() : 1;

        if (scoreText != null)    scoreText.text    = "SCORE: " + score;
        if (bestText != null)     bestText.text     = "BEST: " + best;
        if (coinsText != null)    coinsText.text    = "COINS: " + coins;
        if (distanceText != null) distanceText.text = "DISTANCE: " + distance;
        if (comboText != null)    comboText.text    = "BEST COMBO: x" + combo;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        SceneManager.LoadScene("SampleScene");
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        SceneManager.LoadScene("MainMenu");
    }
}