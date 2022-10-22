using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
    public List<GameObject> enemyGameObjects = new List<GameObject>();
    SpawnerManager spawnerManager;

    private void Start()
    {
        spawnerManager = SpawnerManager.Singleton;
        Invoke("DelayStart", .5f);
        // spawnTimer = spawnTimeWait;
    }

    private void DelayStart()
    {
        foreach (var item in enemyGameObjects)
        {
            PoolSystem.Singleton.AddObjectToPooledObject(item, 5, transform);
        }
    }

    public void SpawnEnemy()
    {
        for (int i = Random.Range(0, spawnerManager.pipeSpawnerPosition.Length); i < spawnerManager.pipeSpawnerPosition.Length; i++)
        {
            // int randomNumberPipe = Random.Range(0, spawnerManager.pipeSpawnerPosition.Length);
            if (spawnerManager.pipeSpawnerPosition[i].busy == false)
            {
                spawnerManager.pipeSpawnerPosition[i].SpawnItem();

                GameManager.Singleton.spawnerManager.currentEnemyCount++;
                StartCoroutine(TriggerSpawn(i));
                break;
            }
        }
    }


    IEnumerator TriggerSpawn(int pos)
    {
        yield return new WaitForSeconds(1);
        int randomEnemyIndex = Random.Range(0, enemyGameObjects.Count);
        var go = PoolSystem.Singleton.SpawnFromPool(enemyGameObjects[randomEnemyIndex].gameObject, spawnerManager.pipeSpawnerPosition[pos].transform.position, Quaternion.identity);
        go.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }
}
