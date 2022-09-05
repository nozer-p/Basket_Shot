using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private GameManager gameManager;
    private Rigidbody2D rb;
    private float delta;

    [SerializeField] private GameObject ballSprt;

    [SerializeField] private float forceValue;
    [SerializeField] private float speedRotate;
    [SerializeField] private float speedRemoveLastGoal;

    [SerializeField] private List<GameObject> goals = new List<GameObject>();
    private bool lastGoal;

    /*
    private bool bounceGoal = false;
    private bool borderGoal = false;
    private int countTouchPerfect;
    private int countTouchWalls;
    private List<GameObject> lastTouches = new List<GameObject>();
    */
    private ScoreManager scoreManager;

    //private SwipeDetection swipeDetection;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
        //swipeDetection = FindObjectOfType<SwipeDetection>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (rb.bodyType == RigidbodyType2D.Dynamic && rb.velocity != Vector2.zero)
        {

            if (delta == 0f) delta = 40f;
            float speed = speedRotate * delta / 10f;
            if (speed > 11f) speed = 11f;
            //ballSprt.transform.Rotate(0f, 0f, speed);
            ballSprt.transform.Rotate(0f, 0f, rb.velocity.x * speedRotate);
        }

        if (lastGoal)
        {
            RemoveLastGoalFromList();
        }

        Fail();
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

            gameManager.SpawnBasket(goals[0].transform.position);
        }
    }

    private void Fail()
    {
        if (goals.Count > 0)
        {
            if (transform.position.y + 10f < goals[0].transform.position.y)
            {
                //Destroy(gameObject);
                gameManager.RestaerLevel();
            }
        }
    }

    public bool IsFailForFollower()
    {
        if (goals.Count > 0)
        {
            if (transform.position.y + 1.5f < goals[0].transform.position.y)
            {
                return true;
            }
        }        
        return false;
    }
    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!lastTouches.Contains(collision.gameObject)) lastTouches.Add(collision.gameObject);

        Debug.Log(lastTouches.Count);
        
        foreach (GameObject tmp in lastTouches)
        {
            if (tmp.CompareTag("Wall"))
            {
                countTouchWalls++;
                bounceGoal = true;
                borderGoal = false;
                countTouchPerfect++;
            }
            else if (tmp.CompareTag("Border") && countTouchWalls > 0)
            {
                bounceGoal = true;
                borderGoal = false;
                countTouchPerfect = 1;
            }
            else if (tmp.CompareTag("Border") && countTouchWalls == 0)
            {
                bounceGoal = false;
                borderGoal = true;
                countTouchPerfect = 1;
            }
            else
            {
                countTouchWalls = 0;
                countTouchPerfect++;
                bounceGoal = false;
                borderGoal = false;
            }
        }
        if (lastTouches.Count > 10)
        {
            int i = 0;
            foreach (GameObject tmp in lastTouches)
            {
                lastTouches.Remove(tmp);
                i++;
                if (i > 4)
                {
                    break;
                }
            }
        }
    }
    */
    public void CheckGoal()
    {
        /*
        int score = 0;

        if (countTouchPerfect > 1)
        {
            score += countTouchPerfect + 1;
        }
            
        if (borderGoal && bounceGoal)
        {
            score += 2;
        }
        else if (borderGoal && !bounceGoal)
        {
            score = 1;
        }

        Debug.Log(score);

        scoreManager.AddScore(score);
        */

        scoreManager.AddScore(1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Star"))
        {
            scoreManager.AddStar();
            Destroy(collision.gameObject);
        }
    }
}