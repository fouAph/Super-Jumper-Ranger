using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseWeaponDamageBuff", menuName = "PlayerBuff/IncreaseWeaponDamageBuff")]
public class IncreaseWeaponDamageBuff : PlayerBuffSO
{
    public int increaseByPercent;

    public override IEnumerator OnUseRoutine(PlayerManager playerManager)
    {
        playerManager.uiManager.HideBuffUI();
        if (playerManager.weaponManager.currentGun)
        {
            var increase = playerManager.weaponManager.currentGun.curentDamage * increaseByPercent / 100 + playerManager.weaponManager.currentGun.curentDamage;
            playerManager.weaponManager.currentGun.curentDamage = increase;
        }
        yield return null;
    }
}