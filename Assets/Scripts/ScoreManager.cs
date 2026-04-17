using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int player1Score = 0;
    public int player2Score = 0;

    public TMP_Text player1Text;
    public TMP_Text player2Text;

    public GameObject winPanel;
    public TMP_Text winText;

    public int maxScore = 10;
    public GameObject controlsPanel;

    private bool gameEnded = false;
    private bool gameStarted = false;
    private bool isPaused = false;
    private GameObject pausePanel;

    void Awake()
    {
        PositionPlayers();
        SetupPlayerColliders();
        SetupSideBounds();
    }

    void Start()
    {
        CreateControlsPanel();
        CreatePausePanel();

        if (controlsPanel != null)
        {
            controlsPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    void Update()
    {
        if (gameStarted && !gameEnded && Input.GetKeyDown(KeyCode.P))
            TogglePause();
    }

    void CreateControlsPanel()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null) return;

        GameObject panelObj = new GameObject("ControlsPanel");
        panelObj.transform.SetParent(canvas.transform, false);

        RectTransform rect = panelObj.AddComponent<RectTransform>();
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = new Vector2(1000, 750);

        Image bg = panelObj.AddComponent<Image>();
        bg.color = new Color(0.1f, 0.1f, 0.1f, 0.95f);

        GameObject mainContent = new GameObject("MainContent");
        mainContent.transform.SetParent(panelObj.transform, false);

        RectTransform mainRect = mainContent.AddComponent<RectTransform>();
        mainRect.anchoredPosition = Vector2.zero;
        mainRect.sizeDelta = new Vector2(950, 700);

        VerticalLayoutGroup mainVlg = mainContent.AddComponent<VerticalLayoutGroup>();
        mainVlg.childForceExpandHeight = false;
        mainVlg.childForceExpandWidth = false;
        mainVlg.spacing = 35;
        mainVlg.padding = new RectOffset(60, 60, 45, 45);

        // Títulos
        AddText(mainContent, "Controls", 58, Color.white, 70);
        AddText(mainContent, "Learn the controls before playing", 26, new Color(0.6f, 0.6f, 0.6f), 40);

        // Contenedor de jugadores
        GameObject playersContainer = new GameObject("PlayersContainer");
        playersContainer.transform.SetParent(mainContent.transform, false);

        RectTransform playersRect = playersContainer.AddComponent<RectTransform>();
        playersRect.sizeDelta = new Vector2(850, 280);

        HorizontalLayoutGroup hlg = playersContainer.AddComponent<HorizontalLayoutGroup>();
        hlg.childForceExpandHeight = false;
        hlg.childForceExpandWidth = true;
        hlg.spacing = 50;

        // Player 1
        CreatePlayerCard(playersContainer, "Player 1", new Color(0.15f, 0.4f, 0.7f, 1),
            "A / D - Move\nW - Jump\nS - Sprint");

        // Player 2
        CreatePlayerCard(playersContainer, "Player 2", new Color(0.9f, 0.6f, 0.15f, 1),
            "← / → - Move\n↑ - Jump\n↓ - Sprint");

        // Botón inicio (centrado)
        GameObject btnObj = new GameObject("StartButton");
        btnObj.transform.SetParent(mainContent.transform, false);

        RectTransform btnRect = btnObj.AddComponent<RectTransform>();
        btnRect.anchoredPosition = Vector2.zero;
        btnRect.sizeDelta = new Vector2(380, 75);

        Image btnImg = btnObj.AddComponent<Image>();
        btnImg.color = new Color(0.15f, 0.15f, 0.15f, 1);

        Button btn = btnObj.AddComponent<Button>();
        btn.targetGraphic = btnImg;

        ColorBlock colors = btn.colors;
        colors.normalColor = new Color(0.15f, 0.15f, 0.15f, 1);
        colors.highlightedColor = new Color(0.25f, 0.25f, 0.25f, 1);
        btn.colors = colors;

        GameObject btnText = new GameObject("Text");
        btnText.transform.SetParent(btnObj.transform, false);
        TextMeshProUGUI btnTmp = btnText.AddComponent<TextMeshProUGUI>();
        btnTmp.text = "Start game →";
        btnTmp.fontSize = 40;
        btnTmp.color = Color.white;
        btnTmp.alignment = TextAlignmentOptions.Center;

        btn.onClick.AddListener(StartGame);
        controlsPanel = panelObj;
    }

    void CreatePlayerCard(GameObject parent, string playerName, Color cardColor, string controls)
    {
        GameObject card = new GameObject("Card");
        card.transform.SetParent(parent.transform, false);

        RectTransform cardRect = card.AddComponent<RectTransform>();
        cardRect.sizeDelta = new Vector2(380, 270);

        Image cardImg = card.AddComponent<Image>();
        cardImg.color = cardColor;

        VerticalLayoutGroup cardVlg = card.AddComponent<VerticalLayoutGroup>();
        cardVlg.childForceExpandHeight = false;
        cardVlg.childForceExpandWidth = false;
        cardVlg.spacing = 15;
        cardVlg.padding = new RectOffset(25, 25, 20, 20);

        AddText(card, playerName, 36, Color.white, 50);
        AddText(card, controls, 26, Color.white, 140);
    }

    void CreatePausePanel()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null) return;

        GameObject panelObj = new GameObject("PausePanel");
        panelObj.transform.SetParent(canvas.transform, false);

        RectTransform rect = panelObj.AddComponent<RectTransform>();
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = new Vector2(700, 350);

        Image bg = panelObj.AddComponent<Image>();
        bg.color = new Color(0.05f, 0.05f, 0.15f, 0.92f);

        GameObject content = new GameObject("Content");
        content.transform.SetParent(panelObj.transform, false);

        RectTransform contentRect = content.AddComponent<RectTransform>();
        contentRect.anchoredPosition = Vector2.zero;
        contentRect.sizeDelta = new Vector2(500, 200);

        VerticalLayoutGroup vlg = content.AddComponent<VerticalLayoutGroup>();
        vlg.childForceExpandHeight = false;
        vlg.childForceExpandWidth = false;
        vlg.spacing = 30;
        vlg.padding = new RectOffset(30, 30, 30, 30);

        AddText(content, "JUEGO PAUSADO", 52, Color.yellow, 70);
        AddText(content, "Presiona P para reanudar", 32, new Color(0.8f, 0.9f, 1f), 50);

        pausePanel = panelObj;
        pausePanel.SetActive(false);
    }

    GameObject AddText(GameObject parent, string content, int fontSize, Color color, int height)
    {
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(parent.transform, false);

        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.sizeDelta = new Vector2(800, height);

        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = content;
        tmp.fontSize = fontSize;
        tmp.color = color;
        tmp.alignment = TextAlignmentOptions.Center;

        LayoutElement layout = textObj.AddComponent<LayoutElement>();
        layout.preferredHeight = height;

        return textObj;
    }

    public void StartGame()
    {
        if (controlsPanel != null)
            controlsPanel.SetActive(false);

        Time.timeScale = 1f;
        gameStarted = true;
        isPaused = false;
    }

    void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            if (pausePanel != null)
                pausePanel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            if (pausePanel != null)
                pausePanel.SetActive(false);
        }
    }

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
            EndGame("Player 1 Wins!");
        else if (player2Score >= maxScore)
            EndGame("Player 2 Wins!");
    }

    void EndGame(string message)
    {
        gameEnded = true;
        winPanel.SetActive(true);
        winText.text = message;
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void PositionPlayers()
    {
        GameObject player1 = GameObject.Find("Player1");
        GameObject player2 = GameObject.Find("Player2");

        if (player1 != null)
        {
            player1.transform.position = new Vector3(-6f, 1.5f, 0f);
            SpriteRenderer sr1 = player1.GetComponent<SpriteRenderer>();
            if (sr1 != null) sr1.sortingOrder = -1;
            Debug.Log($"✓ Player1 posicionado en {player1.transform.position}, sortingOrder=-1");
        }
        else
            Debug.LogError("❌ Player1 no encontrado");

        if (player2 != null)
        {
            player2.transform.position = new Vector3(6f, 1.5f, 0f);
            SpriteRenderer sr2 = player2.GetComponent<SpriteRenderer>();
            if (sr2 != null) sr2.sortingOrder = -1;
            Debug.Log($"✓ Player2 posicionado en {player2.transform.position}, sortingOrder=-1");
        }
        else
            Debug.LogError("❌ Player2 no encontrado");

        if (player1 != null) AdjustGroundCheck(player1);
        if (player2 != null) AdjustGroundCheck(player2);
    }

    void AdjustGroundCheck(GameObject player)
    {
        Transform groundCheck = player.transform.Find("GroundCheck");
        if (groundCheck == null)
        {
            GameObject gc = new GameObject("GroundCheck");
            gc.transform.SetParent(player.transform);
            groundCheck = gc.transform;
        }
        groundCheck.localPosition = new Vector3(0f, -1.0f, 0f);
        Debug.Log($"✓ {player.name} GroundCheck en {groundCheck.localPosition}");
    }

    void SetupPlayerColliders()
    {
        Sprite playerSprite = Resources.Load<Sprite>("Player");
        if (playerSprite == null)
            Debug.LogWarning("No se encontro sprite en Assets/Resources/Player.png - los jugadores seguiran como rectangulos");

        GameObject player1 = GameObject.Find("Player1");
        GameObject player2 = GameObject.Find("Player2");

        if (player1 != null) SetupPlayer(player1, playerSprite);
        if (player2 != null) SetupPlayer(player2, playerSprite);
    }

    void SetupPlayer(GameObject player, Sprite sprite)
    {
        player.transform.localScale = new Vector3(0.5f, 0.5f, 1f);

        PlayerAnimator oldAnim = player.GetComponent<PlayerAnimator>();
        if (oldAnim != null) DestroyImmediate(oldAnim);

        SetupAnimations oldSetup = player.GetComponent<SetupAnimations>();
        if (oldSetup != null) DestroyImmediate(oldSetup);

        // SpriteRenderer sin deformación
        SpriteRenderer sr = player.GetComponent<SpriteRenderer>();
        if (sr == null) sr = player.AddComponent<SpriteRenderer>();
        sr.color = Color.white;
        sr.drawMode = SpriteDrawMode.Simple;  // Usa tamaño natural del sprite
        sr.sortingOrder = 5;
        sr.flipX = false;

        if (sprite != null)
        {
            sr.sprite = sprite;
            // Log para debug
            if (sprite.bounds.size != Vector3.zero)
                Debug.Log($"{player.name} sprite: {sprite.bounds.size}, PPU: {sprite.pixelsPerUnit}");
        }

        // Animator para sprite sheet
        PlayerSpriteAnimator anim = player.GetComponent<PlayerSpriteAnimator>();
        if (anim == null) anim = player.AddComponent<PlayerSpriteAnimator>();

        // Quitar BoxCollider si existe
        BoxCollider2D box = player.GetComponent<BoxCollider2D>();
        if (box != null) DestroyImmediate(box);

        // Agregar CapsuleCollider base (será ajustado por SpriteColliderFitter)
        CapsuleCollider2D cap = player.GetComponent<CapsuleCollider2D>();
        if (cap == null)
        {
            cap = player.AddComponent<CapsuleCollider2D>();
            cap.size = new Vector2(0.8f, 1.8f);
            cap.offset = Vector2.zero;
            cap.direction = CapsuleDirection2D.Vertical;
        }

        // SpriteColliderFitter removido para mantener escala personalizada

        Debug.Log($"✓ {player.name}: Sprite={sr.sprite?.name}, Collider automático activado");
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
        wall.transform.position = new Vector3(xPos, 5, 0);

        BoxCollider2D col = wall.AddComponent<BoxCollider2D>();
        col.size = new Vector2(0.5f, 30);

        Rigidbody2D rb = wall.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
    }
}
