using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private float _gameTime = 30;

    [SerializeField]
    private TextMeshProUGUI[] playerScoreText;

    [SerializeField]
    private TextMeshProUGUI timerText;

    [SerializeField]
    private GameObject gameOverPanel;

    [SerializeField]
    private TextMeshProUGUI winnerText;

    [SerializeField]
    private string[] playerNames = { "Green Toad", "Red Toad" };

    private int[] _playerScores;

    public void AddPoints(int player, int amount)
    {
        _playerScores[player] += amount;
        UpdateUI();
    }

    private void Start()
    {
        _playerScores = new int[2];
        UpdateUI();
    }

    private void Update()
    {
        _gameTime = Mathf.Max(0, _gameTime - Time.deltaTime);
        timerText.text = $"{_gameTime:0.0}";

        if (_gameTime > 0) return;
        Time.timeScale = 0;

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
}
