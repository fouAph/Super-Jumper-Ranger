using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : HealthSystem
{
    public override void Start()
    {
        base.Start();
        onDeathEvent.AddListener(delegate { OnDead(); });
        spriteRenderer.color = originialColor;
        isDead = false;
    }

    private void OnDisable()
    {
        onDeathEvent.RemoveAllListeners();
    }

    void OnDead()
    {
        if (GameManager.Singleton)
        {
            GameManager.Singleton.currentEnemyCount--;
        }
    }
}
