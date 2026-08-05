using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    private float score = 0f;
    private bool isActive = true;
    private float distanceMeters = 0f;
    private Transform ball;

    // Cached so GetBestScore() never reads PlayerPrefs per-frame
    private int cachedBestScore;
    private int cachedBestDistance;

    void Awake()
    {
        instance = this;
        cachedBestScore    = PlayerPrefs.GetInt("BestScore",    0);
        cachedBestDistance = PlayerPrefs.GetInt("BestDistance", 0);
    }

    void Start()
    {
        if (BallController.instance != null) ball = BallController.instance.transform;
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
        if (current > cachedBestScore)
        {
            cachedBestScore = current;
            PlayerPrefs.SetInt("BestScore", cachedBestScore);
            PlayerPrefs.Save();
        }
        int dist = GetDistanceMeters();
        if (dist > cachedBestDistance)
        {
            cachedBestDistance = dist;
            PlayerPrefs.SetInt("BestDistance", cachedBestDistance);
            PlayerPrefs.Save();
        }
    }

    public int GetScore()          { return Mathf.FloorToInt(score); }
    public int GetBestScore()      { return cachedBestScore; }          // no PlayerPrefs read
    public int GetDistanceMeters() { return Mathf.FloorToInt(distanceMeters); }
    public string GetDistanceString()
    {
        int d = GetDistanceMeters();
        return d >= 1000 ? (d / 1000f).ToString("F1") + "km" : d + "m";
    }
}
