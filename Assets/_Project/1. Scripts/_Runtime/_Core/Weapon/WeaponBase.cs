using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    public WeaponDataSO gunDataSO;
    public int currentAmmo;
    public int curentDamage;
    public int maxAmmoInClip;
    public int currentDamageLevelUpgrade = 0;

    public virtual void Shoot() { }
    public virtual void ResetAmmo()
    {
        currentAmmo = maxAmmoInClip;
    }

    // public virtual void UpgradeDamage()
    // {
    //     if (gunDataSO.weaponUpgradeInfo.currentDamageUpgradeLevel > -1)
    //         curentDamage = gunDataSO.weaponUpgradeInfo.currentDamage;
    // }

    IEnumerator UpgradeDamage_Routine(int damageIndex)
    {
        yield return new WaitForSeconds(.02f);
        curentDamage = gunDataSO.weaponUpgradeInfo.damageUpgrade.damageUpgradeLevels[damageIndex];
    }
}

