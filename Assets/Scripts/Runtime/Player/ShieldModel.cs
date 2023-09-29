using UnityEngine;
using UnityEngine.Events;

// [CreateAssetMenu(fileName = "ShieldModel", menuName = "ScriptableObjects/ShieldModel")]
public class ShieldModel
{
    public static float DefaultShieldStartingHealth = 10f;
    public static float DefaultShieldMaxHealth = 10f;

    /// <summary>
    /// HealthUpdate emits the new percentage of health as a float between 0 and 1
    /// </summary>
    public UnityEvent<float> HealthUpdate;

    private float health;
    private float maxHealth;
    private float startingHealth;
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
        ChangeShield(-amount);
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
        HealthUpdate.Invoke(PercentHealth);
    }

    public float ResetShield()
    {
        health = startingHealth;
        HealthUpdate.Invoke(PercentHealth);
        return health;
    }
}