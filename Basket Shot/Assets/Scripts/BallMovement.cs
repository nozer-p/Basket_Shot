using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float forceValue;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Move(float delta)
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.velocity = transform.up * delta * forceValue;
    }

    public void StopInBasket()
    {
        rb.bodyType = RigidbodyType2D.Static;
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    public float GetForceValue()
    {
        return forceValue;
    }
    
    public void Freeze(bool log)
    {
        if (rb != null)
        {
            rb.simulated = !log;
            rb.freezeRotation = log;
        }
    }
}