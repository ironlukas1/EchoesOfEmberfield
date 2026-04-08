using UnityEngine;
using System;

[RequireComponent(typeof(Collider2D))]
public class PlayerHealth : MonoBehaviour
{
    public event Action<int, int> OnHealthChanged;

    [Tooltip("Starting health for the player.")]
    public int maxHealth = 100;

    [Tooltip("Current health (read-only in inspector).")]
    public int currentHealth;

    [Tooltip("Default duration for temporary invincibility windows.")]
    public float defaultInvincibilityDuration = 0.15f;

    public bool IsInvincible => _invincibilityTimer > 0f;

    private float _invincibilityTimer;

    private void Awake()
    {
        currentHealth = maxHealth;
        NotifyHealthChanged();
    }

    private void Update()
    {
        if (_invincibilityTimer > 0f)
            _invincibilityTimer -= Time.deltaTime;
    }

    /// <summary>
    /// Reduces the player's health. Does nothing if health is already zero.
    /// </summary>
    public void TakeDamage(int amount)
    {
        if (amount <= 0)
        {
            Debug.LogWarning($"PlayerHealth.TakeDamage called with non-positive amount: {amount}");
            return;
        }

        if (currentHealth <= 0)
        {
            Debug.Log("Player is already at 0 health; damage ignored.");
            return;
        }

        if (IsInvincible)
        {
            Debug.Log("Player is invincible; damage ignored.");
            return;
        }

        currentHealth = Mathf.Max(0, currentHealth - amount);
        Debug.Log($"Player took {amount} damage, health now {currentHealth}/{maxHealth}");
        NotifyHealthChanged();

        if (currentHealth == 0)
            OnDeath();
    }

    public void TriggerInvincibility(float duration = -1f)
    {
        if (duration <= 0f)
            duration = defaultInvincibilityDuration;

        _invincibilityTimer = Mathf.Max(_invincibilityTimer, duration);
    }

    private void OnDeath()
    {
        Debug.Log("Player died.");
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.TriggerGameOver();
    }

    private void NotifyHealthChanged()
    {
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
}
