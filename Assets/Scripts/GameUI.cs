using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    [Header("TextMeshPro (recommended)")]
    public TMP_Text healthText;
    public TMP_Text healthValueText;
    public TMP_Text dashCountText;
    public TMP_Text dashCooldownText;
    public TMP_Text statusText;
    public TMP_Text controlsText;

    [Header("Legacy fallback")]
    public Text healthTextLegacy;
    public Text healthValueTextLegacy;
    public Text dashCountTextLegacy;
    public Text dashCooldownTextLegacy;
    public Text statusTextLegacy;
    public Text controlsTextLegacy;

    public PlayerHealth playerHealth;
    public Dash playerDash;

    private int _lastHealth = int.MinValue;
    private int _lastMaxHealth = int.MinValue;

    private void Awake()
    {
        if (playerHealth == null)
        {
            var playerGo = GameObject.FindGameObjectWithTag("Player");
            if (playerGo != null)
                playerHealth = playerGo.GetComponent<PlayerHealth>();
        }

        if (playerDash == null)
        {
            var playerGo = GameObject.FindGameObjectWithTag("Player");
            if (playerGo != null)
                playerDash = playerGo.GetComponent<Dash>();
        }

        SetText(controlsText, controlsTextLegacy, "LMB: Attack | Space: Dash | ESC: Pause");

        SubscribeHealth();

        UpdateUI();
    }

    private void OnEnable()
    {
        SubscribeHealth();
    }

    private void OnDisable()
    {
        UnsubscribeHealth();
    }

    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (playerHealth != null)
            UpdateHealthText(playerHealth.currentHealth, playerHealth.maxHealth);

        if (playerDash != null)
            SetText(dashCountText, dashCountTextLegacy, $"Dash: {playerDash.CurrentDashCount}/{playerDash.MaxDashCount}");

        if (playerDash != null)
        {
            var cooldown = playerDash.DashCooldownRemaining;
            SetText(dashCooldownText, dashCooldownTextLegacy, cooldown > 0 ? $"Dash CD: {cooldown:F1}s" : "Dash Ready");
        }

        var gsm = GameStateManager.Instance;
        if (gsm != null && gsm.IsGameOver)
            SetText(statusText, statusTextLegacy, "GAME OVER\nPress R to Restart");
        else if (gsm != null && gsm.IsPaused)
            SetText(statusText, statusTextLegacy, "PAUSED\nPress ESC to Resume");
        else
            SetText(statusText, statusTextLegacy, string.Empty);
    }

    private static void SetText(TMP_Text tmp, Text legacy, string value)
    {
        if (tmp != null)
        {
            tmp.text = value;
            return;
        }

        if (legacy != null)
            legacy.text = value;
    }

    private void SubscribeHealth()
    {
        if (playerHealth == null)
            return;

        playerHealth.OnHealthChanged -= HandleHealthChanged;
        playerHealth.OnHealthChanged += HandleHealthChanged;
    }

    private void UnsubscribeHealth()
    {
        if (playerHealth == null)
            return;

        playerHealth.OnHealthChanged -= HandleHealthChanged;
    }

    private void HandleHealthChanged(int current, int max)
    {
        UpdateHealthText(current, max);
    }

    private void UpdateHealthText(int current, int max)
    {
        if (_lastHealth == current && _lastMaxHealth == max)
            return;

        _lastHealth = current;
        _lastMaxHealth = max;

        // If a dedicated value field is assigned, keep the "Health" label static.
        if (healthValueText != null || healthValueTextLegacy != null)
        {
            SetText(healthValueText, healthValueTextLegacy, $"{current}/{max}");
            return;
        }

        // Fallback: single field with both label and value.
        SetText(healthText, healthTextLegacy, $"Health: {current}/{max}");
    }
}
