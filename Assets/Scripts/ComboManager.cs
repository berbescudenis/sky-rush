using UnityEngine;

public class ComboManager : MonoBehaviour
{
    public static ComboManager instance;

    public float secondsPerLevel = 5f;
    public int maxCombo = 8;

    private int comboLevel = 1;
    private float timer = 0f;
    private bool isActive = true;

    void Awake() { instance = this; }

    void Update()
    {
        if (!isActive || comboLevel >= maxCombo) return;
        timer += Time.deltaTime;
        if (timer >= secondsPerLevel)
        {
            timer = 0f;
            comboLevel++;
        }
    }

    public void Stop() { isActive = false; }
    public int GetCombo() { return comboLevel; }
    public float GetMultiplier() { return comboLevel; }
}
