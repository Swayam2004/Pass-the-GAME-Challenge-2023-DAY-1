using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    public float movementSpeed = 5f;
    public float rotationSpeed = 180f;

    private Rigidbody2D rb;

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
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float moveInput = GameInput.Instance.GetMovementNormalised().y;
        float rotationInput = GameInput.Instance.GetMovementNormalised().x;

        Vector2 movement = transform.up * moveInput * movementSpeed;
        rb.velocity = movement;

        float rotationAmount = rotationInput * rotationSpeed * Time.deltaTime;
        rb.rotation -= rotationAmount;
    }
}
