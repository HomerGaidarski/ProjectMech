using UnityEngine;
using System.Collections;

public class frontRightEngine : MonoBehaviour 
{
	
	public GameObject dropship;
	public Rigidbody rb;
	private float curSpeed;
	private float prevRotation = 0;
	private Quaternion origRotate;
	private Vector3 prevPosition;

	private void Start()
	{
		origRotate = transform.rotation;
		prevPosition = dropship.transform.position;
	}
	// Update is called once per frame
	void Update ()
	{
		//Debug.Log (dropship.transform.rotation.y);
		//dropship has not turned at all
		if (dropship.transform.rotation.y == prevRotation) {



			if (dropship.transform.position.x != prevPosition.x || dropship.transform.position.z != prevPosition.z) {
				//Debug.Log ("goin forward");
				forward ();
			}
			else
			{
				//Debug.Log ("settin idle");
				idle ();
			}
		}
		//dropship is turning right
		else if (dropship.transform.rotation.y < prevRotation) {
			//Debug.Log ("turning right");
			rightTurn ();
		}
		//dropship is turning left
		else if (dropship.transform.rotation.y > prevRotation) {
			//Debug.Log ("turning right");
			leftTurn ();
		}
		else 
		{
		}

		prevRotation = dropship.transform.rotation.y;
		prevPosition = dropship.transform.position;
	}

	void leftTurn()
	{
		//Debug.Log ("here i am");
		Quaternion temp = transform.localRotation;
		//Debug.Log (temp.eulerAngles);
		if (temp.eulerAngles.x > 320  || temp.eulerAngles.x <= 1)
		{
			temp.eulerAngles = new Vector3 (temp.eulerAngles.x - 1f, 0f, 0f);
			transform.localRotation = temp;
		}
	}

	void rightTurn()
	{
		Quaternion temp = transform.localRotation;
		if (temp.eulerAngles.x < 40 || temp.eulerAngles.x >358)
		{
			temp.eulerAngles = new Vector3 (temp.eulerAngles.x + 1f, 0f, 0f);
			transform.localRotation = temp;
		}
	}

	void forward()
	{

		//Debug.Log ("in here");
		Quaternion temp = transform.localRotation;
		if (temp.eulerAngles.x < 60 || temp.eulerAngles.x > 310 && temp.eulerAngles.x != 0) {
			temp.eulerAngles = new Vector3 (temp.eulerAngles.x + 1f, 0f, 0f);
			transform.localRotation = temp;
		} 

	}

	void idle()
	{
		Quaternion temp = transform.localRotation;
		if (temp.eulerAngles.x < 70 && temp.eulerAngles.x != 0) {
			temp.eulerAngles = new Vector3 (temp.eulerAngles.x - 1f, 0f, 0f);
			transform.localRotation = temp;
		} 
		else if (temp.eulerAngles.x > 330 && temp.eulerAngles.x != 0)
		{
			temp.eulerAngles = new Vector3 (temp.eulerAngles.x + 1f, 0f, 0f);
			transform.localRotation = temp;
		}
		else
		{
		}
	}
}
