using UnityEngine;
using System.Collections;
using Common;
using Player;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour {

	public float health;
	public GameObject healthPack;

	private Renderer[] allRenders;
	private Color originalColor;
	private Text scoreText;
	void Start() {
		allRenders = gameObject.GetComponentsInChildren<Renderer> ();
		originalColor = allRenders[0].material.color;
		scoreText = GameObject.Find ("Score").GetComponent<Text>();
	}

	void FixedUpdate () {
		if (health <= 0) {
			string enemyName = gameObject.name;
			print (enemyName + ": Player killed me!!!!!!!!");
			if (enemyName.Equals ("EnemyMech")) {
				ManageGameState.score += 100;
				//spawn health pack, 50% chance
				if (Random.Range (0, 2) == 1) {
					Transform deadTransform = gameObject.transform;
					Vector3 pos = new Vector3(deadTransform.position.x, deadTransform.position.y + 5F, deadTransform.position.z);

					GameObject.Instantiate (healthPack, pos, deadTransform.rotation);
				}
			} else {
				ManageGameState.score += 50;
			}
			scoreText.text = "Score: " + ManageGameState.score;
			ManageGameState.numEnemiesOnMap--;
			//spawn object here








			Destroy (gameObject);
		}
	}

	public void TakeDamage(float dam) {
		health -= dam;
		print ("Enemy health: " + health);
		StartCoroutine (FlashRed ());
	}

	IEnumerator FlashRed() {
		foreach (Renderer r in allRenders) {
			r.material.color = Color.red;
		}
		yield return new WaitForSeconds(0.2F);
		foreach (Renderer r in allRenders) {
			r.material.color = originalColor;
		}
	}
}
