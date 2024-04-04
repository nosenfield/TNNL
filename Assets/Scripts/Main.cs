using System;
using TNNL.Collidables;
using TNNL.Data;
using TNNL.Events;
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
        public static Action<PlayerMVC> SetCurrentPlayer;
        private readonly nosenfield.Logging.Logger logger = new();
        [SerializeField] private GameObject overlayUI;
        [SerializeField] private PlayerRuntimeData playerData;
        [SerializeField] private GameObject playerContainer;
        [SerializeField] private PlayerMVC currentPlayer;

        void Awake()
        {
            // OverlayUI.ResetLevelClicked += ResetLevel;
            OverlayUI.NextLevelClick += LoadNextLevelClickListener;
            OverlayUI.StartRunClick += StartRunClickListener;
            OverlayUI.ResetDataClick += ResetDataClickListener;
            PlayerController.PlayerShipDestroyed += PlayerShipDestroyedListener;
            EventAggregator.Subscribe<LevelCheckpointEvent>(LevelCheckpointListener);
        }

        void Start()
        {
            playerData = new PlayerRuntimeData(PlayerSaveData.GetAttempts());
            InitializeGame();

            GameplayOverlayUI.Instance.SetPlayerData(playerData);
            GameplayOverlayUI.Instance.UpdateUI();

            OverlayUI.SetDebugState();
        }

        void InitializeGame()
        {
            // create a new ship data instance and pass the new instance 
            playerData.ResetAttemptsAndScore();
            PlayerSaveData.SetAttempts(playerData.Attempts);

            LevelParser.Instance.ResetToFirstLevel();

            PlayerMVC.CreatePlayers(playerData, playerContainer);
            OverlayUI.SetGameplayResetState();
            GameplayOverlayUI.Instance.SetGameOverVisible(false);

            OverlayUI.SetRunButtonCopy($"Start {LevelParser.Instance.CurrentLevelName}");
        }

        void StartRunClickListener()
        {
            currentPlayer = PlayerMVC.GetNextAttempt();

            if (currentPlayer == null)
            {
                InitializeGame();
                currentPlayer = PlayerMVC.GetNextAttempt();
            }

            // set the camera and UI to reference the new player and dispatch events to reset UI
            SetCurrentPlayer.Invoke(currentPlayer);
            PlayerMVC.SetCurrentPlayer(currentPlayer);

            if (currentPlayer.AttemptData.CheckpointReached == LevelParser.Instance.CurrentLevelId)
            {
                // NOTE
                // resetting the shields is performed when the player completes all runs on a level
                // see Main.StoppingLineReachedHandler();
                ///
                PlayerMVC.PrepForNextLevel();
                LevelParser.Instance.LoadNextLevel();
            }

            GameplayOverlayUI.Instance.UpdateUI();
            GameplayOverlayUI.Instance.HideOverlay();

            OverlayUI.SetGameplayActiveState();
            OverlayUI.SetVisible(false);

            StoppingLine.Collision += StoppingLineReachedHandler;
            currentPlayer.Controller.ActivatePlayerControls();
            currentPlayer.Controller.StartShipMovement();
        }

        void LoadNextLevelClickListener()
        {
            LevelParser.Instance.LoadNextLevel();
            GameplayOverlayUI.Instance.UpdateUI();
        }

        void LevelCheckpointListener(object e)
        {
            // Stop player input for this ship and center it
            currentPlayer.Controller.DeactivatePlayerControls();
            currentPlayer.Controller.EnterLevelCompleteTransitionState();

            // Add score updates
            RecordScore();
            GameplayOverlayUI.Instance.UpdateUI();

            // Record progress
            currentPlayer.AttemptData.CheckpointReached = LevelParser.Instance.CurrentLevelId;
            PlayerSaveData.Save();

            // push other ships forward
            PlayerMVC.ClearDockingSpaceForShip(currentPlayer);
        }

        void StoppingLineReachedHandler()
        {
            logger.Log(nosenfield.Logging.LogLevel.DEBUG, "StoppingLineReachedHandler()");

            StoppingLine.Collision -= StoppingLineReachedHandler;
            currentPlayer.Controller.StopShipMovement();

            // show UI
            GameplayOverlayUI.Instance.ShowOverlay();

            UpdateRunButtonCopy();
            OverlayUI.SetVisible(true);
        }

        void PlayerShipDestroyedListener()
        {
            // Stop movement of this ship
            currentPlayer.Controller.DeactivatePlayerControls();
            currentPlayer.Controller.StopShipMovement();

            // Add score updates
            RecordScore();
            GameplayOverlayUI.Instance.UpdateUI();

            // Record progress
            currentPlayer.AttemptData.IsAlive = false;
            PlayerSaveData.Save();

            // hide the dead ship
            currentPlayer.View.gameObject.SetActive(false);

            // show UI
            if (PlayerMVC.GetNextAttempt() == null)
            {
                OverlayUI.SetGameplayResetState();
                GameplayOverlayUI.Instance.SetGameOverVisible(true);
            }

            GameplayOverlayUI.Instance.ShowOverlay();

            UpdateRunButtonCopy();
            OverlayUI.SetVisible(true);
        }

        void RecordScore()
        {
            if (playerData.TotalPoints > PlayerSaveData.GetHighScore(LevelParser.Instance.CurrentLevelId))
            {
                PlayerSaveData.SetHighScore(LevelParser.Instance.CurrentLevelId, playerData.TotalPoints);
            }
        }

        void ResetDataClickListener()
        {
            PlayerSaveData.ResetData();
            PlayerSaveData.Save();
            GameplayOverlayUI.Instance.UpdateUI();
        }

        void UpdateRunButtonCopy()
        {
            PlayerMVC nextAttempt = PlayerMVC.GetNextAttempt();
            if (nextAttempt == null)
            {
                OverlayUI.SetGameplayResetState();
                GameplayOverlayUI.Instance.SetGameOverVisible(true);
                OverlayUI.SetRunButtonCopy("New Game");
            }
            else if (nextAttempt.AttemptData.CheckpointReached == LevelParser.Instance.CurrentLevelId)
            {
                OverlayUI.SetRunButtonCopy($"Start {LevelParser.Instance.NextLevelName}");
            }
            else
            {
                OverlayUI.SetRunButtonCopy($"Start Next Run");
            }
        }
    }
}