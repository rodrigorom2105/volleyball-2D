using UnityEngine;

public class SpriteColliderFitter : MonoBehaviour
{
    [Header("Escala máxima permitida")]
    [Tooltip("Si el sprite es muy grande, escalarlo para que no exceda esta altura")]
    public float maxPlayerHeight = 4.66f;

    [Header("Proporción del collider respecto al sprite")]
    [Range(0.8f, 1f)]
    public float colliderWidthPercent = 0.85f;

    [Range(0.8f, 1f)]
    public float colliderHeightPercent = 0.9f;

    void Start()
    {
        FitSpriteAndCollider();
    }

    void FitSpriteAndCollider()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null || sr.sprite == null)
        {
            Debug.LogWarning($"{name}: No hay SpriteRenderer o sprite");
            return;
        }

        // Obtener tamaño real del sprite (en unidades del mundo)
        Bounds spriteBounds = sr.sprite.bounds;
        Vector3 spriteSize = spriteBounds.size;

        Debug.Log($"{name}: Sprite size = {spriteSize}, PPU = {sr.sprite.pixelsPerUnit}");

        // Si el sprite es demasiado grande, escalar el player
        if (spriteSize.y > maxPlayerHeight)
        {
            float scale = maxPlayerHeight / spriteSize.y;
            transform.localScale = new Vector3(scale, scale, 1f);
            spriteSize *= scale;
            Debug.Log($"{name}: Escalado a {scale:F2}x para caber en altura máxima");
        }
        else
        {
            transform.localScale = Vector3.one;
        }

        // Ajustar collider al tamaño real del sprite
        CapsuleCollider2D cap = GetComponent<CapsuleCollider2D>();
        if (cap != null)
        {
            cap.size = new Vector2(spriteSize.x * colliderWidthPercent, spriteSize.y * colliderHeightPercent);
            cap.offset = new Vector2(spriteBounds.center.x, spriteBounds.center.y - spriteSize.y * 0.05f);
            cap.direction = CapsuleDirection2D.Vertical;

            Debug.Log($"{name}: Collider ajustado a {cap.size}, offset {cap.offset}");
        }
    }
}