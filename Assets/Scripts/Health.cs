using UnityEngine;
using System.Collections;
using Common;
using UnityEngine.UI;
using Explosion;

namespace Player
{
    public class Health : GameBehavior
    {
        public float maxHealth = 100;
        private float health;
        private PlayerUI playerUIScript;

        private float lastTargetHealth;
        private float targetHealth;
        private float startTimeHealth;

		private AudioSource bulletHitSound;

		public Texture2D gameOverImage;
		public GameObject explosionPrefab;
		private ExplosionDamage explosionDmgScript;

        void Awake()
        {
            if (GameObject.Find(GlobalVariables.PlayerUI))
            {
                playerUIScript = GameObject.Find(GlobalVariables.PlayerUI).GetComponent<PlayerUI>();
            }
            else
            {
                playerUIScript = null;
                print("No Player UI");
            }
			explosionDmgScript = explosionPrefab.GetComponent<ExplosionDamage> ();
			explosionDmgScript.maxDamage = 35;
        }

        void Start()
        {
			SetHealth(maxHealth);
            lastTargetHealth = targetHealth = health;

			bulletHitSound = GetComponent<AudioSource> ();
        }
        
        public void TakeDamage(string type, float damage)
        {
            startTimeHealth = Time.time;
            lastTargetHealth = health;
            SetHealth(health - damage);
            targetHealth = health;

			if (type == "bullet") {
				bulletHitSound.Play ();
			}

            if(health <= 0)
            {
                health = 0;
                Dead();
            }
			/* for testing enemy damage
			if (health < 95) {
				print ("TIME");
				ManageGameState.TogglePause ();
			}
			*/
        }

		public void TakeDamage(float damage)
		{
			startTimeHealth = Time.time;
			lastTargetHealth = health;
			SetHealth(health - damage);
			targetHealth = health;

			if(health <= 0)
			{
				health = 0;
				Dead();
			}
		}

        private void SetHealth(float h)
        {
            health = h;
            if (playerUIScript != null)
            {
                playerUIScript.UpdateHealthBarImg();
            }
        }

		public bool giveHealth(float h) {
			print ("my health is " + health);
			if (health == maxHealth) {
				return false;
			}
			startTimeHealth = Time.time;
			lastTargetHealth = health;
			if (health + h < maxHealth) {
				SetHealth(health + h);
			} else {
				SetHealth (maxHealth);
			}
			targetHealth = health;
			return true;
		}

        private void Dead()
        {
			
            print("You are dead");
			StartCoroutine(ManageGameState.GameOver ());
        }
			
		void OnGUI() {
			if (health <= 0) {
				print ("draw gameover");
				GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), gameOverImage, ScaleMode.StretchToFill);
			} else {
				//print ("Still alive");
			}
		}

        public float GetHealth()
        {
            return health;
        }

        public float GetMaxHealth()
        {
            return maxHealth;
        }

        public float GetLastTargetHealth()
        {
            return lastTargetHealth;
        }

        public float GetTargetHealth()
        {
            return targetHealth;
        }
        
        public float GetStartTimeHealth()
        {
            return startTimeHealth;
        }
    }
}
