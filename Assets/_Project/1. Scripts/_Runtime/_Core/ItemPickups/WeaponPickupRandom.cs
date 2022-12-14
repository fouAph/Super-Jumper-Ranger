using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickupRandom : MonoBehaviour, IPooledObject
{
    public List<WeaponDataSO> weaponDataSoToSpawn;
    public GameObject pikcupVFX;
    [SerializeField] AudioClip pickupSfx;
    [SerializeField] ScoreAdder scoreAdder;
    [SerializeField] HealthSystem healthSystem;

    private void Start()
    {
        scoreAdder = GetComponent<ScoreAdder>();
        Invoke("DelayStart", .1f);
        healthSystem = GetComponent<HealthSystem>();
    }
    void DelayStart()
    {
        PoolSystem.Singleton.AddObjectToPooledObject(pikcupVFX, 5);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            // SpawnWeapon(other);
            NewSpawnWeapon(other);
            PoolSystem.Singleton.SpawnFromPool(pikcupVFX, transform.position, Quaternion.identity);
            if (!GameManager.Singleton.isTesting)
                AudioPoolSystem.Singleton.PlayAudioSFX(pickupSfx, .5f);

            if (GameManager.Singleton && scoreAdder)
            {
                GameManager.Singleton.UpdateScoreCount(scoreAdder.scoreToAdd);
                SubstractCurrentBoxCount();
            }
            // other.collider.GetComponent<Character>().Flip();
        }

        else if (other.gameObject.layer == LayerMask.NameToLayer("DeadZone"))
        {
            if (healthSystem)
                healthSystem.Die();
            SubstractCurrentBoxCount();
        }
    }

    public void NewSpawnWeapon(Collision2D col)
    {
        var wpn = col.collider.GetComponent<WeaponManager>().PickupGun(this);
        wpn.gunDataSO.SetWeapon();
        wpn.ResetAmmo();
    }



    void SubstractCurrentBoxCount()
    {
        if (GameManager.Singleton.spawnerManager)
            GameManager.Singleton.spawnerManager.currentBoxCount--;

    }

    public void OnObjectSpawn()
    {
        if (weaponDataSoToSpawn != null)
        {
            weaponDataSoToSpawn.Clear();
            foreach (var item in GameManager.Singleton.playerManager.gm.unlockedWeapons)
            {
                weaponDataSoToSpawn.Add(item);
            }
            List<WeaponDataSO> shuffleWeaponDataSoList = ShuffleList.ShuffleListItems<WeaponDataSO>(weaponDataSoToSpawn);
            weaponDataSoToSpawn = shuffleWeaponDataSoList;

        }
    }
}
