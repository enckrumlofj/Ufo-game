using UnityEngine;
using System.Collections;

public class LightStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.SetActive (false);
		Invoke ("setActivatedLight", 2f);
	
	}
	
	void setActivatedLight(){
		gameObject.SetActive (true);
	}
}
