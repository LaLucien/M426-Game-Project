using UnityEngine;

public interface IDamageable
{
    /// <summary>
    /// Applies damage to this object.
    /// </summary>
    /// <param name="amount">Points of damage to apply.</param>
    void TakeDamage(int amount);
}
