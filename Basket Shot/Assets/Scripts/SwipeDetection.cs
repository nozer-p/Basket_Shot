using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    private Vector2 tapPos;
    private Vector2 tapPosOld;
    private Vector2 tapPosNow;
    private Vector2 swipeDelta;

    [SerializeField] private float minDeadZone;
    [SerializeField] private float maxDeadZone;
    private float delta = 0;

    private bool isSwiping;
    private bool isMobile;

    [SerializeField] private GameObject prefabBall;
    [SerializeField] private BallMovement ball;

    private GameObject basket;
    private GameObject ballPosInBasket;
    private GameObject grid;
    private InBasket inBasket;

    [SerializeField] private float offset;
    [SerializeField] private float speed;
    [SerializeField] private float speedTrajectory;

    private void Start()
    {
        //Time.timeScale = 0.1f;
        isMobile = Application.isMobilePlatform;
    }

    [SerializeField] private GameObject[] objects;

    private void Off()
    {
        for(int i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(false);
        }
    }

    private void Update()
    {
        if (!isMobile)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isSwiping = true;
                //VisibleArrow(true);
                tapPos = Input.mousePosition;
                Off();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (inBasket != null && inBasket.GetInBasket()) ResetSwipe();
            }
        }
        else
        {
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    isSwiping = true;
                    //VisibleArrow(true);
                    tapPos = Input.GetTouch(0).position;
                    Off();
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Canceled || Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    if (inBasket != null && inBasket.GetInBasket()) ResetSwipe();
                }
            }
        }

        if (isSwiping && basket != null && inBasket != null && inBasket.GetInBasket())
        {
            CheckSwipe();
        }

        if (inBasket != null)
        {
            if (inBasket.GetInBasket() && !inBasket.GetJoinBasket())
            {
                grid.transform.localScale = new Vector3(1f, 1f + 0.33f * delta / maxDeadZone, 1f);
                ballPosInBasket.transform.localPosition = new Vector3(0f, -0.6f - 0.33f * delta / maxDeadZone, 0f);
                ball.transform.position = ballPosInBasket.transform.position;
                ball.transform.rotation = basket.transform.rotation;
            }
        }
    }

    private void CheckSwipe()
    {
        swipeDelta = Vector2.zero;
        
        if (!isMobile && Input.GetMouseButton(0))
        {
            swipeDelta = (Vector2)Input.mousePosition - tapPos;
        }
        else if (Input.touchCount > 0)
        {
            swipeDelta = Input.GetTouch(0).position - tapPos;
        }

        if (swipeDelta.magnitude > maxDeadZone)
        {
            delta = maxDeadZone;
        }
        else 
        {
            delta = swipeDelta.magnitude;
        }

        if (delta != 0f)
        {
            tapPosNow = Input.mousePosition;

            if (tapPos == tapPosNow)
            {
                Vector3 direction = -Input.mousePosition + (Vector3)tapPosOld;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                basket.transform.rotation = Quaternion.Euler(0f, 0f, angle + offset);
            }
            else
            {
                Vector3 direction = -Input.mousePosition + (Vector3)tapPos;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                basket.transform.rotation = Quaternion.Lerp(basket.transform.rotation, Quaternion.Euler(0f, 0f, angle + offset), speed * Time.deltaTime);
                tapPosOld = tapPos;

                Trajectory.instance.RemBalls();
                if (delta >= minDeadZone)
                {
                    float percent = Mathf.Pow(1f / (maxDeadZone) * (delta - minDeadZone / 5f), 2f) * 2f;                    
                    Trajectory.instance.predict(prefabBall, ball.gameObject.transform.position, ball.gameObject.transform.up * delta * ball.GetForceValue(), percent);
                }
            }
        }
    }

    private void ResetSwipe()
    {
        isSwiping = false;

        if (delta < minDeadZone)
        {
            inBasket.SetInBasket(true);
            Trajectory.instance.RemBalls();
            ball.StopInBasket();
        }
        else
        {
            inBasket.SetInBasket(false);
            Trajectory.instance.RemBalls();
            ball.Move(delta);
        }

        delta = 0f;
        //VisibleArrow(false);
        tapPos = Vector2.zero;
        swipeDelta = Vector2.zero;
    }

    private void VisibleArrow(bool log)
    {
        if (ball != null && log)
        {
            ball.Freeze(true);
        }
        else if (ball != null && !log)
        {
            ball.Freeze(false);
        }
    }

    public void ChangeBasket(GameObject basket, GameObject ballPosInBasket, GameObject grid, InBasket inBasket)
    {
        this.basket = basket;
        this.ballPosInBasket = ballPosInBasket;
        this.grid = grid;
        this.inBasket = inBasket;
    }

    public bool CheckBasket()
    {
        if (basket != null && inBasket != null && inBasket.GetJoinBasket())
        {
            return true;
        }
        return false;
    }
}