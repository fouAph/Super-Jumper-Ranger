using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Singleton;
    private void Awake()
    {
        if (Singleton != null)
        {
            Debug.LogWarning("More than one Instance of " + Singleton.GetType());
            Debug.LogWarning("Destroying " + Singleton.name);
            DestroyImmediate(Singleton.gameObject);
        }

        Singleton = this;
        AudioPoolSystem.Singleton.Initialize();
        if (!isTesting)
        {
            SceneManager.LoadSceneAsync((int)SceneIndexes.TITLE_SCREEN, LoadSceneMode.Additive);
            loadingScreen.SetActive(false);
            pauseMenuScreen.SetActive(false);
            gameOverScreen.SetActive(false);

        }

    }
    #endregion
   
    public bool isTesting;
    public CharacterFlipMode flipMode;
    public bool useMobileControll;
    public MobileController mobileController;

    public UiManager uiManager;
    public SpawnerManager spawnerManager;
    public ShopManager shopManager;

    [Header("Player")]
    public bool invicible;
    public bool useInfiniteAmmo;
    public CharacterDataSO currentCharacter;
    public PlayerManager playerManager;
    public Transform playerSpawnPoint;

    [Header("WeaponSetting")]
    public bool useBulletGravity;

    [Header("Save")]
    public string saveName = "save";
    public SaveData saveData;
    public MapDataSO[] mapDataSO;

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
    [BoxGroup("Game Settings")]
    public float startCountdownTimer;

    [Header("Game State")]
    public GameState gameState = GameState.Menu;
    public int boxCollected;
    public int killCounter;


    float startCountdown;

    List<AsyncOperation> sceneLoading = new List<AsyncOperation>();
    int maxEnemyCount = 3;
    int maxBoxCount = 1;
    bool isPause;
    bool showShop;
    private void Start()
    {
        MusicManager.Singleton.PlayMainMenuMusic();     //Play Main Music Menu

        if (isTesting)
        {
            if (useMobileControll)
            {
                mobileController.SetMobileControllScheme(this);
                mobileController.gameObject.SetActive(true);
            }
            else
                mobileController.gameObject.SetActive(false);
        }
        LoadGameData();                                 //Load Game Data, if the gameData doesnt exist, then create new data                             
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            showShop = !showShop;
            shopManager.gameObject.SetActive(showShop);

        }

        if (gameState != GameState.InGame) return;                          //return While not inGame State

        if (Input.GetKeyDown(KeyCode.Escape))                               //if escape button is pressed
        {
            isPause = !isPause;                                             //Toogle if isPause 
            if (isPause)                                                    //if isPause = true, pause the game
            {
                PauseGame();                                                //call PauseGame Method
            }
            else
            {
                ResumeGame();                                               //if not true then call Resume Game Methode
            }
        }

        Gameloop();

        spawnerManager.ObjectSpawnHandler();
    }

    #region  InGame Methods
    public void ResumeGame()
    {
        Time.timeScale = 1;                                 //Unfreeze the game
        gameState = GameState.InGame;                       //set the gameState to InGame  
        playerManager.EnablePlayerController();   //Enable Player controll
        pauseMenuScreen.SetActive(false);                   //disable Pause Menu Screen
    }

    public void PauseGame()
    {
        Time.timeScale = 0;                                 //Freeze the game
        gameState = GameState.Menu;                         //set the game state to menu
        playerManager.DisablePlayerController();  //Disable player controll
        pauseMenuScreen.SetActive(true);                    //Enable Pause Screen Menu
    }

    public void LoadGame()
    {
        sceneLoading.Clear();                                                                               //Clear sceneLoading AsynOperation List
        MusicManager.Singleton.StopMusic();                                                                 //Stop Music
        loadingScreen.SetActive(true);                                                                      //Enable Loading Scree
        sceneLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.TITLE_SCREEN));                    //Unload Title Screen Scene
        sceneLoading.Add(SceneManager.LoadSceneAsync(currentMapBuildLevelIndex, LoadSceneMode.Additive));   //Load level scene

        if (useMobileControll)
        {
            mobileController.SetMobileControllScheme(this);
            mobileController.gameObject.SetActive(true);
        }
        else
            mobileController.gameObject.SetActive(false);

        StartCoroutine(GetSceneLoadingProgress());
        StartCoroutine(SetupGameReference());
        StartCoroutine(StartGameCountdown());
    }

    public void LoadNextLevel()
    {
        sceneLoading.Clear();                                                                                   //Clear sceneLoading AsynOperation List
        MusicManager.Singleton.StopMusic();                                                                     //Stop Music
        loadingScreen.SetActive(true);                                                                          //Enable Loading Scree
        sceneLoading.Add(SceneManager.UnloadSceneAsync(currentMapBuildLevelIndex));                             //Unload Current level or map
        sceneLoading.Add(SceneManager.LoadSceneAsync(currentMapBuildLevelIndex + 1, LoadSceneMode.Additive));   //Load next level
        ResetCurrentGameProgress();                                                                             //Reset All previous score to 0
        currentMapBuildLevelIndex = currentMapBuildLevelIndex + 1;
        currentMap = mapDataSO[currentMapBuildLevelIndex - 2];
        SetMapSettings();                                                                                       //set Map settings, like how many enemy to spawn
        spawnerManager = SpawnerManager.Singleton;
        StartCoroutine(GetSceneLoadingProgress());
        StartCoroutine(StartGameCountdown());
    }

    //this is for button Event 
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
        gameState = GameState.Menu;
        StartCoroutine(GetSceneLoadingProgress());
        StartCoroutine(SetupGameReference());
        StartCoroutine(StartGameCountdown());

    }

    private void ResetCurrentGameProgress()
    {
        spawnerManager.currentBoxCount = 0;
        spawnerManager.currentEnemyCount = 0;
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
                playerManager.EnablePlayerController();
            }
            yield return null;
        }
        yield return new WaitForSeconds(.2f);
        countdownGameobject.SetActive(false);
        gameState = GameState.InGame;
    }

    void Gameloop()
    {
        // if (currentPlayer.playerHealth.isDead && gameState == GameState.InGame)
        if (gameState == GameState.InGame)
        {
            var totalscore = killCounter / 3 + boxCollected;
            if (playerManager.playerHealth.isDead || totalscore >= targetScore)
            {
                if (playerManager.playerHealth.isDead)
                {
                    gameOverScorePopup.SetTitleToGameOver();
                }

                else if (totalscore >= targetScore)
                {
                    print($"totalscore {totalscore}+ targetScore is {targetScore}");
                    gameOverScorePopup.SetTitleToGameWin();
                    mapDataSO[currentMapBuildLevelIndex - 1].unlocked = true;
                    gameOverScorePopup.levelUnlockedNotifactionText.SetActive(true);
                }

                else if (totalscore <= targetScore)
                    gameOverScorePopup.levelUnlockedNotifactionText.SetActive(false);

                if (totalscore > SaveData.Current.levels[currentMapBuildLevelIndex - 2].highScore)
                {
                    SaveHighScoreData();
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

    #endregion

    #region SaveData, LoadData, ResetData 
    //Save High Score Data 
    private void SaveHighScoreData()
    {
        SaveData.Current.levels[currentMapBuildLevelIndex - 2].highScore = killCounter / 3 + boxCollected;
        SerializationManager.Save(saveName, SaveData.Current);
    }

    private void LoadGameData()
    {
        SaveData save = SaveData.Current = (SaveData)SerializationManager.Load(Application.persistentDataPath + "/saves/" + saveName + ".save");
        if (save != null)
            saveData = save;
        else
        {
            SaveData.Current = saveData;
            SerializationManager.Save(saveName, SaveData.Current);
            print("creating new save");
        }
    }

    public void ResetData()
    {
        var resetedData = saveData;
        for (int i = 0; i < resetedData.levels.Length; i++)
        {
            resetedData.levels[i].highScore = 0;
            if (i != 0)
            {
                mapDataSO[i].unlocked = false;
            }
        }
        saveData = resetedData;
        SaveData.Current = saveData;
        SerializationManager.Save(saveName, SaveData.Current);
    }
    #endregion

    IEnumerator SetupGameReference()
    {
        yield return new WaitForSeconds(.5f);
        uiManager = UiManager.Singleton;
        spawnerManager = SpawnerManager.Singleton;
        spawnerManager.maxEnemyCount = currentMap.maxEnemyCount;
        spawnerManager.maxBoxCount = currentMap.maxBoxCount;

        if (!playerSpawnPoint)
        {
            playerSpawnPoint = GameObject.Find("PlayerSpawnPoint").transform;
        }

        yield return new WaitForSeconds(.2f);
        if (playerManager == false)
        {
            PoolSystem.Singleton.AddObjectToPooledObject(currentCharacter.CharacterPrefab, 1);
            // yield return new WaitForSeconds(.1f);
            yield return null;
        }

        var c = PoolSystem.Singleton.SpawnFromPool(currentCharacter.CharacterPrefab, playerSpawnPoint.position, Quaternion.identity);
        playerManager = c.GetComponent<PlayerManager>();
        yield return new WaitForSeconds(.1f);
        uiManager.InitHealth();


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

        // spawnerManager.maxEnemyCount = currentMap.maxEnemyCount;
        // spawnerManager.maxBoxCount = currentMap.maxBoxCount;
    }
    #endregion

    #region Change Controll
    public void ChangeControllMode_OnClickButton()
    {
        if (useMobileControll)
        {
            ChangeMobileControllMode();
        }
        else ChangeWindowsControllMode();
    }

    void ChangeMobileControllMode()
    {
        int curControll = (int)mobileController.controlScheme;
        if (curControll < 2)
        {
            mobileController.controlScheme++;
        }
        else
            mobileController.controlScheme = 0;

        mobileController.SetMobileControllScheme(this);
    }

    void ChangeWindowsControllMode()
    {
        int curControll = (int)flipMode;
        if (curControll < 1)
            flipMode++;
        else
            flipMode = 0;
        playerManager.c.EnableOrDisableRotatePlayerBody();

    }
    #endregion
}

public enum CharacterFlipMode { ByMousePosition, ByMoveDirection }
public enum SceneIndexes { MANAGER = 0, TITLE_SCREEN = 1, MAP_1 = 2, MAP_2 = 3, MAP_3 = 3, MAP_4 = 4 }
public enum GameState { Menu, InGame, GameOver }
