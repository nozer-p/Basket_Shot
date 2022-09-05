using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject basket;
    [SerializeField] private GameObject star;
    [SerializeField] private float wallDist;
    [SerializeField] private float minDistX;
    [SerializeField] private float minDistY;
    [SerializeField] private float maxDistY;
    private Follower follower;

    private void Start()
    {
        follower = FindObjectOfType<Follower>();
    }

    public void SpawnBasket(Vector3 oldBasketPos)
    {
        if (oldBasketPos.x >= 0f && (follower.GetBorderRX() - oldBasketPos).magnitude >= wallDist + minDistX)
        {
            float randX = Random.Range(minDistX, (follower.GetBorderRX() - oldBasketPos).magnitude);
            float randY = Random.Range(minDistY, maxDistY);
            Instantiate(basket, new Vector3(oldBasketPos.x + randX, oldBasketPos.y + randY, 0f), Quaternion.identity);
            if (Random.Range(0, 100) > 50) Instantiate(star, new Vector3(oldBasketPos.x + randX, oldBasketPos.y + randY + 1f, 0f), Quaternion.identity);
        }
        else if (oldBasketPos.x >= 0f && (follower.GetBorderRX() - oldBasketPos).magnitude <= wallDist + minDistX)
        {
            float randX = Random.Range(minDistX, (follower.GetBorderLX() - oldBasketPos).magnitude - wallDist);
            float randY = Random.Range(minDistY, maxDistY);
            Instantiate(basket, new Vector3(oldBasketPos.x - randX, oldBasketPos.y + randY, 0f), Quaternion.identity);
            if (Random.Range(0, 100) > 50) Instantiate(star, new Vector3(oldBasketPos.x - randX, oldBasketPos.y + randY + 1f, 0f), Quaternion.identity);
        }
        else if (oldBasketPos.x <= 0f && (follower.GetBorderLX() - oldBasketPos).magnitude >= wallDist + minDistX)
        {
            float randX = Random.Range(minDistX, (follower.GetBorderLX() - oldBasketPos).magnitude);
            float randY = Random.Range(minDistY, maxDistY);
            Instantiate(basket, new Vector3(oldBasketPos.x - randX, oldBasketPos.y + randY, 0f), Quaternion.identity);
            if (Random.Range(0, 100) > 50) Instantiate(star, new Vector3(oldBasketPos.x - randX, oldBasketPos.y + randY + 1f, 0f), Quaternion.identity);
        }
        else if (oldBasketPos.x <= 0f && (follower.GetBorderLX() - oldBasketPos).magnitude <= wallDist + minDistX)
        {
            float randX = Random.Range(minDistX, (follower.GetBorderRX() - oldBasketPos).magnitude - wallDist);
            float randY = Random.Range(minDistY, maxDistY);
            Instantiate(basket, new Vector3(oldBasketPos.x + randX, oldBasketPos.y + randY, 0f), Quaternion.identity);
            if (Random.Range(0, 100) > 50) Instantiate(star, new Vector3(oldBasketPos.x + randX, oldBasketPos.y + randY + 1f, 0f), Quaternion.identity);
        }
    }

    public void RestaerLevel()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}
