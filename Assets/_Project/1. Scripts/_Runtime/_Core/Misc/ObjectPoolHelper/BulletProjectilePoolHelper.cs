using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class BulletProjectilePoolHelper : PoolHelper
{
    public ImpactPoolHelper hitVFXPrefab;
    public Rigidbody2D rb;
    private TrailRenderer trailRenderer;
    // ProjectileType projectileType;

    private void Awake()
    {

        useDeactive = true;                                                                    //supaya menonaktifkan peluru/gameobject ketika tidak mengenai apapun selama {deactiveAfter}
        rb = GetComponent<Rigidbody2D>();
        trailRenderer = GetComponent<TrailRenderer>();

        if (!trailRenderer)
            trailRenderer = GetComponentInChildren<TrailRenderer>();


    }

    private void Start()
    {

        PoolSystem.Singleton.AddObjectToPooledObject(hitVFXPrefab.gameObject, 30);              //menambahkan object hitVFXPrefab untuk bisa diinstantiate kan kembali dari PoolSystem
        // PoolSystemGeneric.Singleton.AddObjectToPooledObject(hitVFXPrefab, 30);   
    }

    public override void OnObjectSpawn()
    {
        //Reset velocity dari rigidbody (membuat peluru tidak bergerak)
        rb.velocity = Vector3.zero;

        if (trailRenderer)                                                                      //Jika variable trailRenderer tidak kosong 
            trailRenderer.Clear();                                                              //Reset Trail renderer                                                  
                                                                                                //nonaktifkan object setelah waktu tertentu

        // rb.AddForce(transform.right * WeaponManager.Singleton.currentGun.gunDataSO.shootPower);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        gameObject.SetActive(false);
        // PoolSystemGeneric.Singleton.SpawnFromPool(hitVFXPrefab, transform.position, transform.rotation);
        PoolSystem.Singleton.SpawnFromPool(hitVFXPrefab.gameObject, transform.position, transform.rotation);        //Spawn HitVFXPrefab

        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.OnDamage(WeaponManager.Singleton.weaponInventoryHolder.GetComponentInChildren<Weapon>().curentDamage);
        }

        // if(projectileType == ProjectileType.Explodeable)
        // {
        //      rb.AddForce
        // }

    }
}
public enum ProjectileType { Normal, Explodeable }
