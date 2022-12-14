using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerContainer : MonoBehaviour
{
    public GameManager gameManager;
    public ShopManager shopManager;
    public PlayerManager playerManager;
    public UiManager uiManager;

}

namespace Fou.Manager
{
    public class NewGameManager : MonoBehaviour
    {
        #region Singleton
        public static NewGameManager Singleton;
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
        }
        #endregion
        public bool isTesting;

        #region CurrentGame
        [Header("CurrentGame")]
        public GameState gameState = GameState.Menu;
        public int currentMapBuildLevelIndex;
        public int boxCollected { get; set; }
        public int killCounter { get; set; }


        #endregion

        #region PlayerSettings
        public string playerName;
        public bool invicible;
        public bool useInfiniteAmmo;

        #endregion

        #region GameSettings
        [Header("GameSettings")]
        public CharacterDataSO selectedCharacter;
        public MapDataSO selectedMap;
        public CharacterFlipMode flipMode;
        #endregion

        #region GameObjectReferences
        public MapDataSO[] mapDataSO;
        public GameObject countdownGameobject;
        [SerializeField] CountDownHelper countDownHelper;
        public GameObject loadingScreen;
        public GameOverScorePopup gameOverScorePopup;
        public GameObject pauseMenuScreen;
        public GameObject gameOverScreen;
        #endregion


        private void Start()
        {

        }

        private void Update()
        {

        }

        public void ResumeGame()
        {

        }

        public void PauseGame()
        {

        }



        [Header("Save Settings")]
        public string saveName = "save";
        public SaveData saveData;

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

    }
}
