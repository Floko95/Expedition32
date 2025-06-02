using System;
using UnityEngine;

[Serializable]
public class HealthSystem
{
    public event Action OnHealthChanged;
    public event Action OnHealthMaxChanged;
    public event Action OnDamaged;
    public event Action OnRevived;
    public event Action OnHealed;
    public event Action OnDead;

    [SerializeField] private float healthMax;
    [SerializeField] private float health;

    /// <summary>
    /// Construct a HealthSystem, receives the health max and sets current health to that value
    /// </summary>
    public HealthSystem(float healthMax)
    {
        this.healthMax = healthMax;
        health = healthMax;
        OnHealthChanged?.Invoke();
    }

    /// <summary>
    /// Get the current health
    /// </summary>
    public float GetHealth()
    {
        return health;
    }

    /// <summary>
    /// Get the current max amount of health
    /// </summary>
    public float GetHealthMax()
    {
        return healthMax;
    }

    /// <summary>
    /// Get the current Health as a Normalized value (0-1)
    /// </summary>
    public float GetHealthNormalized()
    {
        return health / healthMax;
    }

    /// <summary>
    /// Deal damage to this HealthSystem
    /// </summary>
    public virtual void Damage(float amount)
    {
        health -= amount;
        if (health < 0)
        {
            health = 0;
        }
        OnHealthChanged?.Invoke();
        OnDamaged?.Invoke();

        if (health <= 0)
        {
            Die();
        }
    }

    public virtual void Revive(float health) {
        if(IsAlive) return;
        Heal(health);
        
        OnRevived?.Invoke();
    }
    
    /// <summary>
    /// Kill this HealthSystem
    /// </summary>
    public virtual void Die()
    {
        OnDead?.Invoke();
    }

    /// <summary>
    /// Test if this Health System is dead
    /// </summary>
    public bool IsDead => health <= 0;
    public bool IsAlive => health > 0;

    /// <summary>
    /// Heal this HealthSystem
    /// </summary>
    public void Heal(float amount)
    {
        health += amount;
        if (health > healthMax)
        {
            health = healthMax;
        }
        OnHealthChanged?.Invoke();
        OnHealed?.Invoke();
    }

    /// <summary>
    /// Heal this HealthSystem to the maximum health amount
    /// </summary>
    public void HealComplete()
    {
        health = healthMax;
        OnHealthChanged?.Invoke();
        OnHealed?.Invoke();
    }

    /// <summary>
    /// Set the Health Amount Max, optionally also set the Health Amount to the new Max
    /// </summary>
    public void SetHealthMax(float healthMax)
    {
        var diff = healthMax - this.healthMax;
        this.healthMax = healthMax;
        if(diff > 0) {
            Heal(diff);
        } else {
            health = Mathf.Min(health, this.healthMax);
        }
        OnHealthMaxChanged?.Invoke();
        OnHealthChanged?.Invoke();
    }

    /// <summary>
    /// Set the current Health amount, doesn't set above healthAmountMax or below 0
    /// </summary>
    /// <param name="health"></param>
    public void SetHealth(float health)
    {
        if (health > healthMax)
        {
            health = healthMax;
        }
        if (health < 0)
        {
            health = 0;
        }
        this.health = health;
        OnHealthChanged?.Invoke();

        if (health <= 0)
        {
            Die();
        }
    }
}
