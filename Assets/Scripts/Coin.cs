using UnityEngine;

public class Coin : MonoBehaviour
{
    public float rotateSpeed  = 180f;
    public float magnetSpeed  = 18f;

    private Transform ball;

    void Start()
    {
        BallController b = FindObjectOfType<BallController>();
        if (b != null) ball = b.transform;
    }

    void Update()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);

        if (ball != null && PowerUpManager.instance != null && PowerUpManager.instance.IsMagnetActive)
            transform.position = Vector3.MoveTowards(transform.position, ball.position, magnetSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CoinManager.instance.AddCoin();
            Destroy(gameObject);
        }
    }
}