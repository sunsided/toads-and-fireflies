using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private float gameTime = 30;

    [SerializeField]
    private float gameOverCooldown = 5;

    [SerializeField]
    private TextMeshProUGUI[] playerScoreText;

    [SerializeField]
    private TextMeshProUGUI timerText;

    [SerializeField]
    private GameObject gameOverPanel;

    [SerializeField]
    private TextMeshProUGUI winnerText;

    [SerializeField]
    private Image cooldownBar;

    [SerializeField]
    private TextMeshProUGUI buttonToContinueText;

    [SerializeField]
    private string[] playerNames = { "Green Toad", "Red Toad" };

    private float _gameTime;

    private float _cooldownTime;

    private int[] _playerScores;

    private bool _gameRunning;

    public void AddPoints(int player, int amount)
    {
        _playerScores[player] += amount;
        UpdateUI();
    }

    private void Start()
    {
        RestartGame();
    }

    private void RestartGame()
    {
        // Remove all existing fireflies
        foreach (var spawner in FindObjectsOfType<FireflySpawner>())
        {
            spawner.RemoveAllFireflies();
        }

        // Remove menu
        gameOverPanel.SetActive(false);
        buttonToContinueText.gameObject.SetActive(false);

        // Reset level
        _cooldownTime = 0;
        _gameTime = gameTime;
        _playerScores = new int[2];
        UpdateUI();

        // Restart
        _gameRunning = true;
        Time.timeScale = 1;
    }

    private void Update()
    {
        TryStopGame();

        if (!_gameRunning)
        {
            _cooldownTime = Mathf.Max(0, _cooldownTime - Time.unscaledDeltaTime);
            cooldownBar.fillAmount = _cooldownTime / gameOverCooldown;

            if (_cooldownTime <= 0)
            {
                buttonToContinueText.gameObject.SetActive(true);
            }

            var buttonPressed = Input.GetButtonDown("Action0") || Input.GetButtonDown("Action1");
            if (buttonPressed && _cooldownTime <= 0)
            {
                RestartGame();
            }

            return;
        }

        _gameTime = Mathf.Max(0, _gameTime - Time.deltaTime);
        timerText.text = $"{_gameTime:0.0}";

        if (_gameTime > 0) return;
        Time.timeScale = 0;
        _gameRunning = false;
        _cooldownTime = gameOverCooldown;

        var winnerIndex = _playerScores[0] > _playerScores[1] ? 0 : _playerScores[0] < _playerScores[1] ? 1 : -1;

        gameOverPanel.SetActive(true);
        if (winnerIndex >= 0)
        {
            var winnerName = playerNames[winnerIndex];
            var winnerColor = ColorUtility.ToHtmlStringRGBA(playerScoreText[winnerIndex].color);
            winnerText.text = $"The <color=#{winnerColor}>{winnerName}</color> wins!";
        }
        else
        {
            winnerText.text = _playerScores[0] == 0 ? "Try harder!" : "Everybody wins!";
        }
    }

    private void UpdateUI()
    {
        playerScoreText[0].text = $"{_playerScores[0]}";
        playerScoreText[1].text = $"{_playerScores[1]}";
    }

    private static void TryStopGame()
    {
        if (!Input.GetButtonDown("Cancel")) return;

        Debug.Log("Stopping game.");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
         Application.OpenURL(webplayerQuitURL);
#else
         Application.Quit();
#endif
    }

}
