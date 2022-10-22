using System.Collections;
using UnityEngine;

public class BuffSpawnManager : MonoBehaviour
{
    public static BuffSpawnManager Singleton;
    public RandomBuffPickup buffPickupPrefab;
    SpawnerManager spawnerManager;


    private void Awake()
    {
        if (Singleton != null)
        {
            Debug.LogWarning("More than one Instance of " + Singleton.GetType());
            Debug.LogWarning("Destroying " + Singleton.name);
            DestroyImmediate(Singleton.gameObject);
        }

        Singleton = this;
    }

    private void Start()
    {
        spawnerManager = SpawnerManager.Singleton;
        Invoke("DelayStart", .5f);
        // spawnTimer = spawnTimeWait;
    }

    private void DelayStart()
    {
        PoolSystem.Singleton.AddObjectToPooledObject(buffPickupPrefab.gameObject, 5, transform);
    }

    public void SpawnBuffBoxItem()
    {
        for (int i = Random.Range(0, spawnerManager.pipeSpawnerPosition.Length); i < spawnerManager.pipeSpawnerPosition.Length; i++)
        {
            // int randomNumberPipe = Random.Range(0,  spawnerManager.pipeSpawnerPosition.Length);
            if (spawnerManager.pipeSpawnerPosition[i].busy == false)
            {
                spawnerManager.pipeSpawnerPosition[i].SpawnItem();
                spawnerManager.currentBuffBoxCount++;
                StartCoroutine(TriggerSpawn(i));
                break;
            }
        }
    }

    IEnumerator TriggerSpawn(int pos)
    {
        yield return new WaitForSeconds(1);
        var go = PoolSystem.Singleton.SpawnFromPool(buffPickupPrefab.gameObject, spawnerManager.pipeSpawnerPosition[pos].transform.position, Quaternion.identity);
        go.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }


}