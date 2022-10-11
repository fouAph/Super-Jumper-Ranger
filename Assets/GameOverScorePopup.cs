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

    public void SetGameOverPopup()
    {
        levelName_TMP.text = GameManager.Singleton.GetSceneName();
        totalKill_TMP.text = $"Total Kill: {GameManager.Singleton.killCounter.ToString()}";
        boxCollected_TMP.text = $"Box Collected: {GameManager.Singleton.playerScore.ToString()}";
        totalScore_TMP.text = $"Total Score: {Mathf.RoundToInt(GameManager.Singleton.killCounter / 3 + GameManager.Singleton.playerScore).ToString()}";


    }
}
