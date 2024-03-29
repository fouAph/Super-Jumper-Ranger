using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public bool canDamagePlayer;
    public float attackCooldown;
    float attackTimer;
    public int damage;
    private void Start()
    {
        attackTimer = attackCooldown;
        canDamagePlayer = true;
    }

    private void Update()
    {
        if (attackTimer >= 0)
            attackTimer -= Time.deltaTime;

        if (attackTimer <= 0)
            canDamagePlayer = true;
    }

 

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (canDamagePlayer)
            {
                IDamageable damageable = other.collider.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.OnDamage(damage);
                }
                attackTimer = attackCooldown;
            }
        }
    }
}
