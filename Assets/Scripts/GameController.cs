using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {
	public Camera cam;
	public GameObject[] spawnObjects;
	public float[] probabilities;
	public float spawnTime;
	public Text scoreCows;
	public Text scoreGarbage;
	public Text timeLeft;
	public GameObject startButton;
	public GameObject restartButton;
	public GameObject gameOverTextC;
	public Text gameOverText;
	public GameObject exitButton;
	public GameObject[] lights;

	private int scoreCowsCounter;
	private int scoreGarbageCounter;
	private float maxWidth;
	private float timeLeftCounter;
	private bool playing;

	Vector3 touchPosWorld;

	//Change me to change the touch phase used.
	TouchPhase touchPhase = TouchPhase.Began;

	void FixedUpdate(){
		if (playing) {
			timeLeftCounter -= Time.deltaTime;
			if (timeLeftCounter < 0)
				timeLeftCounter = 0;
			timeLeft.text = "Time:\n" + Mathf.RoundToInt (timeLeftCounter);
		}
	}

	void Update () {
			//We check if we have more than one touch happening.
			//We also check if the first touches phase is Ended (that the finger was lifted)
		if (DestroyOnContact.lives <= 0)
			timeLeftCounter = 0;
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == touchPhase) {
			//We transform the touch position into word space from screen space and store it.
			touchPosWorld = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
			Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);
			//We now raycast with this information. If we have hit something we can process it.
			RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);

			if (hitInformation.collider != null) {
				//We should have hit something with a 2D Physics collider!
				GameObject touchedObject = hitInformation.transform.gameObject;
				if (touchedObject.CompareTag ("cow")) {
					scoreCowsCounter++;
					scoreCows.text = "Cows saved:\n" + scoreCowsCounter;
					Destroy(touchedObject);
				} else if (touchedObject.CompareTag ("garbage")){
					scoreGarbageCounter++;
					scoreGarbage.text = "Garbage:\n" + scoreGarbageCounter;
					Destroy(touchedObject);
					//multiply speed of lights 2 times if u cathed garbage
					for (int i = 0; i < lights.Length; i++) {
						(lights [i].GetComponent (typeof(AreaEffector2D)) as AreaEffector2D).forceMagnitude = 1.3f*(lights [i].GetComponent (typeof(AreaEffector2D)) as AreaEffector2D).forceMagnitude;
					}
					//
				}
			}
		}
	}


	void Start(){
		
		playing = false;
		toggleUfos (false);
		scoreCows.text = "Cows saved:\n0";
		scoreGarbage.text = "Garbage:\n0 ";
		scoreCowsCounter = 0;
		scoreGarbageCounter = 0;
		timeLeftCounter = 40.0f;
		if (cam == null) {
			cam = Camera.main;
		}
		Vector3 upperCorner = new Vector3 (Screen.width, Screen.height, 0.0f);
		Vector3 targetWidth = cam.ScreenToWorldPoint (upperCorner);
		float objectWidth = spawnObjects [0].GetComponent<Renderer> ().bounds.extents.x;
		maxWidth = targetWidth.x - objectWidth;


	}

	public void StartButton(){
		startButton.SetActive (false);
		exitButton.SetActive (false);
		playing = true;
		toggleUfos (true);
		StartCoroutine (Spawn ());
	}

	void toggleUfos(bool t){
		for (int i = 0; i < lights.Length; i++) {
			lights [i].SetActive (t);
			lights [i].transform.parent.gameObject.SetActive (t);
		}
	}

	IEnumerator Spawn(){
		yield return new WaitForSeconds(2f);
		while (timeLeftCounter>0) {
			GameObject spawnObject = spawnObjects [Choose (probabilities)];
			Vector3 spawnPosition = new Vector3 (
				                        Random.Range (-maxWidth, maxWidth),
				                        transform.position.y + spawnObject.GetComponent<Renderer> ().bounds.extents.y,
				                        0.0f
			                        );
			Quaternion spawnRotation = Quaternion.identity;
			Instantiate (spawnObject, spawnPosition, spawnRotation);
			yield return new WaitForSeconds (spawnTime);
		}
		toggleUfos (false);
		gameOverText.text="Game Over\nScore: "+scoreCowsCounter.ToString ();
		gameOverTextC.SetActive (true);
		restartButton.SetActive (true);
	}

	int Choose(float[] probs){
		float total = 0;
		foreach (float elem in probs) {
			total += elem;
		}

		float randomPoint = Random.value * total;

		for (int i = 0; i < probs.Length; i++) {
			if (randomPoint < probs [i]) {
				return i;
			}else {
				randomPoint -= probs[i];
			}
		}
		return -1;//Should never appear, or u got wrong parameters
	}
}
