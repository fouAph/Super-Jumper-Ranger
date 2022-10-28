using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarHealthHUD : MonoBehaviour, IHealthUI
{
    public Slider healthSlider;

    public void OnInitialize(PlayerManager playerManager)
    {
        healthSlider.maxValue = playerManager.playerHealth.maxHealth;
        healthSlider.value = playerManager.playerHealth.maxHealth;
    }

    public void OnUpdateHealthUI(PlayerManager playerManager)
    {
        healthSlider.value = playerManager.playerHealth.currentHealth;
    }
}
