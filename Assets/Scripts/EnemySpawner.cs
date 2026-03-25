using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class EnemySpawner : MonoBehaviour
{
    [Tooltip("Prefab for the enemy to spawn.")]
    public GameObject enemyPrefab;

    [Tooltip("Optional target for spawned enemies. If null, enemies will try to find a GameObject tagged 'Player'.")]
    public Transform target;

    [Tooltip("How often to spawn enemies (seconds).")]
    public float spawnInterval = 3f;

    [Tooltip("Additional distance beyond the camera view to spawn enemies.")]
    public float spawnDistanceFromView = 2f;

    [Tooltip("How far from the target to spawn enemies when camera is not available.")]
    public float fallbackSpawnRadius = 10f;

    [Tooltip("Optional maximum number of spawned enemies in the scene. 0 = unlimited.")]
    public int maxLiveEnemies = 0;

    private Camera _camera;
    private float _nextSpawnTime;

    private void Awake()
    {
        _camera = Camera.main;
        if (_camera == null)
        {
            _camera = GetComponent<Camera>();
        }

        if (target == null)
        {
            var playerGo = GameObject.FindGameObjectWithTag("Player");
            if (playerGo != null)
                target = playerGo.transform;
        }

        _nextSpawnTime = Time.time + spawnInterval;
    }

    private void Update()
    {
        if (enemyPrefab == null || target == null)
            return;

        if (maxLiveEnemies > 0 && GameObject.FindObjectsByType<EnemyChase>(FindObjectsSortMode.None).Length >= maxLiveEnemies)
            return;

        if (Time.time < _nextSpawnTime)
            return;

        SpawnEnemy();
        _nextSpawnTime = Time.time + spawnInterval;
    }

    private void SpawnEnemy()
    {
        var spawnPosition = GetSpawnPositionOutsideView();
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    private Vector2 GetSpawnPositionOutsideView()
    {
        var center = (Vector2)target.position;
        float radius;

        if (_camera != null && _camera.orthographic)
        {
            // Determine the camera view extents in world units.
            var halfHeight = _camera.orthographicSize;
            var halfWidth = halfHeight * _camera.aspect;

            // Use the larger extent so we always spawn outside the current view.
            radius = Mathf.Max(halfWidth, halfHeight) + spawnDistanceFromView;
        }
        else
        {
            radius = fallbackSpawnRadius;
        }

        var angle = Random.Range(0, Mathf.PI * 2f);
        var direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        return center + direction * radius;
    }
}
