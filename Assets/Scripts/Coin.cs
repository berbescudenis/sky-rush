using UnityEngine;

public class Coin : MonoBehaviour
{
    public float rotateSpeed = 180f;
    public float magnetSpeed = 18f;

    void Update()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);

        if (PowerUpManager.instance != null && PowerUpManager.instance.IsMagnetActive
            && BallController.instance != null)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                BallController.instance.transform.position,
                magnetSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CoinManager.instance.AddCoin();
            gameObject.SetActive(false);  // hide instantly — no GC spike here
            Destroy(gameObject, 3f);       // actual cleanup happens 3s later, off-screen
        }
    }
}
