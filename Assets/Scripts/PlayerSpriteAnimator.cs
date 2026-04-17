using UnityEngine;

public class PlayerSpriteAnimator : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private PlayerController controller;
    private Sprite[] allSprites;
    private Sprite fallbackSprite;

    [Header("Frame Indices from Sprite Sheet")]
    [Tooltip("Posición idle/parado")]
    public int idleFrame = 0;

    [Tooltip("Posición running/moviendo")]
    public int runningFrame = 1;

    [Tooltip("Subiendo durante salto")]
    public int jumpingUpFrame = 2;

    [Tooltip("Pico del salto")]
    public int jumpPeakFrame = 4;

    [Tooltip("Cayendo durante salto")]
    public int fallingFrame = 6;

    private int currentFrame = 0;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<PlayerController>();

        // Guardar sprite actual como fallback
        fallbackSprite = spriteRenderer.sprite;

        // Intentar cargar sprite sheet
        allSprites = Resources.LoadAll<Sprite>("PlayerSheet");
        if (allSprites.Length > 0)
        {
            Debug.Log($"✓ {name}: {allSprites.Length} frames cargados");
        }
    }

    void Update()
    {
        if (controller == null) return;

        bool isGrounded = controller.IsGrounded;
        bool isMoving = rb.linearVelocity.x != 0;
        float velocityY = rb.linearVelocity.y;

        // Seleccionar frame según estado
        if (!isGrounded)
        {
            if (velocityY > 1.5f)
                currentFrame = jumpingUpFrame;
            else if (velocityY > -1f)
                currentFrame = jumpPeakFrame;
            else
                currentFrame = fallingFrame;
        }
        else if (isMoving)
        {
            currentFrame = runningFrame;
        }
        else
        {
            currentFrame = idleFrame;
        }

        // Aplicar frame
        if (allSprites != null && allSprites.Length > 0 && currentFrame < allSprites.Length && allSprites[currentFrame] != null)
        {
            spriteRenderer.sprite = allSprites[currentFrame];
        }
        else if (fallbackSprite != null)
        {
            spriteRenderer.sprite = fallbackSprite;
        }
    }
}
