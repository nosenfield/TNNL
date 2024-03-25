using TMPro;
using TNNL.Collidables;
using TNNL.Events;
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
            EventAggregator.Subscribe<PointCollectionEvent>(PointCollectionListener);
        }

        void OnDestroy()
        {
            EventAggregator.Unsubscribe<PointCollectionEvent>(PointCollectionListener);
        }

        void PointCollectionListener(object e)
        {
            PointCollectionEvent pcEvent = e as PointCollectionEvent;
            int points = pcEvent.Points;
            GameObject prefab = null;
            GameObject anim = null;
            GameObject associatedObject = null;
            switch (pcEvent.AssociatedObject)
            {
                case Mine:
                    prefab = mineCollisionPointAnim;
                    associatedObject = (pcEvent.AssociatedObject as Mine).gameObject;
                    break;
                case ShieldBoost:
                    prefab = shieldCollisionPointAnim;
                    associatedObject = (pcEvent.AssociatedObject as ShieldBoost).gameObject;
                    break;
                case FinishLine:
                    prefab = finishLineCollisionPointAnim;
                    associatedObject = (pcEvent.AssociatedObject as FinishLine).gameObject;
                    break;
            }

            // NOTE
            // Animating the point values is done via an animator w/ the following properties:
            // - anchored position.y + 100
            // - text color.a fade  = 0
            // - event at end of animation to trigger gameobject removal
            //
            // Uncomment the line below to return to programmatic animation
            ///
            // Utilities.AnimateGameObjectToPosition(this, anim, anim.transform.position, anim.transform.position + new Vector3(0f, 100f, 0f), 1f, EasingFunction.EaseOutSine);

            if (prefab != null)
            {
                anim = GameObject.Instantiate(prefab, animationLayer);
                anim.GetComponentInChildren<TextMeshProUGUI>().text = points > 0 ? "+" + points.ToString() : points.ToString();
            }

            if (associatedObject != null)
            {
                Vector3 position = RectTransformUtility.WorldToScreenPoint(UnityEngine.Camera.main, associatedObject.transform.TransformPoint(Vector3.zero));
                anim.transform.position = position;
            }
        }
    }
}