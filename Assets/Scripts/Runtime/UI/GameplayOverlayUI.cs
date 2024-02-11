using TMPro;
using TNNL.Animation;
using TNNL.Collidables;
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

        PlayerData playerData;
        int lastScore;
        [SerializeField] TextMeshProUGUI scoreText;
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

        public void SetPlayerData(PlayerData playerData)
        {
            this.playerData = playerData;
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
            switch (collidable.Type)
            {
                case ShieldCollisionType.Terrain:

                    break;

                case ShieldCollisionType.Mine:
                    prefab = mineCollisionPointAnim;
                    break;

                case ShieldCollisionType.ShieldBoost:
                    prefab = shieldCollisionPointAnim;
                    break;
            }

            if (prefab != null)
            {
                GameObject anim = GameObject.Instantiate(prefab, animationLayer);
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
    }
}