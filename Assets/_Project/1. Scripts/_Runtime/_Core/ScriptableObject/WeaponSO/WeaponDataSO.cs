using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGunDataSO", menuName = "ItemDataSO/Gun/GunDataSO")]
public class WeaponDataSO : ItemDataSO
{
    [Header("Enums")]
    //Weapon Enums
    public BulletType bulletType;
    public AmmoType ammoType;



    [Header("WeaponStats")]
    //Weapon Stats
    public float fireRate = 7f;
    public int damage = 10;
    public int maxAmmoInMag = 30;
    public int maxCarryingAmmo = 90;
    public bool autoFire;

    [Range(0, 40), Tooltip("Untuk shooting menggunakan Raycast, disarankan menggunakan nilai 0.2f")]
    public float spreadValue;
    public int howManyBulletPerShoot = 1;
    public float shootPower = 1200;

    [Header("Audio")]
    //Audio
    public AudioClip shootSFX;

    [Header("CameraShake")]
    //CameraShake Setting
    public float cameraShakeDuration;
    public float cameraShakeStrength;


    [Header("SpawnPosition")]
    //Weapon Position
    public Vector3 spawnPosition;
    // public Vector3 weaponScale;

    [Header("WeaponUpgrade")]
    public bool canUpgradeWeapon;
    [ShowIf("canUpgradeWeapon")]
    public WeaponUpgradeInfo weaponUpgradeInfo;
    
    public void AddWeaponObjectToPool()
    {
        PoolSystem.Singleton.AddObjectToPooledObject(ItemPrefab, 1);
    }

    public GameObject SpawnWeaponObject(Transform weaponholder)
    {
        return PoolSystem.Singleton.SpawnFromPool(ItemPrefab, spawnPosition, Quaternion.identity, weaponholder);
    }

    public void SetWeapon()
    {
        weaponUpgradeInfo.SetUpWeaponUpgradeInfo(this);
    }
    public void ResetAllUpgrade()
    {
        weaponUpgradeInfo.ResetAllUpgrade();
    }
}

public enum BulletType { Projectile, Raycast }
public enum AmmoType { RifleAmmo, SMGAmmo, Rocket }

[System.Serializable]
public class WeaponUpgradeInfo
{
    public int itemPrice;
    public int currentDamage { get; set; }
    public int currentClip { get; set; }
    public float currentFireRate { get; set; }

    public bool useCurrentDamageUpgradeForAllUpgrade = true;

    [Header("Damage Upgrade")]
    [SerializeField] private bool showUpgradeDamage;
    [ShowIf("showUpgradeDamage"), AllowNesting]
    public int currentDamageUpgradeLevel = -1;
    [ShowIf("showUpgradeDamage"), AllowNesting]
    public DamageUpgrade damageUpgrade;

    [Header("Clip Upgrade")]
    [SerializeField] private bool showUpgradeClip;
    [ShowIf("showUpgradeClip"), AllowNesting]
    public int currentClipUpgradeLevel = -1;
    [ShowIf("showUpgradeClip"), AllowNesting]
    public ClipUpgrade clipUpgrade;

    [Header("FireRate Upgrade")]
    [SerializeField] private bool showUpgradeFireRate;
    [ShowIf("showUpgradeFireRate"), AllowNesting]
    public int currentFireRateUpgradeLevel = -1;
    [ShowIf("showUpgradeFireRate"), AllowNesting]
    public FireRateUpgrade fireRateUpgrade;

    public void SetUpWeaponUpgradeInfo(WeaponDataSO weaponDataSO)
    {
        if (useCurrentDamageUpgradeForAllUpgrade)
        {
            if (currentDamageUpgradeLevel == -1)
            {
                currentDamage = weaponDataSO.damage;
                currentClip = weaponDataSO.maxAmmoInMag;
                currentFireRate = weaponDataSO.fireRate;
            }

            else
            {
                currentDamage = damageUpgrade.damageUpgradeLevels[currentDamageUpgradeLevel];
                currentClip = clipUpgrade.clipUpgradeLevels[currentDamageUpgradeLevel];
                currentClip = clipUpgrade.clipUpgradeLevels[currentDamageUpgradeLevel];
            }

            return;
        }

        if (currentDamageUpgradeLevel == -1)
            currentDamage = weaponDataSO.damage;
        else currentDamage = damageUpgrade.damageUpgradeLevels[currentDamageUpgradeLevel];

        if (currentClipUpgradeLevel == -1)
            currentClip = weaponDataSO.maxAmmoInMag;
        else currentClip = clipUpgrade.clipUpgradeLevels[currentClipUpgradeLevel];

        if (currentFireRateUpgradeLevel == -1)
            currentFireRate = weaponDataSO.fireRate;
        else currentFireRate = fireRateUpgrade.fireRateUpgradeLevels[currentFireRateUpgradeLevel];
    }

    public void UpgradeDamage()
    {
        currentDamage = damageUpgrade.damageUpgradeLevels[currentDamageUpgradeLevel];
    }

    public void UpgradeClip()
    {
        currentClip = clipUpgrade.clipUpgradeLevels[currentClipUpgradeLevel];
    }

    public void ResetAllUpgrade()
    {
        currentDamage = 0;
        currentClip = 0;
        currentFireRate = 0f;

        currentDamageUpgradeLevel = -1;
        currentFireRateUpgradeLevel = -1;
        currentClipUpgradeLevel = -1;
    }
}

[System.Serializable]
public struct DamageUpgrade
{
    public int[] damageUpgradePrices;
    public int[] damageUpgradeLevels;
}

[System.Serializable]
public struct ClipUpgrade
{
    public int[] clipUpgradePrices;
    public int[] clipUpgradeLevels;

}

[System.Serializable]
public struct FireRateUpgrade
{
    public int[] fireRateUpgradePrices;
    public float[] fireRateUpgradeLevels;
}
