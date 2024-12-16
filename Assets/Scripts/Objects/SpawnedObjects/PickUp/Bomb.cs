using UnityEngine;

public class Bomb : PausableMonoBehaviour
{
    public float delay = 3f;
    public float explosionRadius = 5f;
    public GameObject explosionEffect;

    private bool hasExploded = false;
    private float countdown;
    private Renderer bombRenderer;
    private Vector3 originalScale;

    void Start()
    {
        countdown = delay;
        bombRenderer = GetComponentInChildren<Renderer>();
        originalScale = transform.localScale;

    }

    protected override void OnPausableUpdate()
    {
        if (hasExploded) return;

        countdown -= Time.deltaTime;

        if (countdown <= 0f)
        {
            Explode();
        }

        UpdateVisualEffects();
    }

    void UpdateVisualEffects()
    {
        if (bombRenderer != null)
        {
            float flashIntensity = Mathf.PingPong(Time.time * (1f / Mathf.Max(countdown, 0.1f)) * 5f, 1f);
            bombRenderer.material.color = Color.Lerp(Color.white, Color.red, flashIntensity);
        }

        float scaleMultiplier = Mathf.Lerp(1f, 1.5f, 1 - (countdown / delay));
        transform.localScale = originalScale * scaleMultiplier;
    }

    void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.TryGetComponent(out IDestructible destructible))
            {
                destructible.DestroyObject();
            }
        }

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
