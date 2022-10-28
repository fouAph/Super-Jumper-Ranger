using System.Collections;
using UnityEngine;
using TMPro;

public class SimpleTextHealthUI : MonoBehaviour, IHealthUI
{
    public TMP_Text health_TMP;

    public void OnInitialize(PlayerManager playerManager)
    {
        health_TMP.text = $"Health : {playerManager.playerHealth.maxHealth.ToString()}";
    }

    public void OnUpdateHealthUI(PlayerManager playerManager)
    {
        health_TMP.text = $"Health : {playerManager.playerHealth.currentHealth.ToString()}";
    }
}