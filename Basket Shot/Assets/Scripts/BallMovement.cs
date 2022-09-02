using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float ForceValue;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        SwipeDetection.SwipeEvent += OnSwipe;
    }

    private void OnSwipe(Vector2 direction, float delta)
    {
        Vector3 dir = (Vector3)direction;
        Move(dir, delta);
    }

    private void Move(Vector3 direction, float delta)
    {
        rb.velocity = direction * ForceValue * delta;
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void Update()
    {


    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Walls" || collision.collider.gameObject.tag == "Basket")
        {

        }
    }
    
    public void FreezeRotation(bool log)
    {
        if (rb != null)
        {
            rb.freezeRotation = log;
        }
    }
}
