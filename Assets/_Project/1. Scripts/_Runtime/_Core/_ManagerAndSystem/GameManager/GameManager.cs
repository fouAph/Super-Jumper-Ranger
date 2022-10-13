using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton;
    [Header("Save")]
    public string saveName = "save";
    public SaveData saveData;
    public MapDataSO[] mapDataSO;
    private void Awake()
    {
        if (Singleton != null)
        {
            Debug.LogWarning("More than one Instance of " + Singleton.GetType());
            Debug.LogWarning("Destroying " + Singleton.name);
            DestroyImmediate(Singleton.gameObject);
        }

        Singleton = this;

        SceneManager.LoadSceneAsync((int)SceneIndexes.TITLE_SCREEN, LoadSceneMode.Additive);
        loadingScreen.SetActive(false);
        pauseMenuScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        AudioPoolSystem.Singleton.Initialize();
    }

    [Header("References")]
    public GameObject countdownGameobject;
    [SerializeField] CountDownHelper countDownHelper;
    public GameObject loadingScreen;
    public GameOverScorePopup gameOverScorePopup;
    public GameObject pauseMenuScreen;
    public GameObject gameOverScreen;
    public int currentMapBuildLevelIndex;

    [BoxGroup("Game Settings")]
    public MapDataSO currentMap;
    [BoxGroup("Game Settings")]
    public int targetScore;


    [Header("Game State")]
    public bool invicible;
    public GameState gameState = GameState.Menu;
    public CharacterDataSO currentCharacter;
    public int boxCollected;
    public int killCounter;



    [BoxGroup("Game Settings")]
    public float startCountdownTimer;
    float startCountdown;

    [BoxGroup("Enemy Spawn Settings")]
    public int maxEnemyCount = 3;

    public int currentEnemyCount;
    [BoxGroup("Enemy Spawn Settings")]
    public float nextEnemySpawnTime = 0.5f;
    float enemyWaitTimer;

    [BoxGroup("Box Spawn Settings")]
    public int maxBoxCount = 1;

    public int currentBoxCount;
    [BoxGroup("Box Spawn Settings")]
    public float nextBoxSpawnTime = 1f;
    float boxWaitTimer;

    private UiManager uiManager;
    ItemSpawnManager itemSpawnManager;
    EnemySpawnerManager enemySpawnerManager;

    List<AsyncOperation> sceneLoading = new List<AsyncOperation>();
    bool isPause;

    private void Start()
    {
        boxWaitTimer = nextBoxSpawnTime;
        enemyWaitTimer = nextEnemySpawnTime;
        MusicManager.Singleton.PlayMainMenuMusic();

        //load Game
        var save = SaveData.Current = (SaveData)SerializationManager.Load(Application.persistentDataPath + "/saves/" + saveName + ".save");
        saveData = save;
    }

    private void Update()
    {


        if (gameState != GameState.InGame) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {

            isPause = !isPause;
            if (isPause)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
        Gameloop();
        if (currentEnemyCount < maxEnemyCount && enemySpawnerManager)
        {
            enemyWaitTimer -= Time.deltaTime;
            if (enemyWaitTimer <= 0)
            {
                enemyWaitTimer = nextEnemySpawnTime;
                SpawnEnemy();
            }
        }
        Mathf.Clamp(currentBoxCount, 0, maxBoxCount);

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

    public void ResumeGame()
    {
        Time.timeScale = 1;
        gameState = GameState.InGame;
        PlayerManager.Singleton.EnablePlayerController();
        pauseMenuScreen.SetActive(false);

    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        gameState = GameState.Menu;
        PlayerManager.Singleton.DisablePlayerController();
        pauseMenuScreen.SetActive(true);
    }

    public void LoadGame()
    {
        sceneLoading.Clear();
        MusicManager.Singleton.StopMusic();
        loadingScreen.SetActive(true);
        sceneLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.TITLE_SCREEN));
        sceneLoading.Add(SceneManager.LoadSceneAsync(currentMapBuildLevelIndex, LoadSceneMode.Additive));

        StartCoroutine(GetSceneLoadingProgress());
        StartCoroutine(StartGameCountdown());
    }

    public void LoadNextLevel()
    {
        sceneLoading.Clear();
        MusicManager.Singleton.StopMusic();
        loadingScreen.SetActive(true);
        sceneLoading.Add(SceneManager.UnloadSceneAsync(currentMapBuildLevelIndex));
        sceneLoading.Add(SceneManager.LoadSceneAsync(currentMapBuildLevelIndex + 1, LoadSceneMode.Additive));
        ResetCurrentGameProgress();
        currentMapBuildLevelIndex = currentMapBuildLevelIndex + 1;
        currentMap = mapDataSO[currentMapBuildLevelIndex -2];
        SetMapSettings();
        StartCoroutine(GetSceneLoadingProgress());
        StartCoroutine(StartGameCountdown());
    }


    public void NextLevel_OnClikButton()
    {
        LoadNextLevel();
    }

    public void RestartGame()
    {
        sceneLoading.Clear();
        loadingScreen.SetActive(true);
        ResetCurrentGameProgress();
        ResumeGame();
        countDownHelper.SetSprite(countDownHelper._3);
        MusicManager.Singleton.StopMusic();
        sceneLoading.Add(SceneManager.UnloadSceneAsync(currentMapBuildLevelIndex));
        sceneLoading.Add(SceneManager.LoadSceneAsync(currentMapBuildLevelIndex, LoadSceneMode.Additive));
        StartCoroutine(GetSceneLoadingProgress());
        StartCoroutine(StartGameCountdown());

    }

    private void ResetCurrentGameProgress()
    {
        currentBoxCount = 0;
        currentEnemyCount = 0;
        killCounter = 0;
        boxCollected = 0;
    }

    IEnumerator GetSceneLoadingProgress()
    {
        for (int i = 0; i < sceneLoading.Count; i++)
        {
            while (sceneLoading[i].isDone)
            {
                yield return null;
            }
        }

        yield return new WaitForSeconds(.01f);
        gameOverScreen.SetActive(false);
        yield return new WaitForSeconds(1f);
        loadingScreen.SetActive(false);
    }

    IEnumerator StartGameCountdown()
    {
        countDownHelper.SetSprite(countDownHelper._3);
        yield return new WaitForSeconds(1);
        countdownGameobject.SetActive(true);
        startCountdown = startCountdownTimer;
        AudioPoolSystem.Singleton.PlayShootAudio(countDownHelper.countdownClip);
        MusicManager.Singleton.PlayInGameMusic();
        while (startCountdown >= 0)
        {
            startCountdown -= Time.deltaTime;
            if (startCountdown <= 2 && startCountdown >= 1)
                countDownHelper.SetSprite(countDownHelper._2);
            else if (startCountdown <= 1 && startCountdown >= 0)
                countDownHelper.SetSprite(countDownHelper._1);
            else if (startCountdown <= 0)
            {
                countDownHelper.SetSprite(countDownHelper._go);
                PlayerManager.Singleton.EnablePlayerController();
            }


            yield return null;
        }
        yield return new WaitForSeconds(.2f);
        countdownGameobject.SetActive(false);
        SetupReference();
        gameState = GameState.InGame;
    }

    #region  InGame Methods
    void Gameloop()
    {
        // if (PlayerManager.Singleton.playerHealth.isDead && gameState == GameState.InGame)
        if (gameState == GameState.InGame)
        {
            var totalscore = killCounter / 3 + boxCollected;
            if (PlayerManager.Singleton.playerHealth.isDead || totalscore >= targetScore)
            {
                if (PlayerManager.Singleton.playerHealth.isDead)
                {
                    gameOverScorePopup.SetTitleToGameOver();
                }

                else if (totalscore >= targetScore)
                {
                    print($"totalscore {totalscore}+ targetScore is { targetScore}");
                    gameOverScorePopup.SetTitleToGameWin();
                    mapDataSO[currentMapBuildLevelIndex - 1].unlocked = true;
                    gameOverScorePopup.levelUnlockedNotifactionText.SetActive(true);
                }

                else
                    gameOverScorePopup.levelUnlockedNotifactionText.SetActive(false);

                if (totalscore > SaveData.Current.levels[currentMapBuildLevelIndex - 2].highScore)
                {
                    SaveHighScore();
                    print(totalscore);
                    gameOverScorePopup.highScorePopupObject.SetActive(true);
                }
                else
                    gameOverScorePopup.highScorePopupObject.SetActive(false);

                gameOverScreen.SetActive(true);
                gameOverScorePopup.SetGameOverPopup();
                gameState = GameState.GameOver;
                MusicManager.Singleton.PlayGameOverMusic();
            }




        }



    }

    private void SaveHighScore()
    {
        SaveData.Current.levels[currentMapBuildLevelIndex - 2].highScore = killCounter / 3 + boxCollected;
        SerializationManager.Save(saveName, SaveData.Current);
    }

    void SpawnBox() => itemSpawnManager.SpawnBoxItem();

    void SpawnEnemy() => enemySpawnerManager.SpawnEnemy();
    #endregion

    void SetupReference()
    {
        uiManager = UiManager.Singleton;
        itemSpawnManager = ItemSpawnManager.Singleton;
        enemySpawnerManager = EnemySpawnerManager.Singleton;
    }

    public void UpdateScoreCount(int scoreToAdd)
    {
        boxCollected += scoreToAdd;
        if (uiManager)
            uiManager.UpdateScoreText(killCounter / 3 + boxCollected);
    }

    #region Get Scene Name
    public static string GetSceneNameFromBuildIndex(int index)
    {
        string scenePath = SceneUtility.GetScenePathByBuildIndex(index);
        string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

        return sceneName;
    }
    public string GetSceneName()
    {
        return GetSceneNameFromBuildIndex(currentMapBuildLevelIndex);
    }
    #endregion

    public void OnPlayGameButton()
    {
        MusicManager.Singleton.StopMusic();
        LoadGame();

    }

    public void MainMenu_OnClikButton()
    {
        MusicManager.Singleton.StopMusic();
        sceneLoading.Clear();
        loadingScreen.SetActive(true);
        sceneLoading.Add(SceneManager.LoadSceneAsync(0));

        StartCoroutine(GetSceneLoadingProgress());
    }

    public void Exit_OnClickButton()
    {
        Application.Quit();
    }

    #region Map Methods
    public void SetMapSettings()
    {
        targetScore = currentMap.scoreTarget;
        maxEnemyCount = currentMap.maxEnemyCount;
        maxBoxCount = currentMap.maxBoxCount;
        currentMapBuildLevelIndex = currentMap.indexInBuildIndex;

    }
    #endregion
}

public enum SceneIndexes { MANAGER = 0, TITLE_SCREEN = 1, MAP_1 = 2, MAP_2 = 3, MAP_3 = 3, MAP_4 = 4 }
public enum GameState { Menu, InGame, GameOver }
