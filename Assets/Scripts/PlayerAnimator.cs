using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private PlayerController controller;

    [SerializeField] private Sprite[] jumpSprites = new Sprite[8];
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite readySprite;

    private float animationSpeed = 0.1f;
    private float animationTimer = 0f;
    private int currentFrame = 0;
    private bool wasGrounded = true;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<PlayerController>();
        
    }

    void Update()
    {
        bool isGrounded = IsGrounded();
        bool isMoving = rb.linearVelocity.x != 0;

        if (!isGrounded)
        {
            // En el aire - animar salto
            UpdateJumpAnimation();
        }
        else if (isMoving)
        {
            // En el suelo moviéndose - frame listo para recibir
            spriteRenderer.sprite = readySprite ?? jumpSprites[1];
        }
        else
        {
            // En el suelo quieto - idle
            spriteRenderer.sprite = idleSprite ?? jumpSprites[0];
        }

        wasGrounded = isGrounded;
    }

    void UpdateJumpAnimation()
    {
        animationTimer += Time.deltaTime;

        if (animationTimer >= animationSpeed)
        {
            animationTimer = 0f;

            // Seleccionar frame basado en velocidad vertical
            float velocityY = rb.linearVelocity.y;

            if (velocityY > 3f)
            {
                currentFrame = 3; // Ascendiendo rápido
            }
            else if (velocityY > 1f)
            {
                currentFrame = 4; // Ascendiendo lento
            }
            else if (velocityY > -1f)
            {
                currentFrame = 5; // Pico del salto
            }
            else if (velocityY > -3f)
            {
                currentFrame = 6; // Cayendo lento
            }
            else
            {
                currentFrame = 7; // Cayendo rápido
            }

            if (jumpSprites.Length > currentFrame && jumpSprites[currentFrame] != null)
            {
                spriteRenderer.sprite = jumpSprites[currentFrame];
            }
        }
    }

    bool IsGrounded()
    {
        if (controller == null) return false;

        // Usar el groundCheck del PlayerController si está disponible
        return Physics2D.OverlapCircle(
            controller.groundCheck.position,
            controller.groundCheckRadius,
            controller.groundLayer
        );
    }
}
