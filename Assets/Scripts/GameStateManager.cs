using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    [Header("State")]
    [SerializeField] private bool _isPaused;
    [SerializeField] private bool _isGameOver;

    public bool IsPaused => _isPaused;
    public bool IsGameOver => _isGameOver;

    public bool IsGameplayBlocked => _isPaused || _isGameOver;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Time.timeScale = 1f;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Time.timeScale = 1f;
    }

    private void Update()
    {
        if (!_isGameOver && Input.GetKeyDown(KeyCode.Escape))
            TogglePause();

        if (_isGameOver && Input.GetKeyDown(KeyCode.R))
            RestartScene();
    }

    public void TriggerGameOver()
    {
        if (_isGameOver)
            return;

        _isGameOver = true;
        _isPaused = false;
        Time.timeScale = 0f;
    }

    public void TogglePause()
    {
        SetPaused(!_isPaused);
    }

    public void SetPaused(bool paused)
    {
        if (_isGameOver)
            return;

        _isPaused = paused;
        Time.timeScale = _isPaused ? 0f : 1f;
    }

    public void RestartScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}