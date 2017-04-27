using UnityEngine;
using System.Collections;
using Common;

public class moveTo : MonoBehaviour
{
	public int health;
	public float rSpeed;
	private GameObject curDest;
	public GameObject waypoint;
	public GameObject exitPoint;
	public int atDropzone = 0;
	private float timeDead = 0;
	private float timeAtDrop = 0;

	private GameObject[] enemyMechDrops = new GameObject[4];
	private GameObject[] enemyMechAttaches = new GameObject[4];
	private ObjectPooling mechPoolScript;

	private GameObject[] droneDrops = new GameObject[6];
	private GameObject[] droneAttaches = new GameObject[6];
	private ObjectPooling dronePoolScript;

	private Vector3 startingPos;

	void Start()
	{
		startingPos = transform.position;
		curDest = waypoint;

		mechPoolScript = GameObject.Find ("EnemyMechPooler").GetComponent<ObjectPooling> ();
		dronePoolScript = GameObject.Find ("DronePooler").GetComponent<ObjectPooling> ();

		findAttachPoints ("enemyMech", enemyMechAttaches);
		findAttachPoints ("drone", droneAttaches);

		AttachEnemies ();
	}
	
	private void findAttachPoints(string enemyName, GameObject[] attachPoints) {
		for (int i = 0; i < attachPoints.Length; i++) {
			string name = enemyName + "_spawn" + i;
			//print (name);
			GameObject attachPoint = transform.FindChild (name).gameObject;
			if (attachPoint != null) {
				attachPoints [i] = attachPoint;
			} else {
				//print (name + " is null");
			}
		}
	}

	IEnumerator waitUntilNeeded() {
		yield return new WaitUntil (() => ManageGameState.needMoreEnemies () && transform.position == startingPos);
		AttachEnemies ();
	}


	void Update(){
		if (!ManageGameState.needMoreEnemies ()) {
			StartCoroutine (waitUntilNeeded ());
		}

		if (hasEnemiesToDrop || transform.position != startingPos) {
			if (health > 0) {
				if (Vector3.Dot (transform.forward, (curDest.transform.position - transform.position).normalized) > .99f && atDropzone == 0) {
					float step = 30 * Time.deltaTime;
					transform.position = Vector3.MoveTowards (transform.position, curDest.transform.position, step);
					if (transform.position == curDest.transform.position) {
						atDropzone = 1;
						//Debug.Log ("im here");
					}
				} else if (atDropzone == 0) {

					Vector3 targetDir = curDest.transform.position - transform.position;
					float step = rSpeed * Time.deltaTime;
					Vector3 newDir = Vector3.RotateTowards (transform.forward, targetDir, step, 0.0F);
					Debug.DrawRay (transform.position, newDir, Color.red);
					transform.rotation = Quaternion.LookRotation (newDir);
					//print ("DROP");
				}
				if (atDropzone == 1) {
					if (timeAtDrop < 5) {
						float step = 10 * Time.deltaTime;
						transform.position = Vector3.MoveTowards (transform.position, (curDest.transform.position + (new Vector3 (0f, -30f, 0f))), step);
						timeAtDrop += Time.deltaTime;
						//print ("dropping the payload");
					} else {
					
						float step = 10 * Time.deltaTime;
						transform.position = Vector3.MoveTowards (transform.position, curDest.transform.position, step);
						if (transform.position == curDest.transform.position) {
							atDropzone = 0;
							curDest = exitPoint;
						}
					}
				}

				if (timeAtDrop > 4) {
					if (hasEnemiesToDrop) {
						DropEnemies ();
						timeToReloadEnemies = true;
					}
				}

				if (transform.position == exitPoint.transform.position) {
					//print ("resetting");
					reset ();
				}
			} else {
				if (timeDead > 4) {
					transform.position = new Vector3 (0f, -50f, 0f);
					reset ();
				} else {
					transform.position -= new Vector3 (0f, .3f, 0f);
					timeDead += Time.deltaTime;
				}
			}
		}
	}

	private bool hasEnemiesToDrop = false;
	private void DropEnemies() {
		if (hasEnemiesToDrop) {
			//print ("you know our motto, we deliva!");
			DropEnemyOfType (enemyMechDrops);
			DropEnemyOfType (droneDrops);
			hasEnemiesToDrop = false;
		}
	}

	private void DropEnemyOfType(GameObject[] enemies) {
		foreach (GameObject enemy in enemies) {
			enemy.transform.parent = null;
			MonoBehaviour[] scripts = enemy.GetComponents<MonoBehaviour> ();
			foreach (MonoBehaviour script in scripts) {
				script.enabled = true;
			}
			enemy.GetComponent<NavMeshAgent> ().enabled = true;
			Collider col = enemy.GetComponent<BoxCollider> ();
			if (col != null) {
				col.enabled = true;
			}
		}
	}

	private void AttachEnemies() {
		if (ManageGameState.needMoreEnemies ()) {
			timeToReloadEnemies = false;
			//print ("attach");
			attachEnemiesOfType (mechPoolScript, enemyMechAttaches, enemyMechDrops);
			attachEnemiesOfType (dronePoolScript, droneAttaches, droneDrops);
			timeAtDrop = 0;
			hasEnemiesToDrop = true;
		}
	}

	private void attachEnemiesOfType(ObjectPooling poolScript, GameObject[] attaches, GameObject[] enemies) {
		for (int i = 0; i < attaches.Length; i++) {
			GameObject enemy = poolScript.GetNextObject ();
			if (enemy == null) {
				print ("broken");
			}
			GameObject attachPoint = attaches [i];
			//print (attachPoint);
			enemy.transform.parent = gameObject.transform;
			enemy.transform.position = attachPoint.transform.position;
			enemy.transform.rotation = attachPoint.transform.rotation;
			enemies [i] = enemy;
			ManageGameState.numEnemiesOnMap++;
			ManageGameState.numEnemiesThisRound++;
		}
	}

	private bool timeToReloadEnemies = true;
	public void reset()
	{
		if (ManageGameState.needMoreEnemies()) {
			if (timeToReloadEnemies) {
				if (transform.position == exitPoint.transform.position)
					AttachEnemies ();
			} else {
				//print ("you're calling me to many times brah! :(");
			}
			
			atDropzone = 0;
			curDest = waypoint;
			health = 150;
		}
	}

	public float getTimeAtDrop(){
		
		return timeAtDrop;
	}
}