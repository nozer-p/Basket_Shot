using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destructor : MonoBehaviour {
	public GameObject  ball,light;
	public gameManager gm;
	public AudioSource d;

	Vector2 position;

	void Start(){

		d = GameObject.Find ("soundManager3").GetComponent<AudioSource> ();
		ball=GameObject.Find("ball");
		light = GameObject.Find ("lightball");
		gm = GameObject.Find ("obstacleManager").GetComponent<gameManager>();



	}
	void OnTriggerEnter2D(Collider2D other)
	{
		position = ball.transform.position;
		position.y = ball.transform.position.y + 3;
		ball.SetActive (false);
		light.SetActive (false);
		gm.loseMenus ();
		d.Play ();
		//StartCoroutine (temp ());

	}

	IEnumerator temp(){

		yield return new WaitForSeconds (2);

		ball.transform.position = position;
		ball.SetActive (true);

	}
		
}
