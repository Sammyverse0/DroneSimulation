using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 30f;
    [SerializeField] float damage = 100f;
    [SerializeField] float blastRadius = 3f;
    [SerializeField] float lifetime = 6f;
    [SerializeField] string ignoreTag;
    [SerializeField] string[] damageTags;
    [SerializeField] GameObject explosionPrefab;

    bool exploded;

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.linearVelocity = transform.forward * speed;

        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (exploded || other.isTrigger) return;

        if (!string.IsNullOrEmpty(ignoreTag) && other.transform.root.CompareTag(ignoreTag))
            return;

        exploded = true;
        Explode();
    }

    void Explode()
    {
        HashSet<Health> alreadyHit = new HashSet<Health>();
        Collider[] hits = Physics.OverlapSphere(transform.position, blastRadius);

        foreach (Collider hit in hits)
        {
            Health health = hit.GetComponentInParent<Health>();
            if (health == null || alreadyHit.Contains(health)) continue;
            if (!IsDamageable(health)) continue;

            alreadyHit.Add(health);
            health.TakeDamage(damage);
        }

        if (explosionPrefab != null)
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    bool IsDamageable(Health health)
    {
        foreach (string tag in damageTags)
        {
            if (health.CompareTag(tag)) return true;
        }

        return false;
    }
}