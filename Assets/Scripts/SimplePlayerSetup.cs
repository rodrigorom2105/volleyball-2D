using UnityEngine;

public class SimplePlayerSetup : MonoBehaviour
{
    void Awake()
    {
        GameObject p1 = GameObject.Find("Player1");
        GameObject p2 = GameObject.Find("Player2");

        if (p1 != null) ApplySprite(p1);
        if (p2 != null) ApplySprite(p2);
    }

    void ApplySprite(GameObject player)
    {
        // Buscar sprite sheet - primero intenta multiple frames, luego single sprite
        Sprite[] sheetSprites = Resources.LoadAll<Sprite>("PlayerSheet");
        Sprite singleSprite = Resources.Load<Sprite>("Player");

        SpriteRenderer sr = player.GetComponent<SpriteRenderer>();
        if (sr == null) sr = player.AddComponent<SpriteRenderer>();

        if (sheetSprites.Length > 0)
        {
            sr.sprite = sheetSprites[0];
            Debug.Log($"{player.name}: Cargando frame 0 de PlayerSheet ({sheetSprites.Length} frames totales)");

            // Agregar animator si hay multiples frames
            if (sheetSprites.Length > 1 && player.GetComponent<PlayerSpriteAnimator>() == null)
            {
                player.AddComponent<PlayerSpriteAnimator>();
            }
        }
        else if (singleSprite != null)
        {
            sr.sprite = singleSprite;
            Debug.Log($"{player.name}: Usando sprite único Player.png");
        }
        else
        {
            Debug.LogWarning($"{player.name}: No encontré sprites. Coloca PlayerSheet.png o Player.png en Assets/Resources/");
        }

        sr.color = Color.white;
        sr.sortingOrder = 5;
    }
}
