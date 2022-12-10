using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    protected string itemName;
    [SerializeField] protected int price;
    [SerializeField] protected TMP_Text itemName_TMP;
    // [SerializeField] protected TMP_Text itemPrice_TMP;
    public TMP_Text upgradeOrBuyTMP;
    [SerializeField] protected Image itemImage;
    [SerializeField] AudioClip buttonSFX;
    [SerializeField] protected Button BuyButton;
    [SerializeField] protected bool canInteractBuyButton;
    protected virtual void Awake()
    {
        itemName_TMP.text = itemName;
        upgradeOrBuyTMP.text = $"Buy for {price.ToString()}";
        BuyButton = GetComponentInChildren<Button>();
        BuyButton.onClick.AddListener(delegate { OnBuyItem(GameManager.Singleton.playerManager); });
    }

    protected bool bought;
    public virtual void OnBuyItem(PlayerManager playerManager)
    {
        if (playerManager.Credit <= price)
        {
            print("Not Enough Credit!!!");
            bought = false;
            return;
        }
        if (buttonSFX)
            AudioPoolSystem.Singleton.PlayAudio(buttonSFX, .5f);
        bought = true;
        playerManager.Credit -= price;
        playerManager.uiManager.UpdateCreditUI();

    }

    public virtual void OnResetGame()
    {
        upgradeOrBuyTMP.text = $"Buy for {price.ToString()}";
    }
}
