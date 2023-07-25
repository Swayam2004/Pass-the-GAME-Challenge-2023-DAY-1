using System;
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
    private AudioSource _playerAudioSource;
    private int _health;
    private bool _isMoving;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There are more than one PlayerController");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _playerAudioSource = GetComponent<AudioSource>();
        _rb = GetComponent<Rigidbody2D>();

        _health = 6;
    }

    private void Update()
    {
        float moveInput = GameInput.Instance.GetMovementNormalised().y;
        float rotationInput = GameInput.Instance.GetMovementNormalised().x;

        Vector2 movement = transform.up * moveInput * _movementSpeed;
        _rb.velocity = movement;

        float rotationAmount = rotationInput * _rotationSpeed * Time.deltaTime;
        _rb.rotation -= rotationAmount;

        _isMoving = !(movement == Vector2.zero && rotationAmount == 0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Junk junk))
        {
            JunkManager.Instance.DestroyJunk(junk);
            OnJunkCollided?.Invoke(this, EventArgs.Empty);

            SoundManager.Instance.PlayPickupSound(junk);
        }
        else if (collision.gameObject.TryGetComponent(out Asteroid asteroid))
        {
            Damage(1);

            SoundManager.Instance.PlayHitSound(asteroid);
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

    public bool IsMoving()
    {
        return _isMoving;
    }

    public AudioSource GetPlayerAudioSource()
    {
        return _playerAudioSource;
    }
}
