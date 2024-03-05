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
    public class CollisionPointsGenerator : MonoBehaviour
    {
        [SerializeField] GameObject mineCollisionPointAnim;
        [SerializeField] GameObject shieldCollisionPointAnim;
        [SerializeField] GameObject finishLineCollisionPointAnim;
        [SerializeField] RectTransform animationLayer;

        void Start()
        {
            ShieldController.ShieldCollision += CollisionListener;
            ShipController.ShipCollision += CollisionListener;
        }

        void OnDestroy()
        {
            ShieldController.ShieldCollision -= CollisionListener;
            ShipController.ShipCollision -= CollisionListener;
        }

        // NOTE
        // Replace CollisionListener through the ShipController/ShieldController with a Collision or PointsEarned event raised through an event hub
        ///
        void CollisionListener(AbstractCollidable collidable)
        {
            int points = collidable.CollisionPoints;
            GameObject prefab = null;
            switch (collidable.Type)
            {
                case CollisionType.Mine:
                    prefab = mineCollisionPointAnim;
                    break;
                case CollisionType.ShieldBoost:
                    prefab = shieldCollisionPointAnim;
                    break;
                case CollisionType.FinishLine:
                    prefab = finishLineCollisionPointAnim;
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
    }
}