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

    // public void SetUpgradeDamage()
    // {
    //     StartCoroutine(UpgradeDamage_Routine());
    // }

    // IEnumerator UpgradeDamage_Routine()
    // {
    //     yield return new WaitForSeconds(.02f);
    //     if (currentDamageLevelUpgrade < 0)
    //     {
    //         print("damageLevel is not valid");
    //         yield return null;
    //     }
    //     curentDamage += gunDataSO.upgradeStats.maxDamageLevelUpgrades[currentDamageLevelUpgrade - 1];
    //     print(currentDamageLevelUpgrade);
    //     print(gunDataSO.upgradeStats.maxDamageLevelUpgrades[currentDamageLevelUpgrade - 1]);
    // }
}

