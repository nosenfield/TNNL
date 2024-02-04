using nosenfield.Logging;
using TNNL.Collidables;
using TNNL.Level;
using TNNL.Player;
using TNNL.UI.UIToolkit;
using UnityEngine;

namespace TNNL
{
    public class Main : MonoBehaviour
    {
        [SerializeField] private GameObject overlayUI;
        void Awake()
        {
            OverlayUI.ResetLevelClicked += ResetLevel;
            OverlayUI.ResetShipClicked += ResetPlayer;
            PlayerController.PlayerShipDestroyed += GameOver;
        }

        void Start()
        {
            ResetLevel();
        }

        void ResetPlayer()
        {
            PlayerController.Instance.ResetPlayer();
            StartGameplay();
        }

        void ResetLevel()
        {
            Debug.Log("Main.ResetLevel");

            LevelParser.Instance.ResetLevel();
        }

        void GameOver()
        {
            ShowMenuBar();
        }

        void StartGameplay()
        {
            HideMenuBar();

            FinishLine.FinishLineCollision += FinishLineCollisionListener;
            PlayerController.Instance.ActivatePlayer();
            Time.timeScale = 1;

            // count down the user

            // fill the shield bar

            // launch the ship
        }

        void FinishLineCollisionListener()
        {
            DefaultLogger.Instance.Log(LogLevel.DEBUG, "Player collided with finish line");
            FinishLine.FinishLineCollision -= FinishLineCollisionListener;
            PauseGameplay();
            ShowMenuBar();
        }

        void PauseGameplay()
        {
            Time.timeScale = 0;
        }

        void HideMenuBar()
        {
            overlayUI.SetActive(false);
        }

        void ShowMenuBar()
        {
            overlayUI.SetActive(true);
        }
    }
}