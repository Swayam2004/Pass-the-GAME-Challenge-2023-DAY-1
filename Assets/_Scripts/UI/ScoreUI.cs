using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    public static ScoreUI Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI _scoreText;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There are more than one PlayerController");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        SetScoreText(0);
    }

    public void SetScoreText(int scoreAmount)
    {
        _scoreText.text = $"Junk Collected : {scoreAmount}";
    }
}
