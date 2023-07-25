using System;
using System.ComponentModel;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;

    private enum State
    {
        GamePlaying,
        GameOver,
    }

    [SerializeField] private GameObject _gameOverUI;
    [SerializeField] private GameObject _gamePauseUI;

    private int _score;
    private State _state;
    private bool _isPaused = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There are more than one PlayerController");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _score = 0;
    }

    private void Start()
    {
        PlayerController.Instance.OnJunkCollided += PlayerController_OnJunkCollided;
        PlayerController.Instance.OnHealthChanged += PlayerController_OnHealthChanged;
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
    }

    private void GameInput_OnPauseAction(object sender, System.EventArgs e)
    {
        ToggleGamePause();
    }

    private void PlayerController_OnHealthChanged(object sender, PlayerController.OnHealthChangedEventArgs e)
    {
        if (IsGamePlaying() && PlayerController.Instance.GetHealth() <= 0)
        {
            _gameOverUI.SetActive(true);
        }
    }

    private void Update()
    {
        switch (_state)
        {
            case State.GamePlaying:
                break;

            case State.GameOver:
                break;
        }
    }

    private void PlayerController_OnJunkCollided(object sender, System.EventArgs e)
    {
        _score++;
        ScoreUI.Instance.SetScoreText(_score);
    }

    public bool IsGamePlaying()
    {
        return _state == State.GamePlaying;
    }

    public void ToggleGamePause()
    {
        _isPaused = !_isPaused;

        if (_isPaused)
        {
            _gamePauseUI.SetActive(true);
            OnGamePaused?.Invoke(this, EventArgs.Empty);
            Time.timeScale = 0;
        }
        else
        {
            _gamePauseUI.SetActive(false);
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
            Time.timeScale = 1;
        }
    }
}
