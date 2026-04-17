using UnityEngine;

public class PlayerPositioner : MonoBehaviour
{
    [Header("Posicionamiento")]
    [Tooltip("Altura a la que se posicionan los jugadores sobre la plataforma")]
    public float playerHeightAboveGround = -2.34f;

    [Tooltip("Distancia lateral desde el centro")]
    public float player1XPos = -6.1f;
    public float player2XPos = 6.1f;

    void Awake()
    {
        PositionPlayers();
    }

    void PositionPlayers()
    {
        GameObject player1 = GameObject.Find("Player1");
        GameObject player2 = GameObject.Find("Player2");

        if (player1 != null)
        {
            Vector3 pos = player1.transform.position;
            player1.transform.position = new Vector3(player1XPos, playerHeightAboveGround, pos.z);
            Debug.Log($"✓ {player1.name} posicionado en ({player1XPos}, {playerHeightAboveGround})");
        }

        if (player2 != null)
        {
            Vector3 pos = player2.transform.position;
            player2.transform.position = new Vector3(player2XPos, playerHeightAboveGround, pos.z);
            Debug.Log($"✓ {player2.name} posicionado en ({player2XPos}, {playerHeightAboveGround})");
        }

        // Ajustar groundCheck para detectar correctamente
        AdjustGroundChecks();
    }

    void AdjustGroundChecks()
    {
        GameObject player1 = GameObject.Find("Player1");
        GameObject player2 = GameObject.Find("Player2");

        if (player1 != null) SetGroundCheckPosition(player1);
        if (player2 != null) SetGroundCheckPosition(player2);
    }

    void SetGroundCheckPosition(GameObject player)
    {
        Transform groundCheck = player.transform.Find("GroundCheck");
        if (groundCheck == null)
        {
            Debug.LogWarning($"{player.name} no tiene GroundCheck hijo");
            return;
        }

        // Posicionar groundCheck debajo del jugador
        // Aproximadamente 0.6 unidades abajo del centro
        groundCheck.localPosition = new Vector3(0, -0.6f, 0);
        Debug.Log($"✓ {player.name} GroundCheck ajustado a posición relativa (0, -0.6)");
    }
}