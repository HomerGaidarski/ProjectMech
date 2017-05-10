using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Common
{
    public class ManageGameState : GameBehavior
    {
        public static bool isPaused;
        public GameObject inGameMenu;
        private static InGameUI inGameUIScript;
		private static AudioSource backgroundMusic;
		public static int numEnemiesOnMap = 0; //keeps track of num enemies currently alive/active on map
		public static int maxEnemiesPossibleOnMap = 40; //max num enemies that can be on the map at the same time
		public static int numEnemiesThisRound = 0; //number of enemies killed for this round
		public static int numEnemiesThisRoundMax = 10; //max number of enemies that can be spawned for this round
		public static int roundNumber = 1;
		public static float delayTime;
		private static bool printOnce = true;

		public static int score = 0;
		private static Text waveText; 
		private static Text waveTransitionText;

		//remember to run this in coroutine()
		public static IEnumerator transitionWaveDisplay() {
			print ("wave transition");
			Color color = waveTransitionText.color;
			while (color.a < 1) {
				color.a += 0.2F;
				if (color.a >= 1) {
					color.a = 1;
				}
				waveTransitionText.color = color;
				yield return new WaitForSeconds (2);
			}
			while (color.a > 0) {
				color.a -= 0.2F;
				if (color.a <= 0) {
					color.a = 0;
				}
				waveTransitionText.color = color;
				yield return new WaitForSeconds (2);
			}
		}

		public static bool needMoreEnemies() {
			if (numEnemiesThisRound >= numEnemiesThisRoundMax) { //round over
				if (numEnemiesOnMap == 0) {
					print ("Round " + roundNumber + " is over.");
					printOnce = true;
					numEnemiesThisRound = 0;
					numEnemiesThisRoundMax = (++roundNumber) * 10;
					delayTime = Time.time + 10;
				}
				return false;
			}

			if (Time.time <= delayTime) {
				return false;
			} else if (printOnce) {
				printOnce = false;
				print ("Round " + roundNumber + " started.");


				//show big text at center of screen fade in and out
				//NOT WORKING RIGHT NOW because can't do startCoroutine(transitionWaveDisplay) in static context :(
				waveTransitionText.text = waveText.text = "Wave " + roundNumber;
			}

			return numEnemiesOnMap < maxEnemiesPossibleOnMap && numEnemiesThisRound < numEnemiesThisRoundMax;
		}

        void Awake()
        {
            inGameUIScript = GameObject.Find(GlobalVariables.World).GetComponent<InGameUI>();
            isPaused = inGameMenu.activeSelf;

			backgroundMusic = GetComponent<AudioSource> ();

			numEnemiesThisRoundMax = roundNumber * 10;
			waveText = GameObject.Find ("Wave").GetComponent<Text> ();
			waveTransitionText = GameObject.Find ("WaveTransition").GetComponent<Text> ();
        }
		public void PausePlay() {
			TogglePause ();
		}

		public static void resetRoundVariables() {
			numEnemiesOnMap = 0; //keeps track of num enemies currently alive/active on map
			maxEnemiesPossibleOnMap = 30; //max num enemies that can be on the map at the same time
			numEnemiesThisRound = 0; //number of enemies spawned for this round
			numEnemiesThisRoundMax = 10; //max number of enemies that can be spawned for this round
			roundNumber = 1;
			delayTime = 0;
			printOnce = true;
		}

		public static IEnumerator GameOver()
		{
			yield return new WaitForSeconds (2);
			resetRoundVariables ();
			score = 0;
			SceneManager.LoadScene ("Main_Scene");
		}

        public static void TogglePause()
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                Time.timeScale = 0;
				backgroundMusic.Pause ();
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
            }
            else
            {
                Time.timeScale = 1;
				backgroundMusic.Play ();
				Cursor.lockState = CursorLockMode.Locked;
            }
            inGameUIScript.inGameMenuPanel.SetActive(isPaused);
        }

		public void Restart() {
			TogglePause ();
			StartCoroutine(GameOver ());
		}

		public void MainMenu() {
			TogglePause ();
			Cursor.lockState = CursorLockMode.None;
			SceneManager.LoadScene ("Splash_Screen");
		}

		public void RageQuit() {
			/*
			 * Quit is ignored in the editor. IMPORTANT: In most cases termination of application under iOS 
			 * should be left at the user discretion. Consult Apple Technical Page qa1561 for further details.
			 */
			print ("Quit if running as application.");
			Application.Quit ();
		}
    }
}
