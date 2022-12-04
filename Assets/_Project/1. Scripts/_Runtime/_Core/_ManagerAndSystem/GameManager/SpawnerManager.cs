using NaughtyAttributes;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    #region Singleton
    public static SpawnerManager Singleton;
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
    #endregion
    public GameManager gm { get; set; }
    public ItemSpawnManager itemSpawnManager { get; set; }
    public BuffSpawnManager buffSpawnManager { get; set; }
    public EnemySpawnerManager enemySpawnerManager { get; set; }

    public PipeItemSpawner[] pipeSpawnerPosition;

    [BoxGroup("Box Spawn Settings")]
    public int currentEnemyCount;
    [BoxGroup("Enemy Spawn Settings")]
    public int maxEnemyCount = 3;
    [BoxGroup("Enemy Spawn Settings")]
    public float nextEnemySpawnTime = 3f;
    float enemyWaitTimer;

    [BoxGroup("Box Spawn Settings")]
    public int currentBoxCount;
    [BoxGroup("Box Spawn Settings")]
    public int maxBoxCount = 1;
    [BoxGroup("Box Spawn Settings")]
    public float nextBoxSpawnTime = 1f;
    float boxWaitTimer;

    [BoxGroup("BuffBox Spawn Settings")]
    public int currentBuffBoxCount;
    [BoxGroup("BuffBox Spawn Settings")]
    public int maxBuffBoxCount = 1;
    [BoxGroup("BuffBox Spawn Settings")]
    public int spawnBuffBoxEveryEnemyKilled = 5;
    // float BoxBuffWaitTimer;
    private void Start()
    {
        gm = GameManager.Singleton;
        itemSpawnManager = GetComponentInChildren<ItemSpawnManager>();
        enemySpawnerManager = GetComponentInChildren<EnemySpawnerManager>();
        buffSpawnManager = GetComponentInChildren<BuffSpawnManager>();

        boxWaitTimer = nextBoxSpawnTime;                //Set Box Timer
        enemyWaitTimer = nextEnemySpawnTime;            //Set Enemy Timer 
        if (!itemSpawnManager) Debug.LogWarning("itemSpawnManager is empty,\n Item/Gun box is not going to spawn");
        if (!buffSpawnManager) Debug.LogWarning("buffSpawnManager is empty,\n Buff box is not going to spawn");
    }

    public void ObjectSpawnHandler()
    {
        //call GameLoop Methode everyframe
        if (currentEnemyCount < maxEnemyCount && enemySpawnerManager)
        {
            enemyWaitTimer -= Time.deltaTime;
            if (enemyWaitTimer <= 0)
            {
                enemyWaitTimer = nextEnemySpawnTime;
                SpawnEnemy();
            }
        }

        if (itemSpawnManager)
        {
            if (currentBoxCount < maxBoxCount && itemSpawnManager)
            {
                boxWaitTimer -= Time.deltaTime;
                if (boxWaitTimer <= 0)
                {
                    boxWaitTimer = nextBoxSpawnTime;
                    SpawnBox();
                }
            }
        }


        if (buffSpawnManager)
        {
            if (currentBuffBoxCount < maxBuffBoxCount && currentEnemyCount < 1)
            {
                SpawnBuffBox();
            }
        }
    }

    void SpawnBox() => itemSpawnManager.SpawnBoxItem();
    void SpawnEnemy() => enemySpawnerManager.SpawnEnemy();
    void SpawnBuffBox() => buffSpawnManager.SpawnBuffBoxItem();
}
