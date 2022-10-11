using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
    public static EnemySpawnerManager Singleton;
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

    public List<GameObject> enemyGameObjects = new List<GameObject>();
    [SerializeField] private PipeItemSpawner[] pipeSpawnerPosition;

    private void Start()
    {

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
        for (int i = Random.Range(0, pipeSpawnerPosition.Length); i < pipeSpawnerPosition.Length; i++)
        {
            // int randomNumberPipe = Random.Range(0, pipeSpawnerPosition.Length);
            if (pipeSpawnerPosition[i].busy == false)
            {
                pipeSpawnerPosition[i].SpawnItem();

                GameManager.Singleton.currentEnemyCount++;
                StartCoroutine(TriggerSpawn(i));
                break;
            }
        }
    }


    IEnumerator TriggerSpawn(int pos)
    {
        yield return new WaitForSeconds(1);
        int randomEnemyIndex = Random.Range(0, enemyGameObjects.Count);
        var go = PoolSystem.Singleton.SpawnFromPool(enemyGameObjects[randomEnemyIndex].gameObject, pipeSpawnerPosition[pos].transform.position, Quaternion.identity);
        go.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }
}
