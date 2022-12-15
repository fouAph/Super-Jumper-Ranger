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
    public TMP_Text creditTMP;
    public TMP_Text ammoCountTMP;
    public TMP_Text targetScore;
    public TMP_Text buffNameTMP;
    public Slider buffTimerProgress;

    IHealthUI iHealthUI;
    GameManager gm;
    private void Start()
    {
        gm = GameManager.Singleton;
        iHealthUI = GetComponent<IHealthUI>();

        if (!gm.isTesting)
        {
            targetScore.text = $"Target Score: {GameManager.Singleton.targetScore.ToString()}";
            UpdateScoreText(0);
        }
        Invoke("UpdateCreditUI", 1f);
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
    public void UpdateCreditUI() => creditTMP.text = $"Credit : {gm.playerManager.Credit.ToString()}";

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
        if (iHealthUI != null)
            iHealthUI.OnInitialize(gm.playerManager);
    }

    public void UpdateHealth()
    {
        if (gm.playerManager)
        {
            if (iHealthUI != null)
                iHealthUI.OnUpdateHealthUI(gm.playerManager);
        }
    }
    #endregion

}
