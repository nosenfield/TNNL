using TNNL.Events;
using UnityEngine;

namespace TNNL.UI
{
    public class BorderGradient : MonoBehaviour
    {
        [SerializeField] Animator animator;

        void Awake()
        {

        }

        void Start()
        {
            EventAggregator.Subscribe<ShieldHealthUpdateEvent>(ShieldHealthUpdateEventListener);
            EventAggregator.Subscribe<ShieldInvincibilityUpdateEvent>(ShieldInvincibilityUpdateEventListener);
        }

        void ShieldHealthUpdateEventListener(object e)
        {
            ShieldHealthUpdateEvent shieldHealthUpdateEvent = (ShieldHealthUpdateEvent)e;

            animator.SetFloat("shieldHealth", shieldHealthUpdateEvent.PercentHealth);
        }

        void ShieldInvincibilityUpdateEventListener(object e)
        {
            ShieldInvincibilityUpdateEvent shieldInvincibilityUpdateEvent = (ShieldInvincibilityUpdateEvent)e;
            animator.SetBool("isInvincible", shieldInvincibilityUpdateEvent.SecondsInvincibleRemaining > 0f);
        }
    }
}