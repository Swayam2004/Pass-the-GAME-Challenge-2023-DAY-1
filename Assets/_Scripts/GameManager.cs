using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int _score;

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
    }

    private void PlayerController_OnJunkCollided(object sender, System.EventArgs e)
    {
        _score++;
        ScoreUI.Instance.SetScoreText(_score);
    }
}
