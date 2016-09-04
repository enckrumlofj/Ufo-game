using UnityEngine;
using System.Collections;

public class DestroyOnContact : MonoBehaviour {

	public static int lives=30;
	public GameObject explosion;
	public GameObject ufo;
	public float respawnTime;

	private Vector3 pos;

	void OnTriggerEnter2D(Collider2D other){

		if (other.gameObject.CompareTag ("bomb")) {
				pos.Set (transform.parent.gameObject.transform.position.x,
				transform.parent.gameObject.transform.position.y,
				0.0f
			);
			Destroy (other.gameObject);
			transform.parent.gameObject.SetActive(false);
		//	this.GetComponent<Animator>().Play ("ufo");
			GameObject cloneExplosion=(GameObject)Instantiate (explosion, transform.position, transform.rotation);
			Destroy (cloneExplosion, 0.5f);
			Invoke("Respawn",respawnTime);
		}else if (other.gameObject.CompareTag ("cow")) {
			lives--;
		}
		Destroy (other.gameObject);

	}
	void Respawn(){
		//GameObject cloneUFOComing=(GameObject)Instantiate (ufoCome, transform.position, transform.rotation);
		//Destroy (cloneUFOComing, 0.5f);
		this.GetComponent<Animator>().Play ("ufo");
		//this.GetComponent<Animator> ().Stop ();
		transform.parent.gameObject.SetActive (true);
		transform.parent.FindChild("light").gameObject.SetActive (false);
		Invoke ("activateLight", 2f);
	}
	void activateLight(){
		transform.parent.FindChild("light").gameObject.SetActive (true);
	}
}