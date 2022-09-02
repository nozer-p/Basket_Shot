using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    public static event OnSwipeInput SwipeEvent;
    public delegate void OnSwipeInput(Vector2 direction, float delta);

    private Vector2 tapPos;
    private Vector2 tapPosOld;
    private Vector2 tapPosNow;
    private Vector2 swipeDelta;

    [SerializeField] private float maxDeadZone = 500;
    private float delta = 0;

    private bool isSwiping;
    private bool isMobile;

    public BallMovement ball;
    public GameObject basket;
    public GameObject grid;
    public float offset;
    public float speed;

    private void Start()
    {
        isMobile = Application.isMobilePlatform;
    }

    private void Update()
    {
        if (!isMobile)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isSwiping = true;
                VisibleArrow(true);
                tapPos = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                //CheckSwipe();
                ResetSwipe();
            }
        }
        else
        {
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    isSwiping = true;
                    VisibleArrow(true);
                    tapPos = Input.GetTouch(0).position;
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Canceled || Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    //CheckSwipe();
                    ResetSwipe();
                }
            }
        }

        if (isSwiping)
        {
            /*
            tapPosNow = Input.mousePosition;

            if (tapPos == tapPosNow)
            {   
                Vector3 direction = -Input.mousePosition + (Vector3)tapPosOld;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                basket.transform.rotation = Quaternion.Euler(0f, 0f, angle + offset);
                //basket.transform.rotation = Quaternion.Lerp(basket.transform.rotation, Quaternion.Euler(0f, 0f, angle + offset), speed * Time.deltaTime);
            }
            else
            {
                Vector3 direction = -Input.mousePosition + (Vector3)tapPos;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                //basket.transform.rotation = Quaternion.Euler(0f, 0f, angle + offset);
                basket.transform.rotation = Quaternion.Lerp(basket.transform.rotation, Quaternion.Euler(0f, 0f, angle + offset), speed * Time.deltaTime);
                tapPosOld = tapPos;
            }
            */
            CheckSwipe();
        }
    }

    private void CheckSwipe()
    {
        swipeDelta = Vector2.zero;

        if (isSwiping)
        {
            if (!isMobile && Input.GetMouseButtonUp(0))
            {
                swipeDelta = (Vector2)Input.mousePosition - tapPos;
            }
            else if (Input.touchCount > 0)
            {
                swipeDelta = Input.GetTouch(0).position - tapPos;
            }
        }

        if (swipeDelta.magnitude > maxDeadZone)
        {
            delta = maxDeadZone;
        }
        else 
        {
            delta = swipeDelta.magnitude;
        }


        if (basket != null)
        {
            Debug.Log(delta);
            grid.transform.localScale = new Vector3(1f, 1f + 0.45f * delta / maxDeadZone, 1f);
            ball.transform.localPosition = new Vector3(0f, -0.48f - 0.45f * delta / maxDeadZone, 0f);
        }

        if (SwipeEvent != null && delta != 0f)
        {
            tapPosNow = Input.mousePosition;

            if (tapPos == tapPosNow)
            {
                Vector3 direction = -Input.mousePosition + (Vector3)tapPosOld;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                basket.transform.rotation = Quaternion.Euler(0f, 0f, angle + offset);
                //basket.transform.rotation = Quaternion.Lerp(basket.transform.rotation, Quaternion.Euler(0f, 0f, angle + offset), speed * Time.deltaTime);
            }
            else
            {
                Vector3 direction = -Input.mousePosition + (Vector3)tapPos;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                //basket.transform.rotation = Quaternion.Euler(0f, 0f, angle + offset);
                basket.transform.rotation = Quaternion.Lerp(basket.transform.rotation, Quaternion.Euler(0f, 0f, angle + offset), speed * Time.deltaTime);
                tapPosOld = tapPos;
            }

            SwipeEvent(swipeDelta.normalized, delta);
        }

        //ResetSwipe();
    }

    private void ResetSwipe()
    {
        isSwiping = false;
        delta = 0f;
        VisibleArrow(false);
        tapPos = Vector2.zero;
        swipeDelta = Vector2.zero;
    }

    private void VisibleArrow(bool log)
    {
        if (ball != null && log)
        {
            ball.FreezeRotation(true);
        }
        else if (ball != null && !log)
        {
            ball.FreezeRotation(false);
        }
    }
}