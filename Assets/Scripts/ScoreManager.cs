using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public int player1Score = 0;
    public int player2Score = 0;

    public TMP_Text player1Text;
    public TMP_Text player2Text;

    public GameObject winPanel;
    public TMP_Text winText;

    public int maxScore = 10;

    private bool gameEnded = false;

    public void Player1Score()
    {
        if (gameEnded) return;

        player1Score++;
        UpdateScore();
        CheckWin();
    }

    public void Player2Score()
    {
        if (gameEnded) return;

        player2Score++;
        UpdateScore();
        CheckWin();
    }

    void UpdateScore()
    {
        player1Text.text = player1Score.ToString();
        player2Text.text = player2Score.ToString();
    }

    void CheckWin()
    {
        if (player1Score >= maxScore)
        {
            EndGame("Player 1 Wins!");
        }
        else if (player2Score >= maxScore)
        {
            EndGame("Player 2 Wins!");
        }
    }

    void EndGame(string message)
    {
        gameEnded = true;

        winPanel.SetActive(true);
        winText.text = message;

        Time.timeScale = 0f; // 🔥 pausa el juego
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}