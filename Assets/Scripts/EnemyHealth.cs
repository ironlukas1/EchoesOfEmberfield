using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyHealth : MonoBehaviour
{
    [Tooltip("Max health for this enemy.")]
    public int maxHealth = 3;

    [Tooltip("Current health (read-only in inspector).")]
    public int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0 || currentHealth <= 0)
            return;

        currentHealth = Mathf.Max(0, currentHealth - amount);
        Debug.Log($"{name} took {amount} damage, health now {currentHealth}/{maxHealth}");

        if (currentHealth == 0)
            Die();
    }

    private void Die()
    {
        // TODO: add death effect (particles, sound, score, etc.)
        Destroy(gameObject);
    }
}
