using TMPro;
using UnityEngine;
public class WeaponShopItem : ShopItem
{
    [SerializeField] WeaponDataSO weaponDataSO;
    [SerializeField] TMP_Text upgradeLevel_TMP;
    [SerializeField] TMP_Text clipStat_TMP;
    [SerializeField] TMP_Text damageStat_TMP;
    private GameManager gm;
    WeaponUpgradeInfo weaponUpgradeInfo;
    bool fullyUpgraded = false;

    void Start()
    {
        weaponUpgradeInfo = weaponDataSO.weaponUpgradeInfo;
        price = weaponUpgradeInfo.itemPrice;
        gm = GameManager.Singleton;
        itemName = weaponDataSO.itemName;
        itemName_TMP.text = weaponDataSO.itemName;
        if (itemImage) itemImage.sprite = weaponDataSO.itemSprite;
        SetupShop();
    }

    public override void SetupShop()
    {
        upgradeLevel_TMP.text = $"Upgrade Level {weaponUpgradeInfo.currentDamageUpgradeLevel + 1}/{weaponUpgradeInfo.damageUpgrade.damageUpgradeLevels.Length}";
        bought = false;
        weaponDataSO.SetWeapon();

        for (int i = 0; i < gm.unlockedWeapons.Count; i++)
        {
            if (gm.unlockedWeapons[i].itemName == weaponDataSO.itemName)
            {
                upgradeOrBuyTMP.text = $"Upgrade for {weaponUpgradeInfo.damageUpgrade.damageUpgradePrices[weaponUpgradeInfo.currentDamageUpgradeLevel + 1]}";
                removeButton.gameObject.SetActive(true);
                bought = true;
            }

            else upgradeOrBuyTMP.text = $"Buy for {price.ToString()}";
        }

        if (weaponUpgradeInfo.currentDamageUpgradeLevel >= weaponUpgradeInfo.damageUpgrade.damageUpgradeLevels.Length - 1 && bought)
            BuyButton.interactable = false;
        else
            BuyButton.interactable = true;

        UpdateStatsShop();
    }

    private void UpdateStatsShop()
    {
        clipStat_TMP.text = $"Ammo: {weaponDataSO.weaponUpgradeInfo.currentClip.ToString()}";
        damageStat_TMP.text = $"Damage:  {weaponDataSO.weaponUpgradeInfo.currentDamage.ToString()}";
        // if (weaponUpgradeInfo.currentDamageUpgradeLevel >= weaponUpgradeInfo.damageUpgrade.damageUpgradeLevels.Length - 1)
        // {
        // fullyUpgraded = true;
        // upgradeOrBuyTMP.text = $"Fully Upgraded";
        // BuyButton.interactable = false;
        // }
        // else
        //     upgradeOrBuyTMP.text = $"Upgrade for {weaponUpgradeInfo.damageUpgrade.damageUpgradePrices[weaponUpgradeInfo.currentDamageUpgradeLevel + 1]}";
    }

    public override void OnBuyItem(PlayerManager playerManager)
    {
        if (!bought)
        {
            base.OnBuyItem(playerManager);

            if (!playerManager.gm.unlockedWeapons.Contains(weaponDataSO))
            {
                playerManager.gm.unlockedWeapons.Add(playerManager.gm.GetWeaponDataSoFromDict(weaponDataSO.itemName));
                OnBuyWeapon(playerManager);
            }

        }

        else OnUpgradeWeapon(playerManager);
    }

    public void OnBuyWeapon(PlayerManager playerManager)
    {
        if (playerManager.gm.CompareWeaponDataSoFromDict(weaponDataSO))
        {
            upgradeOrBuyTMP.text = $"Upgrade for {weaponUpgradeInfo.damageUpgrade.damageUpgradePrices[weaponUpgradeInfo.currentDamageUpgradeLevel + 1]}";
            bought = true;
            removeButton.gameObject.SetActive(true);

            if (weaponUpgradeInfo.currentDamageUpgradeLevel >= weaponUpgradeInfo.damageUpgrade.damageUpgradeLevels.Length - 1)
            {
                fullyUpgraded = true;
                upgradeOrBuyTMP.text = $"Fully Upgraded";
                BuyButton.interactable = false;
            }
        }
    }

    public void OnUpgradeWeapon(PlayerManager playerManager)
    {
        if (playerManager.Credit <= weaponUpgradeInfo.damageUpgrade.damageUpgradePrices[weaponUpgradeInfo.currentDamageUpgradeLevel + 1])
        {
            print(weaponUpgradeInfo.damageUpgrade.damageUpgradePrices[weaponUpgradeInfo.currentDamageUpgradeLevel + 1]);
            print("Not Enough Credit!! To Upgrade");
            return;
        }

        playerManager.Credit -= weaponUpgradeInfo.damageUpgrade.damageUpgradePrices[weaponUpgradeInfo.currentDamageUpgradeLevel + 1];
        playerManager.uiManager.UpdateCreditUI();

        if (fullyUpgraded == false)
        {
            weaponUpgradeInfo.currentDamageUpgradeLevel++;
            upgradeLevel_TMP.text = $"Upgrade Level {weaponUpgradeInfo.currentDamageUpgradeLevel + 1}/{weaponUpgradeInfo.damageUpgrade.damageUpgradeLevels.Length}";

            weaponUpgradeInfo.SetUpWeaponUpgradeInfo(weaponDataSO);
            if (weaponUpgradeInfo.currentDamageUpgradeLevel >= weaponUpgradeInfo.damageUpgrade.damageUpgradeLevels.Length - 1)
            {
                fullyUpgraded = true;
                upgradeOrBuyTMP.text = $"Fully Upgraded";
                BuyButton.interactable = false;
            }
            else
                upgradeOrBuyTMP.text = $"Upgrade for {weaponUpgradeInfo.damageUpgrade.damageUpgradePrices[weaponUpgradeInfo.currentDamageUpgradeLevel + 1]}";

        }
        UpdateStatsShop();

    }

    public override void OnRemove(PlayerManager playerManager)
    {
        if (playerManager.Credit < removeItemPrice || gm.unlockedWeapons.Count <= 2) return;
        playerManager.Credit -= removeItemPrice;
        for (int i = 0; i < playerManager.gm.unlockedWeapons.Count; i++)
        {
            if (weaponDataSO.itemName == playerManager.gm.unlockedWeapons[i].itemName)
            {
                playerManager.gm.unlockedWeapons.RemoveAt(i);
                removeButton.gameObject.SetActive(false);
                upgradeOrBuyTMP.text = $"Buy for {price.ToString()}";
                bought = false;
                BuyButton.interactable = true;
                playerManager.uiManager.UpdateCreditUI();
                break;
            }
        }
    }

    public override void OnResetGame()
    {
        base.OnResetGame();
        weaponDataSO.ResetAllUpgrade();
        SetupShop();

    }
}
