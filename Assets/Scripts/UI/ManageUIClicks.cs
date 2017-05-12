using UnityEngine;
using UnityEngine.UI;
using Player;
using UnityEngine.SceneManagement;

namespace Common
{
    public class ManageUIClicks : GameBehavior
    {
        [Header("Main Menu")]
        public Button newGameBtn;
        public Button continueGameBtn;
        public Button optionsBtn;

		public GameObject optionsImage;

        void Start()
        {
            //DontDestroyOnLoad(transform.gameObject);
            SetIsUIFocusedEvent();

            newGameBtn.onClick.AddListener(() =>
            {
				StartCoroutine(ManageGameState.GameOver());
            });

            continueGameBtn.onClick.AddListener(() =>
            {
                //Function call to continue
                print("TODO: Add functionality to continue.");
            });

            optionsBtn.onClick.AddListener(() =>
            {
                //Function call to options
				showOptions();
            });
        }

        private void SetIsUIFocusedEvent()
        {
            //This is an example of how to call Focusable
            //Focusable(panel);
        }

		private bool displayOptions = false;
		public void showOptions() {
			optionsImage.SetActive (displayOptions = !displayOptions);
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
