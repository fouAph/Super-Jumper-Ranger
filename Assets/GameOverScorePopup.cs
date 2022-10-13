using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameOverScorePopup : MonoBehaviour
{
    public TMP_Text levelName_TMP;
    public TMP_Text totalKill_TMP;
    public TMP_Text boxCollected_TMP;
    public TMP_Text totalScore_TMP;
    public TMP_Text highScore_TMP;
    public GameObject highScorePopupObject;
    public GameObject levelUnlockedNotifactionText;
    public void SetGameOverPopup()
    {
        levelName_TMP.text = GameManager.Singleton.GetSceneName();
        totalKill_TMP.text = $"Total Kill: {GameManager.Singleton.killCounter.ToString()}";
        boxCollected_TMP.text = $"Box Collected: {GameManager.Singleton.boxCollected.ToString()}";
        totalScore_TMP.text = $"Total Score: {Mathf.RoundToInt(GameManager.Singleton.killCounter / 3 + GameManager.Singleton.boxCollected).ToString()}";
        highScore_TMP.text = $"High Score: {GameManager.Singleton.saveData.levels[GameManager.Singleton.currentMapBuildLevelIndex - 2].highScore}";
    }
}
