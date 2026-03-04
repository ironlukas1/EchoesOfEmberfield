using UnityEngine;

public class Player : MonoBehaviour
{
    // Health
    public float _maxHealth = 100f;
    private float _currentHealth;

    //Stamina
    public float _maxStamina = 100f;
    private float _currentStamina;
    private float _staminaRegenRate = 10f; // Stamina points per second
    private float _staminaRegenDelay = 2f; // Time in seconds before stamina starts regenerating after use
    private float _staminaRegenTimer = 0f;

    //Combat
    public float weaponSwingCost = 15f;
    private bool _canSwing = true;


    void Awake()
    {
        _currentHealth = _maxHealth;
        _currentStamina = _maxStamina;
    }

    void Update()
    {
        if(_currentStamina < weaponSwingCost)
        {
            _canSwing = false;
        }
        else
        {
            _canSwing = true;
        }

        if (_currentHealth <= 0)
        {
            Debug.Log("Player is dead");
        }
    }
}