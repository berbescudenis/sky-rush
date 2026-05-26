using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 3, -8);
    public float normalFOV = 60f;
    public float maxFOV = 90f;

    private Camera cam;
    private BallController ball;

    void Start()
    {
        cam = GetComponent<Camera>();
        ball = FindObjectOfType<BallController>();
    }

    void LateUpdate()
    {
        if (target == null) return;

        transform.position = new Vector3(
            Mathf.Lerp(transform.position.x, target.position.x + offset.x, Time.deltaTime * 8f),
            target.position.y + offset.y,
            target.position.z + offset.z
        );

        // FOV increases with speed for intensity feeling
        if (ball != null && cam != null)
        {
            float speedPercent = ball.currentSpeed / ball.maxSpeed;
            cam.fieldOfView = Mathf.Lerp(normalFOV, maxFOV, speedPercent);
        }
    }
}