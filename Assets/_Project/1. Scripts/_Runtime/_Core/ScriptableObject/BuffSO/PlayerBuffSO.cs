using System.Collections;
using UnityEngine;
using NaughtyAttributes;

public class PlayerBuffSO : ScriptableObject
{
    public string buffName;
    public bool isPermanentBuff;
    [HideIf("isPermanentBuff")]
    public float maxDuration;
    public virtual void OnUse(PlayerManager playerManager)
    {
        ResetStats(playerManager);
        playerManager.StartCoroutine(OnUseRoutine(playerManager));
        if (!isPermanentBuff)
            playerManager.StartCoroutine(UpdateBuffUIRoutine(playerManager));
    }

    public virtual IEnumerator OnUseRoutine(PlayerManager playerManager)
    {
        yield return null;
    }

    protected virtual IEnumerator UpdateBuffUIRoutine(PlayerManager playerManager)
    {
        playerManager.uiManager.SetupBuffUi(buffName, maxDuration);
        if (!isPermanentBuff)
            while (playerManager.buffDurationLeft >= 0)
            {
                playerManager.uiManager.UpdateBuffUIProgress(playerManager.buffDurationLeft);
                yield return null;
            }
        yield return null;
    }

    protected void ResetStats(PlayerManager playerManager)
    {
        playerManager.StopCoroutine(UpdateBuffUIRoutine(playerManager));
        // reset ImmortalBuff
        playerManager.gm.invicible = false;
        //reset DoubleJumpBuff
        playerManager.c.jump.maxJumps = 1;
        //reset Infinite ammoBuff
        playerManager.gm.useInfiniteAmmo = false;
        //reset increaseDamageBuff
        // if (playerManager.weaponManager.currentGun)
        //     playerManager.weaponManager.currentGun.curentDamage = playerManager.weaponManager.currentGun.gunDataSO.damage;
    }
}
