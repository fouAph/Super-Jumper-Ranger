using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : HealthSystem
{
    [SerializeField] CreditAdder creditAdder;
    GameManager gm;
    public override void Start()
    {
        gm = GameManager.Singleton;
        creditAdder = GetComponent<CreditAdder>();
        base.Start();
        onDeathEvent.AddListener(delegate { OnDead(); });
    }


    private void OnDisable()
    {
        Setup();
    }

    void OnDead()
    {
        if (gm)
        {
            gm.spawnerManager.currentEnemyCount--;
            if (gm.spawnerManager.currentEnemyCount < 0)
            {
                gm.spawnerManager.currentEnemyCount = 0;
            }

            if (gm.playerManager)
            {
                gm.playerManager.credit += creditAdder.creditToAdd;
                gm.uiManager.UpdateCreditUI();
            }

        }
    }
}
