using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] float maxHealth = 100f;
    [SerializeField] GameObject deathEffect;
    [SerializeField] UnityEvent onDeath;

    float current;

    void Awake()
    {
        current = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (current <= 0f) return;

        current -= amount;
        if (current <= 0f) Die();
    }

    void Die()
    {
        if (deathEffect != null)
            Instantiate(deathEffect, transform.position, Quaternion.identity);

        onDeath.Invoke();
        Destroy(gameObject);
    }
}