using TMPro;
using UnityEngine;
public class WeaponShopItem : ShopItem
{
    [SerializeField] WeaponDataSO weaponDataSO;
    [SerializeField] TMP_Text upgradeLevel_TMP;
    private GameManager gm;
    void Start()
    {
        price = weaponDataSO.weaponUpgradeInfo.itemPrice;
        gm = GameManager.Singleton;
        itemName = weaponDataSO.itemName;
        itemName_TMP.text = weaponDataSO.itemName;

        upgradeLevel_TMP.text = $"Upgrade Level {weaponDataSO.weaponUpgradeInfo.currentDamageUpgradeLevel + 1}/{weaponDataSO.weaponUpgradeInfo.damageUpgrade.damageUpgradeLevels.Length}";
        upgradeOrBuyTMP.text = $"Buy for {price.ToString()}";
        if (itemImage) itemImage.sprite = weaponDataSO.itemSprite;
        for (int i = 0; i < gm.unlockedWeapons.Count; i++)
        {
            if (gm.unlockedWeapons[i].itemName == weaponDataSO.itemName)
            {
                upgradeOrBuyTMP.text = $"Upgrade for {weaponDataSO.weaponUpgradeInfo.damageUpgrade.damageUpgradePrices[weaponDataSO.weaponUpgradeInfo.currentDamageUpgradeLevel + 1]}";
                removeButton.gameObject.SetActive(true);
                bought = true;
            }
        }

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
                print("buying Weapon");
            }

        }
        else
        {
            OnUpgradeWeapon(playerManager);
            print("upgrading Weapon");

        }
    }

    public void OnBuyWeapon(PlayerManager playerManager)
    {
        if (playerManager.gm.CompareWeaponDataSoFromDict(weaponDataSO))
        {
            upgradeOrBuyTMP.text = $"Upgrade for {weaponDataSO.weaponUpgradeInfo.damageUpgrade.damageUpgradePrices[weaponDataSO.weaponUpgradeInfo.currentDamageUpgradeLevel + 1]}";
            // bought = true;
            removeButton.gameObject.SetActive(true);
        }
    }

    public void OnUpgradeWeapon(PlayerManager playerManager)
    {
        if (playerManager.Credit <= weaponDataSO.weaponUpgradeInfo.damageUpgrade.damageUpgradePrices[weaponDataSO.weaponUpgradeInfo.currentDamageUpgradeLevel + 1])
        {
            print("Not Enough Credit!! To Upgrade");
            return;
        }

        print($"Upgrading to Level {weaponDataSO.weaponUpgradeInfo.damageUpgrade.damageUpgradePrices[weaponDataSO.weaponUpgradeInfo.currentDamageUpgradeLevel + 1]}");

        playerManager.Credit -= weaponDataSO.weaponUpgradeInfo.damageUpgrade.damageUpgradePrices[weaponDataSO.weaponUpgradeInfo.currentDamageUpgradeLevel + 1];

        playerManager.uiManager.UpdateCreditUI();
        bool fullyUpgraded = false;

        if (fullyUpgraded == false)
        {
            var info = weaponDataSO.weaponUpgradeInfo;
            info.currentDamageUpgradeLevel++;
            upgradeLevel_TMP.text = $"Upgrade Level {weaponDataSO.weaponUpgradeInfo.currentDamageUpgradeLevel + 1}/{weaponDataSO.weaponUpgradeInfo.damageUpgrade.damageUpgradeLevels.Length}";
            // upgradeLevel_TMP.text = $"Upgrade Level {gm.savedCurrentLevelUpgrade[weaponDataSO.itemName]}/{weaponDataSO.weaponUpgradeInfo.damageUpgrade.damageUpgradeLevels.Length}";
            info.UpgradeDamage();
            if (info.currentDamageUpgradeLevel >= info.damageUpgrade.damageUpgradeLevels.Length - 1)
            {
                fullyUpgraded = true;
                upgradeOrBuyTMP.text = $"Fully Upgraded";
                BuyButton.interactable = false;
            }


        }
        else
        {
            upgradeLevel_TMP.text = $"Fully Upgraded";
            BuyButton.interactable = false;
        }
    }

    bool UpgradeDamage()
    {
        var info = weaponDataSO.weaponUpgradeInfo;
        info.currentDamageUpgradeLevel++;
        info.currentDamage = info.damageUpgrade.damageUpgradeLevels[info.currentDamageUpgradeLevel + 1];

        if (info.currentDamageUpgradeLevel >= info.damageUpgrade.damageUpgradeLevels.Length)
        {
            return true;
        }
        return false;
    }


    public override void OnRemove(PlayerManager playerManager)
    {
        bool removed = false;
        if (removed || playerManager.Credit <= removeItemPrice || gm.unlockedWeapons.Count <= 2) return;
        for (int i = 0; i < playerManager.gm.unlockedWeapons.Count; i++)
        {
            if (weaponDataSO.itemName == playerManager.gm.unlockedWeapons[i].itemName)
            {
                playerManager.gm.unlockedWeapons.RemoveAt(i);
                removed = true;
                removeButton.gameObject.SetActive(false);
                upgradeOrBuyTMP.text = $"Buy for {price.ToString()}";

                break;
            }
        }
    }

    public override void OnResetGame()
    {
        base.OnResetGame();
        upgradeLevel_TMP.text = $"Upgrade Level {weaponDataSO.weaponUpgradeInfo.currentDamageUpgradeLevel + 1}/{weaponDataSO.weaponUpgradeInfo.damageUpgrade.damageUpgradeLevels.Length}";

        for (int i = 0; i < gm.unlockedWeapons.Count; i++)
        {
            if (gm.unlockedWeapons[i].itemName == weaponDataSO.itemName)
            {
                upgradeOrBuyTMP.text = $"Upgrade for {weaponDataSO.weaponUpgradeInfo.damageUpgrade.damageUpgradePrices[weaponDataSO.weaponUpgradeInfo.currentDamageUpgradeLevel + 1]}";
                removeButton.gameObject.SetActive(true);
            }
        }
    }
}
