using UnityEngine;
using System.Collections;

namespace Player
{
    public class Energy : MonoBehaviour {

        private float maxPlayerEnergy = 100;
        private float playerEnergy;

        private float lastTargetEnergy;
        private float targetEnergy;
        private float startTimeEnergy;

        private PlayerUI playerUIScript;

        void Awake()
        {
            if (GameObject.Find(GlobalVariables.PlayerUI))
            {
                playerUIScript = GameObject.Find(GlobalVariables.PlayerUI).GetComponent<PlayerUI>();
            } else
            {
                playerUIScript = null;
                print("No Player UI");
            }
        }

        void Start()
        {
            SetPlayerEnergy(maxPlayerEnergy);
            lastTargetEnergy = targetEnergy = playerEnergy;
        }

        public void UsePlayerEnergy(float energy)
        {
            if (playerUIScript != null)
            {
                lastTargetEnergy = playerUIScript.energyUI;
            }

            startTimeEnergy = Time.time;
			lastTargetEnergy = playerEnergy;
            SetPlayerEnergy(playerEnergy - energy);
            targetEnergy = playerEnergy;

            if (playerEnergy <= 0)
            {
                playerEnergy = 0;
            }
        }

        private void SetPlayerEnergy(float e)
        {
            playerEnergy = e;
            if (playerUIScript != null)
            {
                playerUIScript.UpdateEnergyBarImg();
            }
        }

		public bool giveEnergy(float e) {
			print ("my energy is " + playerEnergy);
			if (playerEnergy == maxPlayerEnergy) {
				return false;
			}
			startTimeEnergy = Time.time;
			lastTargetEnergy = playerEnergy;
			if (playerEnergy + e < maxPlayerEnergy) {
				SetPlayerEnergy(playerEnergy + e);
			} else {
				SetPlayerEnergy (maxPlayerEnergy);
			}
			targetEnergy = playerEnergy;
			return true;
		}

        public float GetEnergy()
        {
            return playerEnergy;
        }

        public float GetMaxEnergy()
        {
            return maxPlayerEnergy;
        }

        public float GetLastTargetEnergy()
        {
            return lastTargetEnergy;
        }

        public float GetTargetEnergy()
        {
            return targetEnergy;
        }

        public float GetStartTimeEnergy()
        {
            return startTimeEnergy;
        }
    }
}
