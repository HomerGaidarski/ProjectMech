using UnityEngine;
using System.Collections;
using Player;

public class EnergyPack : MonoBehaviour {
	private Energy playerEnergyScript;

	// Use this for initialization
	void Start () {
		GameObject playerObject = GameObject.Find ("Player");
		playerEnergyScript = playerObject.GetComponent<Energy> ();
	}

	// Update is called once per frame
	void Update () {
		transform.Rotate (new Vector3(15,30,45) * Time.deltaTime);
	}

	void OnTriggerEnter(Collider col){
		print (col.transform.root.gameObject.name);
		if (col.transform.root.gameObject.name == "Player") {
			if (playerEnergyScript.giveEnergy (20)) {
				Destroy (gameObject);
			}
		}	

	}	
}