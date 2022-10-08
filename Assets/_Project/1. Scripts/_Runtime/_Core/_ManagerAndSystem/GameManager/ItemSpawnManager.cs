using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnManager : MonoBehaviour
{
    public static ItemSpawnManager Singleton;
    public WeaponPickupRandom gunPickupRandom;
    public ItemPickupRandomItem ammoPickupRandom;
    GameManager gameManager;
    [SerializeField] private PipeItemSpawner[] pipeSpawnerPosition;

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
        gameManager = GameManager.Singleton;
        Invoke("DelayStart", .5f);
        // spawnTimer = spawnTimeWait;
    }

    private void DelayStart()
    {
        PoolSystem.Singleton.AddObjectToPooledObject(gunPickupRandom.gameObject, 5, transform);
    }

    public void SpawnBoxItem()
    {
        for (int i = Random.Range(0, pipeSpawnerPosition.Length); i < pipeSpawnerPosition.Length; i++)
        {
            // int randomNumberPipe = Random.Range(0, pipeSpawnerPosition.Length);
            if (pipeSpawnerPosition[i].busy == false)
            {
                pipeSpawnerPosition[i].SpawnItem();
                gameManager.currentBoxCount++;
                StartCoroutine(TriggerSpawn(i));
                break;
            }
        }
    }

    IEnumerator TriggerSpawn(int pos)
    {
        yield return new WaitForSeconds(1);
        var go = PoolSystem.Singleton.SpawnFromPool(gunPickupRandom.gameObject, pipeSpawnerPosition[pos].transform.position, Quaternion.identity);
        go.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }

}
