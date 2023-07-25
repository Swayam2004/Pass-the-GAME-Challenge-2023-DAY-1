using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _mainMenuButton;

    private void Awake()
    {
        _restartButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("GameScene");
        });

        _mainMenuButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("MainMenuScene");
        });

        gameObject.SetActive(false);
    }
}
