using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 6f;
    public float sprintSpeed = 10f;
    public float jumpForce = 10f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.15f;
    public LayerMask groundLayer;

    // Expostar para animaciones
    public bool IsGrounded { get; private set; }

    [Header("Controls")]
    public KeyCode leftKey;
    public KeyCode rightKey;
    public KeyCode jumpKey;
    public KeyCode sprintKey; // 🔥 NUEVO

    [Header("Stamina")]
    public float maxStamina = 100f;
    public float staminaDrain = 25f;
    public float staminaRegen = 15f;

    private float currentStamina;
    private bool isSprinting;

    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentStamina = maxStamina;
    }

    void Update()
    {
        CheckGround();
        Move();
        Jump();
        HandleStamina();
    }

    void Move()
    {
        float move = 0f;

        if (Input.GetKey(leftKey)) move = -1f;
        if (Input.GetKey(rightKey)) move = 1f;

        // 🔥 Sprint logic
        isSprinting = Input.GetKey(sprintKey) && currentStamina > 0 && move != 0;

        float currentSpeed = isSprinting ? sprintSpeed : speed;

        rb.linearVelocity = new Vector2(move * currentSpeed, rb.linearVelocity.y);
    }

    void Jump()
    {
        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void HandleStamina()
    {
        if (isSprinting)
        {
            currentStamina -= staminaDrain * Time.deltaTime;
        }
        else
        {
            currentStamina += staminaRegen * Time.deltaTime;
        }

        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
    }

    void CheckGround()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius);
        isGrounded = System.Array.Exists(hits, h => h.gameObject != gameObject);
        IsGrounded = isGrounded;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody2D ballRb = collision.gameObject.GetComponent<Rigidbody2D>();

            Vector2 direction = (collision.transform.position - transform.position).normalized;

            float force = 18f;  // Aumentado de 10f para más potencia

            ballRb.AddForce(direction * force, ForceMode2D.Impulse);
        }
    }

    // 🔹 Para UI
    public float GetStaminaNormalized()
    {
        return currentStamina / maxStamina;
    }

    // Solo para visualizar el ground check
    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}