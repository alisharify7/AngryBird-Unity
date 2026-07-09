using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piggie : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 3.0f;
    [SerializeField] private float _currentHealth;
    [SerializeField] private float _damanageThreashhold;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void DamagePiggie(float damageAmount)
    {
        _currentHealth -= damageAmount;
        if (_currentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        GameManager.instance.RemovePiggie(this);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float impactVelacity = collision.relativeVelocity.magnitude;
        if (impactVelacity > _damanageThreashhold)
        {
            DamagePiggie(impactVelacity);
        }
    }

}
