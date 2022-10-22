using System.Collections.Generic;
using UnityEngine;

public class WeaponPickupRandom : MonoBehaviour
{
    public List<WeaponDataSO> gunDataSO;
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
            other.collider.GetComponent<WeaponManager>().PickupGun(this);
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


    void SubstractCurrentBoxCount()
    {
        if (GameManager.Singleton.spawnerManager)
            GameManager.Singleton.spawnerManager.currentBoxCount--;

    }


}
