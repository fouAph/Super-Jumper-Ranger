using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "InfiniteAmmoBuff", menuName = "PlayerBuff/InfiniteAmmoBuff")]
public class InfiniteAmmoBuff : PlayerBuffSO
{
    public override IEnumerator OnUseRoutine(PlayerManager playerManager)
    {
        playerManager.buffDurationLeft = maxDuration;
        while (playerManager.buffDurationLeft >= 0)
        {
            playerManager.gm.useInfiniteAmmo = true;
            playerManager.buffDurationLeft -= Time.deltaTime;
            if (playerManager.buffDurationLeft <= 0)
            {
                playerManager.uiManager.HideBuffUI();
                playerManager.gm.useInfiniteAmmo = false;
            }
            yield return null;
        }
        yield return null;
    }
}