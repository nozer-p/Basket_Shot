using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class shop : MonoBehaviour {

	public GameObject[] ball;
	public int[] owned;
	private GameObject current;
	public GameObject nextb, prevs, buy, play,watchb;
	private ball b;
	public Text price,credit,adst;
	public bool addBall;
	private int init=0,crd;


	void Start(){


		int children = transform.childCount;
		ball= new GameObject[children];
		for (int i = 0; i < children; ++i) {
			ball[i]= transform.GetChild (i).gameObject;
		}
	
		init = PlayerPrefs.GetInt ("init");
		if (init == 0&& addBall) {
			owned = new int[ball.Length];
			owned [0] = 1;
			PlayerPrefsX.SetIntArray ("own", owned);
			init = 1;
			PlayerPrefs.SetInt ("init", init);
		} else {
			owned = PlayerPrefsX.GetIntArray ("own");
		}
		if (addBall) {
			int[] tmp= new int[ball.Length];
			for (int i = 0; i < owned.Length; i++) {
				tmp [i] = owned [i];
			}
			owned = tmp;
			PlayerPrefsX.SetIntArray ("own", owned);

		}
		current = ball [0];
		for (int i = 1; i < ball.Length; i++) {

			ball [i].SetActive (false);

		}
		crd = PlayerPrefs.GetInt ("credit");
		credit.text = crd.ToString();

		price.text="owned";
		buy.SetActive (false);
		play.SetActive (true);
		prevs.SetActive (false);

		
	}
	public void next(){
		
		b=current.GetComponent<ball>();     //prev ball
		prevs.SetActive (true);
		if(b.number<ball.Length-1){
			//print (b.number);
			if (b.number == ball.Length - 2) {
				nextb.SetActive (false);
			}		
		ball [b.number].SetActive (false);
		for (int i = 0; i < ball.Length; i++) {

			if (i == b.number + 1) {
					ball actual = ball [i].GetComponent<ball> ();
					ball [i].SetActive (true);

					if (owned [i] == 1) {
						buy.SetActive (false);
						play.SetActive (true);
						watchb.SetActive (false);
						price.text = "owned";
					} else {

						if (actual.ads) {

							watchb.SetActive (true);
							buy.SetActive (false);
							play.SetActive (false);

							if (PlayerPrefs.GetInt ("watch" + actual.number) == 0) {

								adst.text = "X " + actual.numberAds;
								price.text = "";
							} else {


								adst.text = "X " + PlayerPrefs.GetInt("watch"+actual.number);
								price.text = "";
							}

							//adst.text = "X " + actual.numberAds;


						} else {

							watchb.SetActive (false);
							buy.SetActive (true);
							play.SetActive (false);
							price.text = ball [i].GetComponent<ball> ().price.ToString ();
						}
					}

				current = ball [i];
			}

		}

		}
	}

		

	public void prev(){

		b=current.GetComponent<ball>();


		nextb.SetActive (true);
		if(b.number>0){
			if (b.number == 1) {
				prevs.SetActive (false);
			}	
			//print (b.number);
			ball [b.number].SetActive (false);
			for (int i = 0; i < ball.Length; i++) {

				if (i == b.number - 1) {
					ball actual = ball [i].GetComponent<ball> ();
					ball [i].SetActive (true);

					if (owned[i]==1) {
						buy.SetActive (false);
						play.SetActive (true);
						price.text = "owned";
						watchb.SetActive (false);
					} else {

						if (actual.ads) {
							
							watchb.SetActive (true);
							buy.SetActive (false);
							play.SetActive (false);
							if (PlayerPrefs.GetInt ("watch" + actual.number) == 0) {

								adst.text = "X " + actual.numberAds;
								price.text = "";
							} else {


								adst.text = "X " + PlayerPrefs.GetInt("watch"+actual.number);
								price.text = "";
							}



						} else {

							watchb.SetActive (false);
							buy.SetActive (true);
							play.SetActive (false);
							price.text = ball [i].GetComponent<ball> ().price.ToString ();

						}

					}
				
					current = ball [i];
				}

			}

		}
		
	}

	public void buying(){
		
		b=current.GetComponent<ball>();
		if (crd >= b.price) {
			owned [b.number] = 1;

			PlayerPrefsX.SetIntArray ("own", owned);

			PlayerPrefs.SetInt ("ball", b.number);
			//print (b.number);
			crd = crd - b.price;
			PlayerPrefs.SetInt ("credit", crd);
			price.text = "owned";
			play.SetActive (true);
			buy.SetActive (false);
		} else {

			print("not enogh money");
		}





	}
	public void playing(){

		b=current.GetComponent<ball>();

		PlayerPrefs.SetInt ("ball", b.number);
		SceneManager.LoadScene("main");
	}

	public void watch(){



		b=current.GetComponent<ball>();
		if (PlayerPrefs.GetInt ("watch" + b.number) == 0) {



			//print ("watched " + b.number);
			PlayerPrefs.SetInt ("watch" + b.number, b.numberAds - 1);

			if (PlayerPrefs.GetInt ("watch" + b.number) == 0) {

				owned [b.number] = 1;

				PlayerPrefsX.SetIntArray ("own", owned);

				PlayerPrefs.SetInt ("ball", b.number);
	
				price.text = "owned";
				play.SetActive (true);
				buy.SetActive (false);
				watchb.SetActive (false);

			} else {

				adst.text = "X " + PlayerPrefs.GetInt("watch"+b.number);
			}


		} else {


			PlayerPrefs.SetInt ("watch" + b.number, PlayerPrefs.GetInt("watch"+b.number)-1);

			if (PlayerPrefs.GetInt ("watch" + b.number) == 0) {

				owned [b.number] = 1;

				PlayerPrefsX.SetIntArray ("own", owned);

				PlayerPrefs.SetInt ("ball", b.number);

				price.text = "owned";
				play.SetActive (true);
				buy.SetActive (false);
				watchb.SetActive (false);

			} else {

				adst.text = "X " + PlayerPrefs.GetInt("watch"+b.number);
			}
		}

		//print ("a video was watched so take this");


	}
}
