using TMPro;
using TNNL.Data;
using TNNL.Level;
using TNNL.RuntimeData;
using UnityEngine;

namespace TNNL.UI
{
    public class GameplayOverlayUI : MonoBehaviour
    {
        private static GameplayOverlayUI instance;
        public static GameplayOverlayUI Instance
        {
            get
            {
                return instance;
            }
        }

        PlayerRuntimeData playerData;
        [SerializeField] TextMeshProUGUI scoreText;
        [SerializeField] TextMeshProUGUI livesText;
        [SerializeField] TextMeshProUGUI levelNameDisplay;
        [SerializeField] TextMeshProUGUI highScoreDisplay;
        [SerializeField] GameObject levelInfo;
        [SerializeField] GameObject gameOverText;

        void Awake()
        {
            instance = this;
        }

        public void SetPlayerData(PlayerRuntimeData playerData)
        {
            this.playerData = playerData;
        }

        public void UpdateUI()
        {
            scoreText.text = playerData?.TotalPoints.ToString();
            levelNameDisplay.text = LevelParser.Instance.CurrentLevelName;

            highScoreDisplay.text = PlayerSaveData.GetHighScore(LevelParser.Instance.CurrentLevelId).ToString();
        }

        public void SetGameOverVisible(bool visible)
        {
            gameOverText.SetActive(visible);
        }

        void Update()
        {
            // NOTE
            // This does not need to be set every frame, but is likely cheaper/easier than implementing a PlayerPointsUpdate event
            ///

            scoreText.text = playerData?.TotalPoints.ToString();
        }

        public void HideOverlay()
        {
            levelInfo.SetActive(false);
        }

        public void ShowOverlay()
        {
            levelInfo.SetActive(true);
        }
    }
}