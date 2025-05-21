// Health.cs
using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private bool useAnimation = true;
    public int CurrentHealth { get; private set; }

    // Events
    public event Action<int, int> OnHealthChanged; // (current, max)
    public event Action OnDied;

    private Animator m_animator;

    void Awake()
    {
        CurrentHealth = maxHealth;
        // Optional animation use so both the hero knight (who has animations) and potentially
        // other non-animated enemies can use this. Need to make sure useAnimation is always properly set.
        if (useAnimation)
            m_animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Reduces health by damageAmount (clamped at zero), invokes OnHealthChanged,
    /// and triggers death if health drops to zero.
    /// </summary>
    public void TakeDamage(int damageAmount)
    {
        if (damageAmount <= 0 || CurrentHealth <= 0)
            return;

        CurrentHealth = Mathf.Max(CurrentHealth - damageAmount, 0);

        if (CurrentHealth == 0)
        {
            Die();
            return;
        }

        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);

        if (useAnimation && m_animator != null)
            m_animator.SetTrigger("Hurt");

    }

    /// <summary>
    /// Increases health by healAmount (clamped at maxHealth),
    /// and invokes OnHealthChanged.
    /// </summary>
    public void Heal(int healAmount)
    {
        if (healAmount <= 0 || CurrentHealth <= 0)
            return;

        CurrentHealth = Mathf.Min(CurrentHealth + healAmount, maxHealth);
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
    }

    private void Die()
    {
        OnDied?.Invoke();

        if (useAnimation && m_animator != null)
            m_animator.SetTrigger("Death");
    }
}
