using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponDataSO gunDataSO;
    public Transform bulletSpawnPoint;
    public BulletProjectilePoolHelper bulletPrefab;         //BulletPrefab with BulletProjectilePoolHelper.cs

    public int currentAmmo;
    public int curentDamage;
    private float currentFireRate;

    private float _lastFired;
    private PoolSystem poolSystem;
    [SerializeField] private bool debugSetGunPosition;

    Vector2 directions;
    UiManager uiManager;
    GameManager gm;
    bool fireInput;
    private void Start()
    {
        gm = GameManager.Singleton;
        poolSystem = PoolSystem.Singleton;
        uiManager = UiManager.Singleton;
        transform.localPosition = gunDataSO.spawnPosition;
        // PoolSystemGeneric.Singleton.AddObjectToPooledObject(bulletPrefab, 50);
        PoolSystem.Singleton.AddObjectToPooledObject(bulletPrefab.gameObject, 50);
        currentAmmo = gunDataSO.maxAmmoInMag;
        curentDamage = gunDataSO.damage;

    }

    private void Update()
    {
        if (gm.useMobileControll)
        {
            fireInput = gm.shootJoystick.progress >= gm.shootThreshold;
        }
        else
            fireInput = gunDataSO.autoFire ? Input.GetKey(KeyCode.Mouse0) : Input.GetKeyDown(KeyCode.Mouse0);

        if (fireInput || gm.isFiring)
        {
            Shoot();
        }

        if (debugSetGunPosition)
        {
            Vector3 pos = transform.localPosition;
            gunDataSO.spawnPosition = pos;
        }
    }

    public void Shoot()
    {
        if (currentAmmo > 0)
            if (Time.time - _lastFired > 1 / gunDataSO.fireRate)
            {
                if (!WeaponManager.Singleton.useInfiniteAmmo)
                    currentAmmo--;

                // if (uiManager)
                //     UiManager.Singleton.UpdateAmmoCountText(currentAmmo);

                _lastFired = Time.time;
                switch (gunDataSO.bulletType)
                {
                    case BulletType.Projectile:
                        ShootProjectile();
                        break;
                    case BulletType.Raycast:
                        ShootRaycast();
                        break;
                }

                AudioPoolSystem.Singleton.PlayShootAudio(gunDataSO.shootSFX, 1f);
                if (CameraShake.Singleton)
                    CameraShake.Singleton.ShakeOnce(gunDataSO.cameraShakeDuration, gunDataSO.cameraShakeStrength);
            }
    }

    public void ShootProjectile()
    {
        var spawnEuler = bulletSpawnPoint.localEulerAngles;
        for (int i = 0; i < gunDataSO.howManyBulletPerShoot; i++)
        {
            bulletSpawnPoint.transform.localRotation = Quaternion.Euler(
                            //Don't rotate X
                            0,
                            //Don't rotate Y
                            0,
                            //Rotate random z
                            Random.Range(-gunDataSO.spreadValue, gunDataSO.spreadValue));

            var bullet = poolSystem.SpawnFromPool(bulletPrefab.gameObject, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            var bulletRb = bullet.GetComponent<Rigidbody2D>();
            bulletRb.AddForce(bulletSpawnPoint.right * gunDataSO.shootPower);
            // bulletRb.velocity = bulletSpawnPoint.right * gunDataSO.shootPower;

            #region Unused (Pool Generic)
            /*
            bullet = PoolSystemGeneric.Singleton.SpawnFromPool(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation).gameObject;
            var bulletRb = bullet.GetComponent<Rigidbody2D>();
            // bulletRb.velocity = Vector3.zero;
            bulletRb.velocity = bulletSpawnPoint.right * 1000;
            // bulletRb.AddForce(bulletSpawnPoint.right * gunDataSO.shootPower);
            print(bulletRb);
            */
            #endregion
        }

    }

    //TODO Add Shooting With Raycast
    public void ShootRaycast()
    {
        Vector3 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        for (int i = 0; i < gunDataSO.howManyBulletPerShoot; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(bulletSpawnPoint.position, bulletSpawnPoint.right + new Vector3(Random.Range(-gunDataSO.spreadValue, gunDataSO.spreadValue), Random.Range(-gunDataSO.spreadValue, gunDataSO.spreadValue)), 1000f);

            Debug.DrawLine(bulletSpawnPoint.position, hit.point, Color.red, .2f);
            print(hit.point + new Vector2(Random.Range(-gunDataSO.spreadValue, gunDataSO.spreadValue), Random.Range(-gunDataSO.spreadValue, gunDataSO.spreadValue)));
            var bullet = poolSystem.SpawnFromPool(bulletPrefab.gameObject, bulletSpawnPoint.position, Quaternion.identity) as GameObject;
            var bulletRb = bullet.GetComponent<Rigidbody2D>();
            bulletRb.velocity = Vector3.zero;
            // bulletRb.velocity = direction * 1000;
            bulletRb.AddForce(direction += bulletSpawnPoint.right * gunDataSO.shootPower, ForceMode2D.Impulse);
        }

    }


}
