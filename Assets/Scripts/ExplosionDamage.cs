using UnityEngine;
using System.Collections;

namespace Explosion{
public class ExplosionDamage : MonoBehaviour {

	public float radius;
	public float maxDamage = 35;

	private bool didOnce = false;
	private ArrayList parents = new ArrayList();

	void FixedUpdate () {
		if (!didOnce) {
			Collider[] hitColliders = Physics.OverlapSphere (transform.position, radius, 1 << 11); 
			foreach (Collider col in hitColliders) {
				GameObject parent = col.transform.root.gameObject;
				if (!parents.Contains (parent)) {
					parents.Add (parent);
					EnemyHealth script = parent.GetComponent<EnemyHealth> ();

					float distanceFromExplosionOrigin = Vector3.Distance (transform.position, col.transform.position);
					print (parent.name + " was hit at a distance of: " + distanceFromExplosionOrigin + " from the explosion.");

					if (distanceFromExplosionOrigin >= maxDamage)
						distanceFromExplosionOrigin = maxDamage - 5;

					script.TakeDamage (maxDamage - distanceFromExplosionOrigin);
				}
			}
			didOnce = true;
		}
	}
}
}
