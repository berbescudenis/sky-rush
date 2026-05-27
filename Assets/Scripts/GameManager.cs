using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private bool isGameOver = false;

    void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
    }

    public void GameOver()
    {
        if (isGameOver) return;

        // Shield absorbs the hit — player survives
        if (PowerUpManager.instance != null && PowerUpManager.instance.ConsumeShield())
        {
            if (CameraShake.instance != null) CameraShake.instance.Shake(0.2f, 0.15f);
            return;
        }

        isGameOver = true;

        BallController ball = FindObjectOfType<BallController>();
        if (ball != null) ball.Die();

        if (ScoreManager.instance != null) ScoreManager.instance.StopScore();

        StartCoroutine(SlowMotionDeath());
    }

    System.Collections.IEnumerator SlowMotionDeath()
    {
        if (CameraShake.instance != null) CameraShake.instance.Shake(0.45f, 0.25f);

        Time.timeScale = 0.1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        yield return new WaitForSecondsRealtime(1.5f);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        yield return new WaitForSecondsRealtime(0.3f);

        GameOverUI goUI = FindObjectOfType<GameOverUI>(true);
        Debug.Log("GameOverUI found: " + (goUI != null));
        if (goUI != null)
            goUI.ShowGameOver();
        else
            SceneManager.LoadScene("MainMenu");
    }
}