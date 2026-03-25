using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AttackHit : MonoBehaviour
{
    [Tooltip("How much damage the attack does to enemies.")]
    public int damage = 1;

    [Tooltip("How long the attack exists before being destroyed.")]
    public float lifetime = 0.15f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Try an EnemyHealth component first.
        var enemyHealth = other.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
            return;
        }

        // Fallback: try destroying an EnemyChase (if no health script is present).
        var enemy = other.GetComponent<EnemyChase>();
        if (enemy != null)
        {
            Destroy(enemy.gameObject);
        }
    }
}
