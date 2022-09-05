using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField] private float speed;
    private GameObject player;
    private float borderX;

    static Plane plane;
    [SerializeField] private Transform leftWall;
    [SerializeField] private Transform rightWall;

    private void Start()
    {
        player = FindObjectOfType<BallMovement>().gameObject;

        plane = new Plane(transform.forward, transform.position);
        borderX = Mathf.Abs(CalcPosition(new Vector2(0f, 0f)).x);

        leftWall.transform.position = new Vector3(-borderX - leftWall.transform.localScale.x / 2, 0f, 0f);
        rightWall.transform.position = new Vector3(borderX + rightWall.transform.localScale.x / 2, 0f, 0f);

        Trajectory.instance.copyAllObstacles();
    }

    private void FixedUpdate()
    {
        if (player != null && transform.position.y != player.transform.position.y && !player.GetComponent<BallMovement>().IsFailForFollower())
        {
            Vector3 directionToTarget = (player.transform.position - transform.position).normalized;
            directionToTarget = new Vector3(0f, directionToTarget.y, 0f);
            transform.Translate(directionToTarget * speed * Time.deltaTime);
        }
    }

    public Vector3 CalcPosition(Vector2 screenPos)
    {
        //Ray ray = UICamera.currentCamera.ScreenPointToRay(screenPos);    // для NGUI
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        float dist = 0f;
        Vector3 pos = Vector3.zero;

        if (plane.Raycast(ray, out dist))
            pos = ray.GetPoint(dist);

        return pos;
    }

    public Vector3 GetBorderRX()
    {
        return new Vector3(borderX, transform.position.y, transform.position.z);
    }

    public Vector3 GetBorderLX()
    {
        return new Vector3(-borderX, transform.position.y, transform.position.z);
    }
}