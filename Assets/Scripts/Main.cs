using nosenfield.Logging;
using TNNL.Collidables;
using TNNL.Data;
using TNNL.Level;
using TNNL.Player;
using TNNL.RuntimeData;
using TNNL.UI;
using TNNL.UI.UIToolkit;
using UnityEngine;

namespace TNNL
{
    public class Main : MonoBehaviour
    {
        private readonly nosenfield.Logging.Logger logger = new();
        [SerializeField] private GameObject overlayUI;
        [SerializeField] private PlayerRuntimeData playerData;

        void Awake()
        {
            OverlayUI.ResetLevelClicked += ResetLevel;
            OverlayUI.StartRunClicked += StartRun;
            OverlayUI.NextLevelClicked += LoadNextLevel;
            PlayerController.PlayerShipDestroyed += GameOver;

            playerData = new PlayerRuntimeData();
        }

        void Start()
        {
            ResetLevel();
            GameplayOverlayUI.Instance.SetPlayerData(playerData);
            GameplayOverlayUI.Instance.UpdateUI();
        }

        void StartRun()
        {
            playerData.ShipsRemaining--;
            if (playerData.ShipsRemaining <= 0)
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

            // Add score updates
            RecordScore();
            GameplayOverlayUI.Instance.UpdateUI();

            // show UI
            GameplayOverlayUI.Instance.GameplayEnded();
            ShowMenuBar();
        }

        void StartGameplay()
        {
            HideMenuBar();

            FinishLine.FinishLineCollision += FinishLineCollisionListener;
            PlayerController.Instance.ActivatePlayer();

            // count down the user

            // fill the shield bar

            // launch the ship
        }

        void FinishLineCollisionListener()
        {
            FinishLine.FinishLineCollision -= FinishLineCollisionListener;

            PauseGameplay();

            // Add score updates
            RecordScore();
            GameplayOverlayUI.Instance.UpdateUI();

            // show UI
            GameplayOverlayUI.Instance.GameplayEnded();
            ShowMenuBar();
        }

        void RecordScore()
        {
            if (playerData.TotalPoints > PlayerSaveData.GetHighScore(LevelParser.Instance.CurrentLevelId))
            {
                PlayerSaveData.SetHighScore(LevelParser.Instance.CurrentLevelId, playerData.TotalPoints);
            }
        }

        void PauseGameplay()
        {
            PlayerController.Instance.PausePlayer();
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