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


    private void OnDisable()
    {
        Setup();
    }

    void OnDead()
    {
        if (GameManager.Singleton)
        {
            GameManager.Singleton.currentEnemyCount--;
            if (GameManager.Singleton.currentEnemyCount < 0)
            {
                GameManager.Singleton.currentEnemyCount = 0;
            }

        }
    }
}
