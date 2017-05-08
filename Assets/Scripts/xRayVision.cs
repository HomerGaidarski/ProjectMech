using UnityEngine;
using System.Collections;

public class xRayVision : MonoBehaviour {
	GameObject[] enemiesOnMap;
	bool keyToggled = false;

	void FixedUpdate () {
		if (Input.GetKey (KeyCode.F)) {
			keyToggled = !keyToggled;
			if (keyToggled) {
				enemiesOnMap = GameObject.FindGameObjectsWithTag ("Enemy");
			} else {

			}
		}
	}
}
