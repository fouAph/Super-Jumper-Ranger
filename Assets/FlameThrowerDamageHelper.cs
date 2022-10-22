using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrowerDamageHelper : WeaponBase
{
    FlameThrower flameThrower;
    // Start is called before the first frame update
    void Start()
    {
        flameThrower = GetComponentInParent<FlameThrower>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        HealthSystem healthSystem = other.GetComponent<HealthSystem>();
        if (healthSystem)
        {
            var hs = healthSystem.currentHealth -= flameThrower.DamagePerSecond;
            if (hs <= 0)
            {
                // if (healthSystem.isDead) return;
                healthSystem.onDeathEvent?.Invoke();
            }
        }
    }
}
