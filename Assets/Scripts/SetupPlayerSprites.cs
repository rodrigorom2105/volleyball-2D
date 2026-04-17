using UnityEngine;

public class SetupPlayerSprites : MonoBehaviour
{
    [Header("Sprite del personaje")]
    [Tooltip("Arrastra aqui el sprite guardado en Assets/Sprites/Player.png")]
    public Sprite playerSprite;

    [Header("Ajustes de Collider (CapsuleCollider2D)")]
    public Vector2 colliderSize = new Vector2(0.7f, 1.4f);
    public Vector2 colliderOffset = new Vector2(0f, 0f);
    public CapsuleDirection2D colliderDirection = CapsuleDirection2D.Vertical;

    [Header("Escala visual del sprite")]
    public Vector3 playerScale = new Vector3(0.5f, 0.5f, 1f);

    [Header("Colores por jugador (tint opcional)")]
    public Color player1Tint = Color.white;
    public Color player2Tint = Color.white;

    void Awake()
    {
        if (playerSprite == null)
        {
            playerSprite = Resources.Load<Sprite>("Player");
            if (playerSprite == null)
            {
                Debug.LogWarning("No se encontro el sprite. Asignalo en el Inspector o guardalo en Assets/Resources/Player.png");
            }
        }

        ApplyToPlayer("Player1", player1Tint);
        ApplyToPlayer("Player2", player2Tint);
    }

    void ApplyToPlayer(string playerName, Color tint)
    {
        GameObject player = GameObject.Find(playerName);
        if (player == null)
        {
            Debug.LogWarning($"{playerName} no encontrado");
            return;
        }

        // Escala
        player.transform.localScale = playerScale;

        // SpriteRenderer
        SpriteRenderer sr = player.GetComponent<SpriteRenderer>();
        if (sr == null) sr = player.AddComponent<SpriteRenderer>();

        if (playerSprite != null)
        {
            sr.sprite = playerSprite;
        }
        sr.color = tint;
        sr.sortingOrder = 5;

        // Reemplazar BoxCollider2D por CapsuleCollider2D si existe
        BoxCollider2D box = player.GetComponent<BoxCollider2D>();
        if (box != null) DestroyImmediate(box);

        CapsuleCollider2D cap = player.GetComponent<CapsuleCollider2D>();
        if (cap == null) cap = player.AddComponent<CapsuleCollider2D>();

        cap.size = colliderSize;
        cap.offset = colliderOffset;
        cap.direction = colliderDirection;

        Debug.Log($"Sprite y collider aplicados a {playerName}");
    }
}