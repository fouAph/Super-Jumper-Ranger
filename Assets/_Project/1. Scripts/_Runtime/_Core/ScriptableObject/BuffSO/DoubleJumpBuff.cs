using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "DoubleJumpBuff", menuName = "PlayerBuff/DoubleJumpBuff")]

public class DoubleJumpBuff : PlayerBuffSO
{
    public int maxJump = 2;
    int defaultMaxJump = 1;

    public override IEnumerator OnUseRoutine(PlayerManager playerManager)
    {
        playerManager.buffDurationLeft = maxDuration;
        while (playerManager.buffDurationLeft >= 0)
        {
            playerManager.c.jump.maxJumps = maxJump;
            playerManager.buffDurationLeft -= Time.deltaTime;
            if (playerManager.buffDurationLeft <= 0)
            {
                playerManager.uiManager.HideBuffUI();
                playerManager.c.jump.maxJumps = defaultMaxJump;

            }
            yield return null;
        }
    }
}
