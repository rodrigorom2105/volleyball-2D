// using UnityEngine;
// using UnityEngine.SceneManagement;

// public class BallController : MonoBehaviour
// {
//     private Rigidbody2D rb;

//     public ScoreManager scoreManager;

//     void Start()
//     {
//         rb = GetComponent<Rigidbody2D>();
//         ResetBall();
//     }

//     void OnCollisionEnter2D(Collision2D collision)
//     {
//         if (collision.gameObject.CompareTag("GroundLeft"))
//         {
//             scoreManager.Player2Score();
//             ResetBall();
//         }

//         if (collision.gameObject.CompareTag("GroundRight"))
//         {
//             scoreManager.Player1Score();
//             ResetBall();
//         }
//     }

//     void ResetBall()
//     {
//         transform.position = new Vector2(0, 2);
//         rb.linearVelocity = new Vector2(Random.Range(-3f, 3f), 5f);
//     }
// }
using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody2D rb;

    public ScoreManager scoreManager;

    [Header("Ball Settings")]
    public float initialHorizontalRange = 1.5f;
    public float initialVerticalForce = 3f;
    public float gravityScale = 3.5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale; // aplicar gravedad al inicio
        ResetBall();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GroundLeft"))
        {
            scoreManager.Player2Score();
            ResetBall();
        }

        if (collision.gameObject.CompareTag("GroundRight"))
        {
            scoreManager.Player1Score();
            ResetBall();
        }
    }

    void ResetBall()
    {
        transform.position = new Vector2(0, 2);
        rb.linearVelocity = new Vector2(
            Random.Range(-initialHorizontalRange, initialHorizontalRange),
            initialVerticalForce
        );
    }
}