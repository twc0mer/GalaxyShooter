using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool isGameOver;

    [SerializeField]
    private bool isCoop;

    [SerializeField]
    private bool _isPaused = false;

    [SerializeField]
    private GameObject _pauseMenuPanel;

    private Animator _pauseAnimator;

    public void GameOver()
    {
        isGameOver = true;
    }

    public void ResumePlay()
    {
        _isPaused = false;
        if (_pauseAnimator != null)
        {
            _pauseAnimator.SetBool("IsPaused", false);
        }

        _pauseMenuPanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void BackToMainMenu()
    {
        _isPaused = false;
        if (_pauseAnimator != null)
        {
            _pauseAnimator.SetBool("IsPaused", false);
        }

        _pauseMenuPanel.SetActive(false);
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }

    private void Pause()
    {
        _isPaused = true;
        _pauseMenuPanel.SetActive(true);

        if (_pauseAnimator != null)
        {
            _pauseAnimator.SetBool("IsPaused", true);
        }
        
        Time.timeScale = 0;
    }

    private void Start()
    {
        _pauseAnimator = _pauseMenuPanel.GetComponent<Animator>();
        _pauseAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && _pauseMenuPanel != null)
        {
            if (!_isPaused)
            {
                Pause();
            }
            else
            {
                ResumePlay();
            }
            
        }

        if (Input.GetKeyDown(KeyCode.R) && isGameOver)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().name == "MainMenu")
            {
                Application.Quit(0);
            }
            else
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
    }

    public bool IsCoop() { return isCoop; }
}
