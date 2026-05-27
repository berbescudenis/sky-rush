using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    private float score = 0f;
    private bool isActive = true;
    private float distanceMeters = 0f;
    private Transform ball;

    void Awake() { instance = this; }

    void Start()
    {
        BallController b = FindObjectOfType<BallController>();
        if (b != null) ball = b.transform;
    }

    void Update()
    {
        if (!isActive) return;
        float multiplier = ComboManager.instance != null ? ComboManager.instance.GetMultiplier() : 1f;
        score += Time.deltaTime * 10f * multiplier;

        if (ball != null)
            distanceMeters = Mathf.Max(distanceMeters, ball.position.z);
    }

    public void StopScore()
    {
        isActive = false;
        if (ComboManager.instance != null) ComboManager.instance.Stop();
        SaveBestScore();
    }

    void SaveBestScore()
    {
        int current = GetScore();
        int best = PlayerPrefs.GetInt("BestScore", 0);
        if (current > best)
        {
            PlayerPrefs.SetInt("BestScore", current);
            PlayerPrefs.Save();
        }
        int dist = GetDistanceMeters();
        int bestDist = PlayerPrefs.GetInt("BestDistance", 0);
        if (dist > bestDist)
        {
            PlayerPrefs.SetInt("BestDistance", dist);
            PlayerPrefs.Save();
        }
    }

    public int GetScore() { return Mathf.FloorToInt(score); }
    public int GetBestScore() { return PlayerPrefs.GetInt("BestScore", 0); }
    public int GetDistanceMeters() { return Mathf.FloorToInt(distanceMeters); }
    public string GetDistanceString()
    {
        int d = GetDistanceMeters();
        return d >= 1000 ? (d / 1000f).ToString("F1") + "km" : d + "m";
    }
}