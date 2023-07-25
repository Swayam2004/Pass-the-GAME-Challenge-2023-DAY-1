using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    public event EventHandler OnJunkCollided;
    public event EventHandler<OnHealthChangedEventArgs> OnHealthChanged;
    public class OnHealthChangedEventArgs : EventArgs
    {
        public int Health;
    }

    public float _movementSpeed = 15f;
    public float _rotationSpeed = 180f;

    private Rigidbody2D _rb;
    private int _health;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There are more than one PlayerController"); 
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _health = 6;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float moveInput = GameInput.Instance.GetMovementNormalised().y;
        float rotationInput = GameInput.Instance.GetMovementNormalised().x;

        Vector2 movement = transform.up * moveInput * _movementSpeed;
        _rb.velocity = movement;

        float rotationAmount = rotationInput * _rotationSpeed * Time.deltaTime;
        _rb.rotation -= rotationAmount;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Junk junk))
        {
            Destroy(junk.gameObject);
            OnJunkCollided?.Invoke(this, EventArgs.Empty);
        }
        else if (collision.gameObject.TryGetComponent(out Asteroid asteroid))
        {
            Damage(1);
        }
    }


    public int GetHealth()
    {
        return _health;
    }

    public void Damage(int damageAmount)
    {
        _health -= damageAmount;
        OnHealthChanged?.Invoke(this, new OnHealthChangedEventArgs
        {
            Health = _health
        });
    }
}
