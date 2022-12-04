using TMPro;
public class WeaponShopItem : ShopItem
{
    public WeaponDataSO weaponDataSO;
    void Start()
    {
        itemName = weaponDataSO.itemName;
        itemName_TMP.text = weaponDataSO.itemName;
        itemPrice_TMP.text = price.ToString();
    }
    public override void OnBuyItem(PlayerManager playerManager)
    {
        base.OnBuyItem(playerManager);
        playerManager.gm.spawnerManager.itemSpawnManager.weaponPickupRandomPrefab.weaponDataSO.Add(playerManager.gm.GetWeaponDataSoFromDict(weaponDataSO.itemName));
    }

}