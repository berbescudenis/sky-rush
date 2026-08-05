using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    private const float destroyDistance = 40f;

    void Update()
    {
        if (BallController.instance != null &&
            transform.position.z < BallController.instance.transform.position.z - destroyDistance)
        {
            Destroy(gameObject);
        }
    }
}
