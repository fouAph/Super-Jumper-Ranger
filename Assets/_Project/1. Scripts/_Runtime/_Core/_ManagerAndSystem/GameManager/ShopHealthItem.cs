using UnityEngine;

public class ShopHealthItem : ShopItem
{
    [SerializeField] private int healthToAdd = 2;
    protected override void Awake()
    {
        base.Awake();
        itemName_TMP.text = string.Format("Add {0} {1}", healthToAdd, itemName);
    }
    public override void OnBuyItem(PlayerManager playerManager)
    {
        base.OnBuyItem(playerManager);
        if (playerManager.Credit >= price && playerManager.playerHealth.currentHealth <= playerManager.playerHealth.maxHealth - healthToAdd)
        {
            playerManager.Credit -= price;
            playerManager.playerHealth.currentHealth += healthToAdd;
            playerManager.gm.uiManager.UpdateHealth();
            playerManager.gm.uiManager.UpdateCreditUI();
        }
    }
}
