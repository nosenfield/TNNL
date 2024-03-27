using TNNL.Player;
using UnityEngine;

namespace TNNL.UI
{
    public class BorderGradient : MonoBehaviour
    {
        [SerializeField] Animator animator;

        void Start()
        {
            ShieldModel.HealthUpdate += HealthUpdateListener;
            ShieldModel.InvincibilityUpdate += InvincibilityUpdateListener;
        }

        void HealthUpdateListener(float shieldHealth)
        {
            animator.SetFloat("shieldHealth", shieldHealth);
        }

        void InvincibilityUpdateListener(float secondsRemaining)
        {
            animator.SetBool("isInvincible", secondsRemaining > 0f);
        }
    }
}