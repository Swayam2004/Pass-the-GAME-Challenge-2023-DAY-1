using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _asteroidSpinSpeed;
    [SerializeField] private float _asteroidMoveSpeedMin;
    [SerializeField] private float _asteroidMoveSpeedMax;

    private Rigidbody2D _asteroidRb;
    private Transform _playerTransform;

    private void Awake()
    {
        _asteroidRb = GetComponent<Rigidbody2D>();

        _playerTransform = PlayerController.Instance.transform;
    }

    private void Start()
    {
        Vector2 playerDirection = _playerTransform.position - transform.position;
        _asteroidRb.velocity = playerDirection.normalized * Random.Range(_asteroidMoveSpeedMin, _asteroidMoveSpeedMax);
    }

    private void Update()
    {
        float rotationAmount = _asteroidSpinSpeed * Time.deltaTime;
        _asteroidRb.rotation -= rotationAmount;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Junk junk))
        {
            Destroy(junk.gameObject);
        }
    }
}
