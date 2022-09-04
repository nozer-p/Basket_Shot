using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private float delta;

    [SerializeField] private float forceValue;
    [SerializeField] private float speedRotate;
    [SerializeField] private float speedRemoveLastGoal;

    [SerializeField] private List<GameObject> goals = new List<GameObject>();
    private bool lastGoal;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (rb.bodyType == RigidbodyType2D.Dynamic && rb.velocity != Vector2.zero)
        {
            if (delta == 0f) delta = 40f;
            float speed = speedRotate * delta / 10f;
            if (speed > 11f) speed = 11f;
            transform.Rotate(0f, 0f, speed);
        }

        if (lastGoal)
        {
            RemoveLastGoalFromList();
        }
    }

    public void Move(float delta)
    {
        this.delta = delta;
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

    public void AddGoal(GameObject goal)
    {
        if (!goals.Contains(goal))
        {
            goals.Add(goal);
        }
    }

    public bool RemoveGoal()
    {
        if (goals.Count > 1)
        {
            lastGoal = true;
            return lastGoal;
        }
        return lastGoal;
    }

    private void RemoveLastGoalFromList()
    {
        goals[0].transform.localScale = Vector3.Lerp(goals[0].transform.localScale, Vector3.zero, speedRemoveLastGoal * Time.deltaTime);
        
        if (goals[0].transform.localScale.z <= 0.02f)
        {
            Destroy(goals[0]);
            goals.Remove(goals[0]);
            lastGoal = false;
        }
    }
}