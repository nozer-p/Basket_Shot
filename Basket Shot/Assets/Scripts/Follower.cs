using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField] private float speed;
    private GameObject player;

    private void Start()
    {
        player = FindObjectOfType<BallMovement>().gameObject;    
    }

    private void FixedUpdate()
    {
        if (transform.position.y != player.transform.position.y)
        {
            Vector3 directionToTarget = (player.transform.position - transform.position).normalized;
            directionToTarget = new Vector3(0f, directionToTarget.y, 0f);
            transform.Translate(directionToTarget * speed * Time.deltaTime);
        }
    }
}