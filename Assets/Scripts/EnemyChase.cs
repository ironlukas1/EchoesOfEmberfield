using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class EnemyChase : MonoBehaviour
{
    [Tooltip("Who the enemy should chase. If left empty, it will try to find an object tagged 'Player'.")]
    public Transform target;

    [Tooltip("Movement speed in units/second.")]
    public float speed = 3f;

    [Tooltip("How close the enemy gets before stopping (0 = never stop).")]
    public float stopDistance = 0.25f;

    [Tooltip("Damage applied when this enemy collides with the player.")]
    public int damage = 10;

    [Tooltip("Radius used to keep enemies from overlapping by applying a small repulsion force.")]
    public float separationRadius = 0.32f;

    [Tooltip("How strongly enemies push away from each other within the separation radius.")]
    public float separationStrength = 1.25f;

    [Tooltip("Which layers count as " + "other enemies" + " for separation. Use an 'Enemy' layer if you want to restrict this.")]
    public LayerMask separationMask = ~0;

    private Rigidbody2D _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        if (target == null)
        {
            var playerGo = GameObject.FindGameObjectWithTag("Player");
            if (playerGo != null)
                target = playerGo.transform;
        }
    }

    void FixedUpdate()
    {
        if (target == null)
            return;

        var direction = (target.position - transform.position);
        var distance = direction.magnitude;

        if (distance <= stopDistance)
        {
            _rb.linearVelocity = Vector2.zero;
            return;
        }

        // Base pursuit movement.
        var move = ((Vector2)direction).normalized * speed;

        // Separation: push away from nearby enemies to avoid overlapping.
        Vector2 separation = Vector2.zero;
        var hits = Physics2D.OverlapCircleAll(transform.position, separationRadius, separationMask);
        foreach (var hit in hits)
        {
            // Only separate from other enemies (prevents the player/attack hitbox from pushing).
            if (hit.gameObject == gameObject)
                continue;

            if (hit.GetComponent<EnemyChase>() == null)
                continue;

            var diff = (Vector2)transform.position - (Vector2)hit.transform.position;
            var dist = diff.magnitude;
            if (dist > 0f)
            {
                separation += diff.normalized * ((separationRadius - dist) / separationRadius);
            }
        }

        move += separation * separationStrength;
        _rb.linearVelocity = move;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player"))
            return;

        // Prefer PlayerHealth because the HUD reads from PlayerHealth.
        var playerHealth = collision.collider.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            if (playerHealth.IsInvincible)
                return;

            Debug.Log($"{name}: collided with PlayerHealth, applying {damage} damage.");
            playerHealth.TakeDamage(damage);
            return;
        }

        // Backwards-compatibility for the older Player script.
        var player = collision.collider.GetComponent<Player>();
        if (player != null)
        {
            Debug.Log($"{name}: collided with Player, applying {damage} damage.");
            player.TakeDamage(damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, separationRadius);
    }
}
