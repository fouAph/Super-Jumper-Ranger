using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnManager : MonoBehaviour
{
    public static ItemSpawnManager Singleton;
    public WeaponPickupRandom gunPickupRandom;
    public ItemPickupRandomItem ammoPickupRandom;
    SpawnerManager spawnerManager;

    private void Start()
    {
        spawnerManager = SpawnerManager.Singleton;
        Invoke("DelayStart", .5f);
        // spawnTimer = spawnTimeWait;
    }

    private void DelayStart()
    {
        PoolSystem.Singleton.AddObjectToPooledObject(gunPickupRandom.gameObject, 5, transform);
    }

    public void SpawnBoxItem()
    {
        for (int i = Random.Range(0, spawnerManager.pipeSpawnerPosition.Length); i < spawnerManager.pipeSpawnerPosition.Length; i++)
        {
            // int randomNumberPipe = Random.Range(0,  spawnerManager.pipeSpawnerPosition.Length);
            if (spawnerManager.pipeSpawnerPosition[i].busy == false)
            {
                spawnerManager.pipeSpawnerPosition[i].SpawnItem();
                spawnerManager.currentBoxCount++;
                StartCoroutine(TriggerSpawn(i));
                break;
            }
        }
    }

    IEnumerator TriggerSpawn(int pos)
    {
        yield return new WaitForSeconds(1);
        var go = PoolSystem.Singleton.SpawnFromPool(gunPickupRandom.gameObject, spawnerManager.pipeSpawnerPosition[pos].transform.position, Quaternion.identity);
        go.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }

}
