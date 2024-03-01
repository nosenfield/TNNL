using System;
using System.Collections.Generic;
using TMPro;
using TNNL.Animation;
using TNNL.Collidables;
using TNNL.Level;
using TNNL.Player;
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

        UserData playerData;
        [SerializeField] TextMeshProUGUI scoreText;
        [SerializeField] TextMeshProUGUI livesText;
        [SerializeField] TextMeshProUGUI levelNameDisplay;
        [SerializeField] TextMeshProUGUI highScoreDisplay;
        [SerializeField] GameObject levelInfo;
        [SerializeField] GameObject mineCollisionPointAnim;
        [SerializeField] GameObject shieldCollisionPointAnim;
        [SerializeField] RectTransform animationLayer;

        void Awake()
        {
            instance = this;
        }

        public void SetPlayerData(UserData playerData)
        {
            this.playerData = playerData;
        }

        public void UpdateUI()
        {
            scoreText.text = playerData?.TotalPoints.ToString();
            levelNameDisplay.text = LevelParser.Instance.CurrentLevelName;
            highScoreDisplay.text = LevelParser.Instance.HighScore.ToString();
        }

        void Update()
        {
            // NOTE
            // This does not need to be set every frame, but is likely cheaper/easier than implementing a PlayerPointsUpdate event
            ///

            scoreText.text = playerData?.TotalPoints.ToString();
        }

        public void GameplayStarted()
        {
            levelInfo.SetActive(false);
        }

        public void GameplayEnded()
        {
            levelInfo.SetActive(true);
        }
    }
}