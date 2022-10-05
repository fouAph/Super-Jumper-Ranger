using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : HealthSystem
{
    public override void Start()
    {
        base.Start();
        onDeathEvent.AddListener(delegate { OnDead(); });
    }

    void OnDead()
    {
        if(GameManager.Singleton)
        {
            GameManager.Singleton.currentEnemyCount--;
        }
    }
}
