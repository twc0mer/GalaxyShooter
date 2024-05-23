using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    public int currentScore;
    public int bestScore;

    [SerializeField]
    private TextMeshProUGUI _scoreText;

    [SerializeField]
    private TextMeshProUGUI _bestScoreText;

    [SerializeField]
    private Image _livesImage;

    [SerializeField]
    private Sprite[] _liveSprites;

    [SerializeField]
    private TextMeshProUGUI _gameOverText;

    [SerializeField]
    private TextMeshProUGUI _restartText;

    private GameManager _gameManager;

    void Start()
    {
        _scoreText.text = "Score: 0";
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("GameManager is null");
        }

        bestScore = PlayerPrefs.GetInt("Score");
        _bestScoreText.text = $"Best: {bestScore}";
    }

    public void CheckForBestScore()
    {
        if (currentScore > bestScore)
        {
            bestScore = currentScore;
            _bestScoreText.text = $"Best: {bestScore}";
            PlayerPrefs.SetInt("Score", bestScore);
        }
    }

    public void UpdateScoreText(int score)
    {
        currentScore = score;
        _scoreText.text = $"Score: {currentScore}";
    }

    public void UpdateLives(int currentLives)
    {
        if (_livesImage != null && _liveSprites.Length > 0)
        {
            _livesImage.sprite = _liveSprites[currentLives];
        }

        if (currentLives <= 0)
        {
            CheckForBestScore();   
            GameOver();
        }
    }

    public void GameOver()
    {
        if (_gameOverText != null)
        {
            _gameOverText.gameObject.SetActive(true);
            _restartText.gameObject.SetActive(true);
            _gameManager.GameOver();
            StartCoroutine(GameOverFlickerRoutine());
        }
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOverText.text = "Game Over";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = String.Empty;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
