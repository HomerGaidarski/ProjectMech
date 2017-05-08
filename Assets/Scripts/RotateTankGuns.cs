using UnityEngine;
using System.Collections;
using Common;

public class RotateTankGuns : MonoBehaviour {
	
	public float mouseSensitivity = 1.0F;

	float currAngle = 0.0F;
	private float maxAngle = 50.0F;
	private float minAngle = -3.0F;

	void Update () {
		if (!ManageGameState.isPaused) {
			float rotDegrees = Input.GetAxis ("Mouse Y");

			if ((currAngle >= maxAngle && rotDegrees > 0) || (currAngle <= minAngle && rotDegrees < 0)) {
				rotDegrees = 0;
			} else {
				currAngle += rotDegrees;
			}
			transform.Rotate (rotDegrees * -mouseSensitivity, 0, 0);
		}
	}
}
