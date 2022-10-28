using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "immortalBuff", menuName = "PlayerBuff/immortalBuff")]
public class immortalBuff : PlayerBuffSO
{
    public override IEnumerator OnUseRoutine(PlayerManager playerManager)
    {
        playerManager.buffDurationLeft = maxDuration;
        while (playerManager.buffDurationLeft >= 0)
        {
            playerManager.gm.invicible = true;
            playerManager.buffDurationLeft -= Time.deltaTime;
            if (playerManager.buffDurationLeft <= 0)
            {
                playerManager.uiManager.HideBuffUI();
                playerManager.gm.invicible = false;

            }
            yield return null;
        }
    }
}