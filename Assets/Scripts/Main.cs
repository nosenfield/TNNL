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
        private readonly nosenfield.Logging.Logger logger = new();
        [SerializeField] private GameObject overlayUI;
        [SerializeField] private PlayerRuntimeData playerData;
        [SerializeField] private GameObject playerContainer;

        void Awake()
        {
            OverlayUI.ResetLevelClicked += ResetLevel;
            OverlayUI.StartRunClicked += StartRun;
            OverlayUI.NextLevelClicked += LoadNextLevel;
            PlayerController.PlayerShipDestroyed += GameOver;
        }

        void Start()
        {
            playerData = new PlayerRuntimeData(PlayerSaveData.GetShipData());
            PlayerMVC.CreatePlayers(playerData, playerContainer);

            InitializeGame();
            GameplayOverlayUI.Instance.SetPlayerData(playerData);
            GameplayOverlayUI.Instance.UpdateUI();
        }

        void InitializeGame()
        {
            playerData.ResetPlayerData();
            PlayerSaveData.SetShipData(playerData.ShipData);
            PlayerMVC.CreatePlayers(playerData, playerContainer);
            ResetLevel();

        }

        void StartRun()
        {
            if (PlayerMVC.GetCurrentShip() == null)
            {
                InitializeGame();
            }

            GameplayOverlayUI.Instance.UpdateUI();
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

            // Record progress
            PlayerMVC.GetCurrentShip().ShipData.IsAlive = false;

            // show UI
            GameplayOverlayUI.Instance.GameplayEnded();
            ShowMenuBar();
        }

        void StartGameplay()
        {
            HideMenuBar();

            EventAggregator.Subscribe<LevelCheckpointEvent>(LevelCheckpointListener);
            PlayerMVC.GetCurrentShip().Controller.Activate();

            // count down the user

            // fill the shield bar

            // launch the ship
        }

        void LevelCheckpointListener(object e)
        {
            LevelCheckpointEvent levelCheckpointEvent = (LevelCheckpointEvent)e;
            EventAggregator.Unsubscribe<LevelCheckpointEvent>(LevelCheckpointListener);

            EnterTransitionState();

            // Add score updates
            RecordScore();
            GameplayOverlayUI.Instance.UpdateUI();

            // Record progress
            PlayerMVC.GetCurrentShip().ShipData.CheckpointReached = LevelParser.Instance.CurrentLevelId;


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

        void EnterTransitionState()
        {
            PlayerMVC.GetCurrentShip().Controller.EnterTransitionState();
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