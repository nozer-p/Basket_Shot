using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InBasket : MonoBehaviour
{
    private bool inBasket;
    [SerializeField] private bool leftBasket;
    [SerializeField] private bool joinBasket;

    [SerializeField] private float speed;
    [SerializeField] private float speedMove;

    [SerializeField] private GameObject basket;
    [SerializeField] private GameObject ballPosInBasket;
    [SerializeField] private GameObject grid;

    private GameObject ball;

    private SwipeDetection swipeDetection;

    [SerializeField] private Vector3 joinForce;
    [SerializeField] private Vector3 leftForce;

    private float timeBtwFall;
    [SerializeField] private float startTimeBtwFall;

    private void Start()
    {
        ball = FindObjectOfType<BallMovement>().gameObject;
        swipeDetection = FindObjectOfType<SwipeDetection>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && timeBtwFall <= 0f)
        {
            Debug.Log(1);
            inBasket = true;
            joinBasket = true;
            leftBasket = false;

            swipeDetection.ChangeBasket(basket, ballPosInBasket, grid, this.gameObject.GetComponent<InBasket>());

            FindObjectOfType<BallMovement>().GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            other.gameObject.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log(0);
            inBasket = false;
            leftBasket = true;
            timeBtwFall = startTimeBtwFall;

            FindObjectOfType<BallMovement>().GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            //other.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    private void Update()
    {
        GetBack();

        if (timeBtwFall < 0f)
        {
            timeBtwFall = 0f;
        }
        else if (timeBtwFall > 0f)
        {
            timeBtwFall -= Time.deltaTime; 
        }
    }

    private void GetBack()
    {
        if (!inBasket)
        {
            if (leftBasket)
            {
                grid.transform.localScale = Vector3.Lerp(grid.transform.localScale, leftForce, speed * Time.deltaTime);
                ballPosInBasket.transform.localPosition = new Vector3(0f, -0.6f + 1f - grid.transform.localScale.y, 0f);

                if (grid.transform.localScale.y <= leftForce.y + 0.01f)
                {
                    leftBasket = false;
                }
            }
            else
            {
                grid.transform.localScale = Vector3.Lerp(grid.transform.localScale, Vector3.one, speed * Time.deltaTime);
                ballPosInBasket.transform.localPosition = new Vector3(0f, -0.6f + 1f - grid.transform.localScale.y, 0f);
            }
        }
        else
        {
            if (joinBasket)
            {
                if ((ballPosInBasket.transform.position - ball.transform.position).magnitude <= 0.1f)
                {
                    if (!leftBasket)
                    {
                        grid.transform.localScale = Vector3.Lerp(grid.transform.localScale, joinForce, speed * Time.deltaTime);
                        ballPosInBasket.transform.localPosition = new Vector3(0f, -0.6f + 1f - grid.transform.localScale.y, 0f);
                        ball.transform.position = ballPosInBasket.transform.position;
                        ball.transform.rotation = basket.transform.rotation;
                        //ballPosInBasket.transform.localPosition = new Vector3(0f, -0.5f - 0.33f * delta / maxDeadZone, 0f);

                        if (grid.transform.localScale.y >= joinForce.y - 0.01f)
                        {
                            leftBasket = true;
                        }
                    }
                    else
                    {
                        grid.transform.localScale = Vector3.Lerp(grid.transform.localScale, Vector3.one, speed * Time.deltaTime);
                        ballPosInBasket.transform.localPosition = new Vector3(0f, -0.6f + 1f - grid.transform.localScale.y, 0f);
                        ball.transform.position = ballPosInBasket.transform.position;
                        ball.transform.rotation = basket.transform.rotation;

                        if (grid.transform.localScale.y <= 1.01f)
                        {
                            joinBasket = false;
                        }
                    }
                }
                else
                {
                    ball.transform.position = Vector3.MoveTowards(ball.transform.position, ballPosInBasket.transform.position, speedMove * Time.deltaTime);
                }
            }
        }
    }

    public bool GetJoinBasket()
    {
        return joinBasket;
    }

    public bool GetInBasket()
    {
        return inBasket;
    }

    public void SetInBasket(bool inBasket)
    {
        this.inBasket = inBasket;
    }
}