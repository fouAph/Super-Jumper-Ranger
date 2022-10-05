using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager Singleton;
    ItemSpawnManager itemSpawnManager;
    EnemySpawnerManager enemySpawnerManager;
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

    [Header("Game State")]
    public GameObject player;
    public bool gameStart;

    [Header("Score")]
    public int score;
    public TMP_Text scoreTMP;

    [Header("Enemy Spawn Settings")]
    public int maxEnemyCount;
    // [System.NonSerialized]
    public int currentEnemyCount;
    public float nextEnemySpawnTime = 0.5f;
    float enemyWaitTimer;

    [Header("Box Spawn Settings")]
    public int maxBoxCount = 1;
    // [System.NonSerialized]
    public int currentBoxCount;
    public float nextBoxSpawnTime = 1f;
    float boxWaitTimer;

    private void Start()
    {
        itemSpawnManager = ItemSpawnManager.Singleton;
        enemySpawnerManager = EnemySpawnerManager.Singleton;
        Invoke("GameStart", 1f);
        boxWaitTimer = nextBoxSpawnTime;
    }

    private void Update()
    {
        if (!gameStart) return;


        if (currentEnemyCount < maxEnemyCount && enemySpawnerManager)
        {
            enemyWaitTimer -= Time.deltaTime;
            if (enemyWaitTimer <= 0)
            {
                enemyWaitTimer = nextEnemySpawnTime;
                SpawnEnemy();
            }
        }

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

    void SpawnBox()
    {
        itemSpawnManager.SpawnBoxItem();
    }

    void SpawnEnemy()
    {
        enemySpawnerManager.SpawnEnemy();
        print($"curent enemy count is {currentEnemyCount} that is less than { maxEnemyCount} /n spawning 1 enemy");
    }

    void GameStart()
    {
        gameStart = true;
    }

    public void UpdateScore(int scoreToAdd)
    {
        print("added");
        score += scoreToAdd;
        if (scoreTMP)
            scoreTMP.text = $"Score : {score}";
    }


}
