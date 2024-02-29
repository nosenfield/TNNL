using nosenfield.Logging;
using TNNL.Collidables;
using TNNL.Level;
using TNNL.Player;
using TNNL.UI;
using TNNL.UI.UIToolkit;
using UnityEngine;

namespace TNNL
{
    public class Main : MonoBehaviour
    {
        [SerializeField] private GameObject overlayUI;
        [SerializeField] private UserData playerData;

        void Awake()
        {
            OverlayUI.ResetLevelClicked += ResetLevel;
            OverlayUI.StartRunClicked += StartRun;
            OverlayUI.NextLevelClicked += LoadNextLevel;
            PlayerController.PlayerShipDestroyed += GameOver;

            playerData = new UserData();
        }

        void Start()
        {
            ResetLevel();
            GameplayOverlayUI.Instance.SetPlayerData(playerData);
            GameplayOverlayUI.Instance.UpdateUI();
        }

        void StartRun()
        {
            playerData.CurrentRun--;
            if (playerData.CurrentRun <= 0)
            {
                ResetLevel();
                playerData.ResetPlayerData();
            }

            GameplayOverlayUI.Instance.UpdateUI();

            PlayerController.Instance.ResetPlayer();
            GameplayOverlayUI.Instance.GameplayStarted();
            StartGameplay();
        }

        void ResetLevel()
        {
            LevelParser.Instance.ResetLevel();
        }

        void LoadNextLevel()
        {
            LevelParser.Instance.LoadNextLevel();
            playerData.ResetPlayerData();
            GameplayOverlayUI.Instance.UpdateUI();
        }

        void GameOver()
        {
            GameplayOverlayUI.Instance.GameplayEnded();
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

            RecordScore();

            GameplayOverlayUI.Instance.UpdateUI();

            PauseGameplay();
            GameplayOverlayUI.Instance.GameplayEnded();
            ShowMenuBar();
        }

        void RecordScore()
        {
            if (playerData.TotalPoints > LevelParser.Instance.HighScore)
            {
                LevelParser.Instance.HighScore = playerData.TotalPoints;
            }
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