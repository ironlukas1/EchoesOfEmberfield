using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    [Tooltip("Target GameObject for the camera to follow (usually the Player).")]
    public Transform target;

    [Tooltip("Distance that the camera trails behind the target in world units.")]
    public Vector3 offset = new Vector3(0f, 0f, -10f);

    [Tooltip("How fast the camera catches up to the target.")]
    [Range(0.01f, 20f)]
    public float smoothSpeed = 8f;

    private void Reset()
    {
        // Automatically assign the player transform if no target is set yet.
        if (target == null)
        {
            var playerGo = GameObject.FindGameObjectWithTag("Player");
            if (playerGo != null)
                target = playerGo.transform;
        }

        // Default orthographic camera offset that offers proper 2D view.
        offset = new Vector3(0f, 0f, -10f);
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        var targetPosition = target.position + offset;
        var smoothPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothPosition;
    }
}
