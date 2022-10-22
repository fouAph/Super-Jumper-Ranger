using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    #region Singleton
    public static UiManager Singleton;
    private void Awake()
    {
        if (Singleton != null)
        {
            DestroyImmediate(Singleton);
        }

        Singleton = this;
    }
    #endregion

    public TMP_Text scoreTMP;
    public TMP_Text ammoCountTMP;
    public TMP_Text targetScore;
    public TMP_Text buffNameTMP;
    public Slider buffTimerProgress;
    public Transform healthSpriteHolder;
    public GameObject healthPrefab;
    private PoolSystem poolSystem;

    GameManager gm;
    private void Start()
    {
        gm = GameManager.Singleton;
        poolSystem = PoolSystem.Singleton;
        if (!gm.isTesting)
        {
            // Invoke("InitHealth", .5f);
            targetScore.text = $"Target Score: {GameManager.Singleton.targetScore.ToString()}";
            UpdateScoreText(0);
        }
    }

    public void UpdateScoreText(int score)
    {
        if (scoreTMP)
            scoreTMP.text = $"Score : {score}";
    }

    public void UpdateAmmoCountText(int ammo)
    {
        if (ammoCountTMP)
            ammoCountTMP.text = $"Ammo : {ammo}";
    }

    #region Buff UI Region

    public void SetupBuffUi(string buffName, float maxDuration)
    {
        buffNameTMP.gameObject.SetActive(true);
        buffTimerProgress.gameObject.SetActive(true);

        buffNameTMP.text = buffName;
        buffTimerProgress.maxValue = maxDuration;
        buffTimerProgress.value = maxDuration;
    }

    public void UpdateBuffUIProgress(float timeLeft)
    {
        buffTimerProgress.value = timeLeft;
    }

    public void HideBuffUI()
    {
        buffNameTMP.gameObject.SetActive(false);
        buffTimerProgress.gameObject.SetActive(false);

    }
    #endregion

    #region Health Region
    public void InitHealth()
    {
        if (!poolSystem) return;
        else
        {
            PoolSystem.Singleton.AddObjectToPooledObject(healthPrefab, 7);
            for (int i = healthSpriteHolder.childCount; i < gm.playerManager.playerHealth.maxHealth; i++)
            {
                var go = PoolSystem.Singleton.SpawnFromPool(healthPrefab, Vector3.zero, Quaternion.identity, healthSpriteHolder);
                go.transform.localScale = Vector3.one;
            }
        }
    }

    public void UpdateHealth()
    {
        if (gm.playerManager)
        {
            int result = gm.playerManager.playerHealth.maxHealth - gm.playerManager.playerHealth.currentHealth;
            for (int i = 0; i < result; i++)
            {
                healthSpriteHolder.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
    #endregion
}
