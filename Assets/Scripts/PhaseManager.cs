using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    public static PhaseManager instance;
    public int currentPhase = 1;

    private int[] phaseThresholds = { 0, 200, 500, 1000, 2000, 3500 };
    private string[] phaseNames = {
        "STARTER",
        "ACCELERATE",
        "FLOW",
        "INTENSE",
        "EXTREME",
        "NO LIMITS"
    };

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (ScoreManager.instance == null) return;
        int score = ScoreManager.instance.GetScore();

        for (int i = phaseThresholds.Length - 1; i >= 0; i--)
        {
            if (score >= phaseThresholds[i])
            {
                if (currentPhase != i + 1)
                {
                    currentPhase = i + 1;
                    OnPhaseChanged(currentPhase);
                }
                break;
            }
        }
    }

    void OnPhaseChanged(int phase)
    {
        Debug.Log("PHASE " + phase + ": " + phaseNames[phase - 1]);

        // Check for power-up unlock before touching the spawner
        if (PowerUpManager.instance != null)
        {
            string unlocked = PowerUpManager.instance.UnlockForPhase(phase);
            if (unlocked != null)
                UnlockToast.instance?.ShowToast(unlocked + " UNLOCKED!");
        }

        ObstacleSpawner spawner = FindObjectOfType<ObstacleSpawner>();
        if (spawner == null) return;

        switch (phase)
        {
            case 1:
                spawner.spawnInterval = 20f;
                spawner.movingChance = 0f;
                spawner.canSpawnGaps = false;
                spawner.canSpawnBlades = false;
                spawner.canSpawnFakePaths = false;
                spawner.canSpawnHammers = false;
                spawner.canSpawnLasers = false;
                spawner.canSpawnPortals = false;
                break;
            case 2:
                spawner.spawnInterval = 16f;
                spawner.movingChance = 0.2f;
                spawner.canSpawnGaps = true;
                spawner.canSpawnBlades = false;
                spawner.canSpawnFakePaths = false;
                spawner.canSpawnHammers = false;
                spawner.canSpawnLasers = false;
                spawner.canSpawnPortals = false;
                break;
            case 3:
                spawner.spawnInterval = 13f;
                spawner.movingChance = 0.4f;
                spawner.canSpawnGaps = true;
                spawner.canSpawnBlades = true;
                spawner.canSpawnFakePaths = true;
                spawner.canSpawnHammers = false;
                spawner.canSpawnLasers = false;
                spawner.canSpawnPortals = false;
                break;
            case 4:
                spawner.spawnInterval = 10f;
                spawner.movingChance = 0.6f;
                spawner.canSpawnGaps = true;
                spawner.canSpawnBlades = true;
                spawner.canSpawnFakePaths = true;
                spawner.canSpawnHammers = true;
                spawner.canSpawnLasers = true;
                spawner.canSpawnPortals = false;
                break;
            case 5:
                spawner.spawnInterval = 7f;
                spawner.movingChance = 0.8f;
                spawner.canSpawnGaps = true;
                spawner.canSpawnBlades = true;
                spawner.canSpawnFakePaths = true;
                spawner.canSpawnHammers = true;
                spawner.canSpawnLasers = true;
                spawner.canSpawnPortals = true;
                break;
            case 6:
                spawner.spawnInterval = 5f;
                spawner.movingChance = 1f;
                spawner.canSpawnGaps = true;
                spawner.canSpawnBlades = true;
                spawner.canSpawnFakePaths = true;
                spawner.canSpawnHammers = true;
                spawner.canSpawnLasers = true;
                spawner.canSpawnPortals = true;
                break;
        }
    }

    public string GetPhaseName()
    {
        return phaseNames[currentPhase - 1];
    }
}