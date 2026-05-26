using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    private Transform ball;
    private const float destroyDistance = 40f;

    void Start()
    {
        BallController b = FindObjectOfType<BallController>();
        if (b != null) ball = b.transform;
    }

    void Update()
    {
        if (ball != null && transform.position.z < ball.position.z - destroyDistance)
            Destroy(gameObject);
    }
}
