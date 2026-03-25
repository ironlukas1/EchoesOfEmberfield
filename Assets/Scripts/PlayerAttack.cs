using UnityEngine;

[RequireComponent(typeof(Transform))]
public class PlayerAttack : MonoBehaviour
{
    [Tooltip("Prefab that represents the swing/attack hitbox. It should have a Collider2D set as a trigger and an AttackHit component.")]
    public GameObject attackPrefab;

    [Tooltip("How far in front of the player the swing spawns.")]
    public float attackDistance = 1f;

    [Tooltip("Time between swings (seconds).")]
    public float attackCooldown = 0.35f;

    private float _nextAttackTime;

    void Update()
    {
        if (attackPrefab == null)
            return;

        if (Time.time < _nextAttackTime)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            PerformAttack();
        }
    }

    private void PerformAttack()
    {
        var cam = Camera.main;
        if (cam == null)
            return;

        var mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        var direction = ((Vector2)(mouseWorld - transform.position)).normalized;
        if (direction.sqrMagnitude < 0.001f)
            direction = (Vector2)transform.right;

        var spawnPos = (Vector2)transform.position + direction * attackDistance;
        var rot = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

        var hit = Instantiate(attackPrefab, spawnPos, rot);

        // Keep the attack hitbox attached to the player so it follows movement while active.
        hit.transform.SetParent(transform, true);

        _nextAttackTime = Time.time + attackCooldown;
    }
}
