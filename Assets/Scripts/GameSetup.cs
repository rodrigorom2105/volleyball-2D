using UnityEngine;

public class GameSetup : MonoBehaviour
{
    void Start()
    {
        SetupPlayerColliders();
        SetupSideBounds();
    }

    void SetupPlayerColliders()
    {
        GameObject player1 = GameObject.Find("Player1");
        GameObject player2 = GameObject.Find("Player2");

        if (player1 != null) ReplaceCollider(player1);
        if (player2 != null) ReplaceCollider(player2);
    }

    void ReplaceCollider(GameObject player)
    {
        BoxCollider2D box = player.GetComponent<BoxCollider2D>();
        if (box != null) DestroyImmediate(box);

        CapsuleCollider2D cap = player.AddComponent<CapsuleCollider2D>();
        cap.size = new Vector2(0.5f, 1f);
    }

    void SetupSideBounds()
    {
        CreateWall("WallLeft", -12f);
        CreateWall("WallRight", 12f);
    }

    void CreateWall(string name, float xPos)
    {
        if (GameObject.Find(name) != null) return;

        GameObject wall = new GameObject(name);
        wall.transform.position = new Vector3(xPos, 0, 0);

        BoxCollider2D col = wall.AddComponent<BoxCollider2D>();
        col.size = new Vector2(1, 20);

        Rigidbody2D rb = wall.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
    }
}
