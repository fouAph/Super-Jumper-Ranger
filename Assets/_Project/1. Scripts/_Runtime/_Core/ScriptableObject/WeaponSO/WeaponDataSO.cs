using UnityEngine;

[CreateAssetMenu(fileName = "NewGunDataSO", menuName = "ItemDataSO/Gun/GunDataSO")]
public class WeaponDataSO : ItemDataSO
{
    [Header("Enums")]
    //Weapon Enums
    public BulletType bulletType;
    public AmmoType ammoType;

    [Header("Audio")]
    //Audio
    public AudioClip shootSFX;

    [Header("CameraShake")]
    //CameraShake Setting
    public float cameraShakeDuration;
    public float cameraShakeStrength;

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

    [Header("SpawnPosition")]
    //Weapon Position
    public Vector3 spawnPosition;
    // public Vector3 weaponScale;

    public void AddWeaponObjectToPool()
    {
        PoolSystem.Singleton.AddObjectToPooledObject(ItemPrefab, 1);
    }

    public GameObject SpawnWeaponObject(Transform weaponholder)
    {
        return PoolSystem.Singleton.SpawnFromPool(ItemPrefab, spawnPosition, Quaternion.identity,weaponholder);
    }
}

public enum BulletType { Projectile, Raycast }
public enum AmmoType { RifleAmmo, SMGAmmo, Rocket }
