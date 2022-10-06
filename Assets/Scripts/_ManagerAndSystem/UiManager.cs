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

    public void UpdateScoreText(int score)
    {
        scoreTMP.text = $"Score : {score}";
    }

    public void UpdateAmmoCountText(int ammo)
    {
        ammoCountTMP.text = $"Ammo : {ammo}";
    }
}
