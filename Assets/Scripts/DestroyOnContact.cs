using UnityEngine;
using System.Collections;

public class DestroyOnContact : MonoBehaviour {

	public static int lives=3;
	public GameObject explosion;
	public GameObject ufo;
	public float respawnTime;

	private Vector3 pos;

	void Start(){
		lives = 3;
	}

	void OnTriggerEnter2D(Collider2D other){

		if (other.gameObject.CompareTag ("bomb")) {
				pos.Set (transform.parent.gameObject.transform.position.x,
				transform.parent.gameObject.transform.position.y,
				0.0f
			);
			Destroy (other.gameObject);
			transform.parent.gameObject.SetActive(false);
			GameObject cloneExplosion=(GameObject)Instantiate (explosion, transform.position, transform.rotation);
			Destroy (cloneExplosion, 0.5f);
			Invoke("Respawn",respawnTime);
		}else if (other.gameObject.CompareTag ("cow")) {
			lives--;
		}
		else if (other.gameObject.CompareTag ("garbage")) {
		}
		Destroy (other.gameObject);

	}
	void Respawn(){
		this.GetComponent<Animator>().Play ("ufo");
		transform.parent.gameObject.SetActive (true);
		transform.parent.FindChild("light").gameObject.SetActive (false);
		Invoke ("activateLight", 2f);
	}
	void activateLight(){
		transform.parent.FindChild("light").gameObject.SetActive (true);
	}
}