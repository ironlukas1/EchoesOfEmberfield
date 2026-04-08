using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    private float _lifeTime = 0.8f;
    private float _timer;
    private Vector3 _moveSpeed = new Vector3(0f, 1.25f, 0f);

    public void Initialize(int damageAmount)
    {
        var textMesh = GetComponent<TextMesh>();
        if (textMesh != null)
        {
            textMesh.text = damageAmount.ToString();
            textMesh.color = Color.red;
            textMesh.fontSize = 32;
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.alignment = TextAlignment.Center;
        }

        _timer = _lifeTime;
    }

    private void Update()
    {
        transform.position += _moveSpeed * Time.deltaTime;
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * 2f);

        if ((_timer -= Time.deltaTime) <= 0f)
        {
            Destroy(gameObject);
        }
    }

    public static void Spawn(int damageAmount, Vector3 worldPosition)
    {
        var go = new GameObject("DamagePopup", typeof(TextMesh), typeof(DamagePopup));
        go.transform.position = worldPosition;
        go.transform.localScale = Vector3.one * 0.02f; // small world-space text

        var popup = go.GetComponent<DamagePopup>();
        popup.Initialize(damageAmount);
    }
}
