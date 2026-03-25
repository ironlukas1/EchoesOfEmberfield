using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerHealth : MonoBehaviour
{
    [Tooltip("Starting health for the player.")]
    public int maxHealth = 5;

    [Tooltip("Current health (read-only in inspector).")]
    public int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Reduces the player's health. Does nothing if health is already zero.
    /// </summary>
    public void TakeDamage(int amount)
    {
        if (amount <= 0 || currentHealth <= 0)
            return;

        currentHealth = Mathf.Max(0, currentHealth - amount);
        Debug.Log($"Player took {amount} damage, health now {currentHealth}/{maxHealth}");

        if (currentHealth == 0)
            OnDeath();
    }

    private void OnDeath()
    {
        Debug.Log("Player died.");
        // TODO: Add death behavior (respawn, game over, etc.)
    }
}
