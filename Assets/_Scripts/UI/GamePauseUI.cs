using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _mainMenuButton;

    private void Awake()
    {
        _resumeButton.onClick.AddListener(() =>
        {
            GameManager.Instance.ToggleGamePause();
        });

        _mainMenuButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("MainMenuScene");
        });

        gameObject.SetActive(false);
    }
}
