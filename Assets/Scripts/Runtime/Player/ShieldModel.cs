using System;
using UnityEngine;
using UnityEngine.Events;

namespace TNNL.Player
{
    [Serializable]
    public class ShieldModel
    {
        public static float DefaultShieldStartingHealth = 10f;
        public static float DefaultShieldMaxHealth = 10f;

        /// <summary>
        /// HealthUpdate emits the new percentage of health as a float between 0 and 1. this currently throws an error because it has no listeners.
        /// Who needs to listen to this event and how will they get the reference to the event through this model which is instantiated by the controller?
        /// Emit a registration as the "active" model?
        /// </summary>
        public static Action<float> HealthUpdate;
        public static Action<float> InvincibilityUpdate;

        [SerializeField] private float health;
        [SerializeField] private float maxHealth;
        [SerializeField] private float startingHealth;
        [SerializeField] private float secondsInvincibleRemaining;
        public float SecondsInvincibleRemaining
        {
            get
            {
                return secondsInvincibleRemaining;
            }
            set
            {
                secondsInvincibleRemaining = value;
                InvincibilityUpdate?.Invoke(value);
            }
        }
        public float PercentHealth
        {
            get
            {
                return health / maxHealth;
            }
        }

        public ShieldModel(float startingHealth, float maxHealth)
        {
            this.health = this.startingHealth = startingHealth;
            this.maxHealth = maxHealth;
            this.SecondsInvincibleRemaining = 0f;
        }

        /// <summary>
        /// the amount to remove from the current shield health. amount must be >= 0
        /// </summary>
        /// <param name="amount">must be >= 0</param>
        /// <returns>the new shield health</returns>
        /// <exception cref="System.Exception">This is the exception text.</exception>
        public float DamageShield(float amount)
        {

            if (amount < 0) throw new System.Exception("Damage amount specificed is less than 0");
            if (SecondsInvincibleRemaining == 0f)
            {
                ChangeShield(-amount);
            }

            return health;
        }

        /// <summary>
        /// The amount to add to the current shield health. amount must be >= 0
        /// </summary>
        /// <param name="amount">must be >= 0</param>
        /// <returns>the new shield health</returns>
        /// <exception cref="System.Exception">This is the exception text.</exception>
        public float HealShield(float amount)
        {
            if (amount < 0) throw new System.Exception("Heal amount specificed is less than 0");
            ChangeShield(amount);
            return health;
        }

        private void ChangeShield(float amount)
        {
            health += amount;

            // don't allow the health to go below 0 as a negative scale creates a reversed, but visible view component
            if (health < 0f)
            {
                health = 0f;
            }

            HealthUpdate.Invoke(PercentHealth);
        }

        public float ResetShield()
        {
            health = startingHealth;
            HealthUpdate.Invoke(PercentHealth);
            return health;
        }
    }
}