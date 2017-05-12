using UnityEngine;
using System.Collections;
using Common;
using Player;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour {

	public float health;
	public GameObject deathDropObject;

	private Renderer[] allRenders;
	private int numRenders;
	private Color originalColor;
	private Renderer mapPinRenderer;
	private Text scoreText;
	void Start() {
		allRenders = gameObject.GetComponentsInChildren<Renderer> ();
		originalColor = allRenders [0].material.color;
		mapPinRenderer = gameObject.transform.Find ("EnemyMapPin").gameObject.GetComponent<Renderer> ();
		scoreText = GameObject.Find ("Score").GetComponent<Text>();
	}

	void FixedUpdate () {
		if (health <= 0) {
			string enemyName = gameObject.name;
			print (enemyName + ": Player killed me!!!!!!!!");

			Transform deadTransform = gameObject.transform;
			Vector3 pos = new Vector3(deadTransform.position.x, deadTransform.position.y + 5F, deadTransform.position.z);

			if (enemyName.Equals ("EnemyMech")) {
				ManageGameState.score += 100;
			} else {
				ManageGameState.score += 50;
				pos.y = 5.706116f;
			}
			//spawn energy or health pack, 50% chance
			if (Random.Range (0, 2) == 1) {
				//need to implement object pooling
				GameObject.Instantiate (deathDropObject, pos, deadTransform.rotation);
			}

			scoreText.text = "Score: " + ManageGameState.score;
			ManageGameState.numEnemiesOnMap--;
			Destroy (gameObject); //need to replace with object pooling
		}
	}

	public void TakeDamage(float dam) {
		health -= dam;
		print ("Enemy health: " + health);
		StartCoroutine (FlashRed ());
	}

	IEnumerator FlashRed() {
		foreach (Renderer r in allRenders) {
			if (mapPinRenderer != r) {
				r.material.color = Color.red;
			}
		}
		yield return new WaitForSeconds(0.2F);
		foreach (Renderer r in allRenders) {
			if (mapPinRenderer != r) {
				r.material.color = originalColor;
			}
		}
	}
}
