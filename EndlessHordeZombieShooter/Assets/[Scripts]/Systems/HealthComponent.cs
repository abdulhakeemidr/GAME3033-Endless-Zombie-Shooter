using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float currentHealth;
    public float CurrentHealth => currentHealth;
    [SerializeField]
    private float maxHealth;
    public float MaxHealth => maxHealth;

    protected virtual void Start()
    {
        currentHealth = MaxHealth;
    }

    public virtual void Destroy()
    {
        Destroy(gameObject);
    }

    public virtual void TakeDamage(float damage)
    {
        Debug.Log("Hit");
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            Destroy();
        }
    }
}