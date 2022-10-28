using System.Collections.Generic;
using UnityEngine;

public class RandomBuffPickup : MonoBehaviour
{
    public List<PlayerBuffSO> weaponBuffList = new List<PlayerBuffSO>();
    public List<PlayerBuffSO> playerBuffList = new List<PlayerBuffSO>();
    [SerializeField] GameObject pikcupVFX;
    [SerializeField] AudioClip pickupSfx;
    [SerializeField] ScoreAdder scoreAdder;
    [SerializeField] HealthSystem healthSystem;
    GameManager gm;
    int random;
    private void Start()
    {
        gm = GameManager.Singleton;
        scoreAdder = GetComponent<ScoreAdder>();
        Invoke("DelayStart", .1f);
        healthSystem = GetComponent<HealthSystem>();
        if (!healthSystem)
            healthSystem = gameObject.AddComponent<ItemHealth>();
    }

    void DelayStart()
    {
        PoolSystem.Singleton.AddObjectToPooledObject(pikcupVFX, 5);
    }

    public void BuffSelector()
    {
        string buffName = "";
        int select = Random.Range(0, 10);
        if (select > 5 && gm.playerManager.weaponManager.currentGun)
        {
            print("random weapon");
            random = Random.Range(0, weaponBuffList.Count);
            var buff = weaponBuffList[random];
            buffName = buff.buffName;
            buff.OnUse(gm.playerManager);
            return;
        }
        else select = 0;

        if (select == 0)
        {
            random = Random.Range(0, playerBuffList.Count);
            var buff = playerBuffList[random];
            buffName = buff.buffName;
            buff.OnUse(gm.playerManager);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            BuffSelector();

            gameObject.SetActive(false); PoolSystem.Singleton.SpawnFromPool(pikcupVFX, transform.position, Quaternion.identity);
            if (AudioPoolSystem.Singleton)
                AudioPoolSystem.Singleton.PlayAudioSFX(pickupSfx, .5f);

            if (gm && scoreAdder)
            {
                gm.UpdateScoreCount(scoreAdder.scoreToAdd);
                SubstractCurrentBoxCount();
            }
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("DeadZone"))
        {
            if (healthSystem)
                healthSystem.Die();
            SubstractCurrentBoxCount();
        }
    }

    void SubstractCurrentBoxCount()
    {
        if (gm.spawnerManager)
            gm.spawnerManager.currentBuffBoxCount--;

    }
}
