using KartGame.Timeline;
using UnityEngine;

namespace KartGame.UI
{
    /// <summary>
    /// The MetaGameController is responsible for switching control between the high level
    /// contexts of the application, eg the Main Menu and Gameplay systems.
    /// </summary>
    public class MetaGameController : MonoBehaviour
    {
        [Tooltip("A reference to the main menu.")]
        public MainUIController mainMenu;
        [Tooltip("A reference to the race countdown director trigger.")]
        public DirectorTrigger raceCountdownTrigger;
        [Tooltip("The UI canvases used for game play.")]
        public Canvas[] gamePlayCanvas;
        [Tooltip("A reference for the track manager .")]
        public KartGame.Track.TrackManager trackManager;

        bool m_ShowMainCanvas = true;
        bool m_FirstTime = true;
        GameManager gameManager;

        void OnEnable()
        {
            _ToggleMainMenu(m_ShowMainCanvas);
        }

        void Start()
        {
            //Start the game immediately rather than show the pause menu.
            HandleMenuButton();
            gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        }

        public void BackToMainMenu()
        {
            if (gameManager != null)
            {
                gameManager.BackToMainMenu();
            }
        }
        public void PlayAgain()
        {
            if (gameManager != null)
            {
                gameManager.PlayAgain();

            }
        }

        /// <summary>
        /// Turns the main menu on or off.
        /// </summary>
        public void ToggleMainMenu(bool show)
        {
            if (m_ShowMainCanvas != show)
            {
                _ToggleMainMenu(show);
            }
        }

        void _ToggleMainMenu(bool show)
        {
            if (show)
            {
                // WORKAROUND: This is due to a problem where setting the time scale to 0 causes audio to stop playing.
                Time.timeScale = 0.00001f;
                mainMenu.gameObject.SetActive(true);
                foreach (var i in gamePlayCanvas) i.gameObject.SetActive(false);
            }
            else
            {
                Time.timeScale = 1;
                mainMenu.gameObject.SetActive(false);
                foreach (var i in gamePlayCanvas) i.gameObject.SetActive(true);
            }
            m_ShowMainCanvas = show;
        }

        void Update()
        {

            if (trackManager.IsRaceOver && !m_FirstTime)
            {
                ToggleMainMenu(true);
            }

            if (Input.GetButtonDown("Menu"))
            {
                HandleMenuButton();
            }
        }

        void HandleMenuButton()
        {
            ToggleMainMenu(show: !m_ShowMainCanvas);
            Time.timeScale = m_ShowMainCanvas ? 0f : 1f;

            if (m_FirstTime)
            {
                raceCountdownTrigger.TriggerDirector();
                m_FirstTime = false;
            }
        }
    }
}
