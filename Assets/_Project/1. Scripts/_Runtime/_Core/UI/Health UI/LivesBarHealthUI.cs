using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesBarHealthUI : MonoBehaviour, IHealthUI
{
    public Transform healthSpriteHolder;
    public GameObject healthPrefab;
    private PoolSystem poolSystem;
   
    public void OnInitialize(PlayerManager playerManager)
    {
        poolSystem = PoolSystem.Singleton;
        if (!poolSystem) return;
        else
        {
            PoolSystem.Singleton.AddObjectToPooledObject(healthPrefab, 10);
            for (int i = 0; i < playerManager.playerHealth.maxHealth; i++)
            {
                var go = PoolSystem.Singleton.SpawnFromPool(healthPrefab, Vector3.zero, Quaternion.identity, healthSpriteHolder);
                go.transform.localScale = Vector3.one;
            }
        }
    }

    public void OnUpdateHealthUI(PlayerManager playerManager)
    {
        int result = (playerManager.playerHealth.maxHealth - playerManager.playerHealth.currentHealth);
        // print(result);
        // foreach (GameObject obj in healthSpriteHolder)
        // {
        //     obj.SetActive(false);
        // }
        // for (int i = 0; i < gm.playerManager.playerHealth.currentHealth; i++)
        // {
        //     var go = PoolSystem.Singleton.SpawnFromPool(healthPrefab, Vector3.zero, Quaternion.identity, healthSpriteHolder);
        //     go.transform.localScale = Vector3.one;
        // }

        for (int i = 0; i < result; i++)
        {
            healthSpriteHolder.GetChild(i).gameObject.SetActive(false);
        }
    }




}
