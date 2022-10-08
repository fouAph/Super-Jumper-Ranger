using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton;
    public CharacterDataSO currentCharacter;
    ItemSpawnManager itemSpawnManager;
    EnemySpawnerManager enemySpawnerManager;
    string sceneName;
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
    public GameState gameState = GameState.Menu;


    [Header("Player")]
    public int score;

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

    private UiManager uiManager;
    private void Start()
    {

        Invoke("DelayStart", .1f);
        // Invoke("GameStart", 3f);
        boxWaitTimer = nextBoxSpawnTime;

    }

    void DelayStart()
    {
        // PoolSystem.Singleton.SpawnFromPool(PlayerManager.Singleton.characterDataSO.CharacterPrefab, playerSpawnPosition.position, Quaternion.identity);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))

            Mathf.Clamp(currentBoxCount, 0, currentBoxCount);

        if (gameState == GameState.GetReady || gameState == GameState.Menu) return;

        if (PlayerManager.Singleton.playerHealth.isDead)
            gameState = GameState.GameOver;




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
    }

    void GameStart()
    {
        gameState = GameState.InGame;
        uiManager = UiManager.Singleton;
        itemSpawnManager = ItemSpawnManager.Singleton;
        enemySpawnerManager = EnemySpawnerManager.Singleton;
    }

    public void UpdateScoreCount(int scoreToAdd)
    {
        score += scoreToAdd;
        if (uiManager)
            uiManager.UpdateScoreText(score);
    }


    public void OnPlayGameButton()
    {
        SceneManager.LoadSceneAsync(1);

        Invoke("GameStart", 1);
    }



}

public enum GameState { Menu, GetReady, InGame, GameOver }
