using UnityEngine;

public class BallController : MonoBehaviour
{
    public float startSpeed = 10f;
    public float maxSpeed = 50f;
    public float speedIncreaseRate = 0.8f;
    public float laneDistance = 2f;
    public float laneSwitchSpeed = 8f;
    public float jumpForce = 7f;
    public float extraGravity = 15f;

    [HideInInspector] public float currentSpeed;
    private int currentLane = 1;
    private float targetX;
    private Rigidbody rb;
    private bool isDead = false;
    private TrailRenderer trail;
    private bool isGrounded = false;

    [SerializeField] private UnityEngine.UI.Image flashOverlay;
    public Material[] ballMaterials;

    private Vector2 touchStart;
    private bool isSwiping = false;
    private float swipeThreshold = 50f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetX = 0f;
        currentSpeed = startSpeed;
        trail = GetComponent<TrailRenderer>();
        ApplyBallVisuals();
    }

    void ApplyBallVisuals()
    {
        int index = PlayerPrefs.GetInt("SelectedBall", 0);
        if (ballMaterials == null || index >= ballMaterials.Length) return;

        Renderer r = GetComponent<Renderer>();
        if (r != null) r.material = ballMaterials[index];

        if (trail != null && ballMaterials[index] != null)
        {
            Color c = ballMaterials[index].color;
            trail.startColor = new Color(c.r, c.g, c.b, 1f);
            trail.endColor   = new Color(c.r, c.g, c.b, 0f);
        }
    }

    void Update()
    {
        if (isDead) return;

        if (transform.position.y < -10f)
            GameManager.instance.GameOver();

        currentSpeed = Mathf.Min(currentSpeed + speedIncreaseRate * Time.deltaTime, maxSpeed);

        if (Input.GetKeyDown(KeyCode.LeftArrow) && currentLane > 0)
        {
            currentLane--;
            targetX = (currentLane - 1) * laneDistance;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && currentLane < 2)
        {
            currentLane++;
            targetX = (currentLane - 1) * laneDistance;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStart = touch.position;
                isSwiping = true;
            }
            else if (touch.phase == TouchPhase.Moved && isSwiping)
            {
                Vector2 swipeDelta = touch.position - touchStart;
                if (Mathf.Abs(swipeDelta.x) > swipeThreshold)
                {
                    if (swipeDelta.x > 0 && currentLane < 2)
                    {
                        currentLane++;
                        targetX = (currentLane - 1) * laneDistance;
                    }
                    else if (swipeDelta.x < 0 && currentLane > 0)
                    {
                        currentLane--;
                        targetX = (currentLane - 1) * laneDistance;
                    }
                    isSwiping = false;
                }
            }
            else if (touch.phase == TouchPhase.Ended && isSwiping)
            {
                Vector2 swipeDelta = touch.position - touchStart;
                if (Mathf.Abs(swipeDelta.x) < swipeThreshold && isGrounded)
                {
                    Jump();
                }
                isSwiping = false;
            }
        }

        Vector3 pos = transform.position;
        pos.x = Mathf.Lerp(pos.x, targetX, Time.deltaTime * laneSwitchSpeed);
        transform.position = pos;

        if (trail != null)
        {
            float speedPercent = currentSpeed / maxSpeed;
            trail.time = Mathf.Lerp(0.1f, 0.5f, speedPercent);
        }
    }

    void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        isGrounded = false;
        PlayerPrefs.SetInt("TotalJumps", PlayerPrefs.GetInt("TotalJumps", 0) + 1);
    }

    void FixedUpdate()
    {
        if (isDead) return;
        rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, currentSpeed);

        if (!isGrounded)
        {
            rb.AddForce(Vector3.down * extraGravity, ForceMode.Acceleration);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    public void TeleportToLane(int lane)
    {
        currentLane = lane;
        targetX = (lane - 1) * laneDistance;
        rb.position = new Vector3(targetX, rb.position.y, rb.position.z + 8f);
    }

    public void TeleportToLaneAtZ(int lane, float zPosition)
    {
        currentLane = lane;
        targetX = (lane - 1) * laneDistance;
        rb.position = new Vector3(targetX, rb.position.y, zPosition);
    }

    public void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector3.zero;
        rb.AddForce(Vector3.up * 8f, ForceMode.Impulse);
        if (trail != null) trail.enabled = false;
        StartCoroutine(FlashEffect());
    }

    System.Collections.IEnumerator FlashEffect()
    {
        UnityEngine.UI.Image flash = flashOverlay != null
            ? flashOverlay
            : FindObjectOfType<UnityEngine.UI.Image>();
        if (flash == null) yield break;

        float elapsed = 0f;
        float duration = 0.15f;
        Color c = Color.white;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            c.a = Mathf.Lerp(0.8f, 0f, elapsed / duration);
            flash.color = c;
            yield return null;
        }
        c.a = 0f;
        flash.color = c;
    }

    public float GetSpeed() { return currentSpeed; }
}