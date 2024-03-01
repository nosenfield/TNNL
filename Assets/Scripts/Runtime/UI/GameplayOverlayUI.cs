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

        void Start()
        {
            ShieldController.ShieldCollision += ShieldCollisionListener;
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

        void ShieldCollisionListener(AbstractCollidable collidable)
        {
            GameObject prefab = null;
            int points = 0;

            switch (collidable.Type)
            {
                case CollisionType.Terrain:
                    points = playerData.TerrainCollisionPoints;
                    break;

                case CollisionType.Mine:
                    prefab = mineCollisionPointAnim;
                    points = playerData.MineCollisionPoints;
                    break;

                case CollisionType.ShieldBoost:
                    prefab = shieldCollisionPointAnim;
                    points = playerData.ShieldCollisionPoints;
                    break;
            }

            if (prefab != null)
            {
                GameObject anim = GameObject.Instantiate(prefab, animationLayer);
                anim.GetComponentInChildren<TextMeshProUGUI>().text = points > 0 ? "+" + points.ToString() : points.ToString();
                Vector3 position = RectTransformUtility.WorldToScreenPoint(UnityEngine.Camera.main, collidable.transform.TransformPoint(Vector3.zero));
                anim.transform.position = position;

                // Utilities.AnimateGameObjectToPosition(this, anim, anim.transform.position, anim.transform.position + new Vector3(0f, 100f, 0f), 1f, EasingFunction.EaseOutSine);
                // NOTE
                // Animating the point values is now done via an animator w/ the following properties:
                // - anchored position.y + 100
                // - text color.a fade  = 0
                // - event at end of animation to trigger gameobject removal
                ///
            }
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