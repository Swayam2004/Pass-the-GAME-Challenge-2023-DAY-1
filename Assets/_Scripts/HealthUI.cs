using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Image[] _heartSpriteArray;
    [SerializeField] private Sprite _emptyHeart;
    [SerializeField] private Sprite _fullHeart;

    private int _maxHealth = 6;
    private int _health = 6;

    private void Start()
    {
        PlayerController.Instance.OnHealthChanged += PlayerController_OnHealthChanged; ;

        _maxHealth = _heartSpriteArray.Length;

        _health = _maxHealth;
        UpdateVisual();
    }

    private void PlayerController_OnHealthChanged(object sender, PlayerController.OnHealthChangedEventArgs e)
    {
        _health = e.Health;
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (_health > _maxHealth)
        {
            _health = _maxHealth;
        }

        for (int i = 0; i < _heartSpriteArray.Length; i++)
        {
            if (i < _maxHealth)
            {
                _heartSpriteArray[i].gameObject.SetActive(true);
            }
            else
            {
                _heartSpriteArray[i].gameObject.SetActive(true);
            }

            if(i < _health)
            {
                _heartSpriteArray[i].sprite = _fullHeart;
            }
            else
            {
                _heartSpriteArray[i].sprite = _emptyHeart;
            }
        }
    }
}
