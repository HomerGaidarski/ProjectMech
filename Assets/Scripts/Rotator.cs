using UnityEngine;
using System.Collections;
using Player;

public class Rotator : MonoBehaviour {
	Health playerHealthScript;

	// Use this for initialization
	void Start () {
		GameObject playerObject = GameObject.Find ("Player");
		playerHealthScript = playerObject.GetComponent<Health> ();
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (new Vector3(15,30,45) * Time.deltaTime);
	}

	void OnTriggerEnter(Collider col){
		print (col.transform.root.gameObject.name);
		if (col.transform.root.gameObject.name == "Player") {
			print ("AYYYYYYYYYYYYYYYYYY LMAO");
			if (playerHealthScript.giveHealth (10)) {
				Destroy (gameObject);
			}
		}	

	}	
}
