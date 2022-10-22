using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AddHealthBuff", menuName = "PlayerBuff/AddHealthBuff")]
public class AddHealthBuff : PlayerBuffSO
{
    public GameObject healedVFX;
    private void Start()
    {
    }

    IEnumerator AddHealedVFXToPool(PlayerManager playerManager)
    {
        PoolSystem.Singleton.AddObjectToPooledObject(healedVFX, 3);
        yield return null;
    }

    IEnumerator SpawnHealedVFX(PlayerManager playerManager)
    {
        PoolSystem.Singleton.SpawnFromPool(healedVFX, playerManager.transform.position, Quaternion.identity);
        yield return null;
    }

    public override IEnumerator OnUseRoutine(PlayerManager playerManager)
    {
        playerManager.StartCoroutine(AddHealedVFXToPool(playerManager));
         playerManager.StartCoroutine(SpawnHealedVFX(playerManager));
        playerManager.playerHealth.currentHealth++;
        if (playerManager.uiManager)
            playerManager.uiManager.UpdateHealth();
        playerManager.uiManager.HideBuffUI();
        yield return null;
    }



}