using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [SerializeField] protected string itemName;
    [SerializeField] protected int price;
    [SerializeField] protected TMP_Text itemName_TMP;
    [SerializeField] protected TMP_Text itemPrice_TMP;
    [SerializeField] protected Sprite itemIcon;
    [SerializeField] AudioClip buttonSFX;
    protected Button BuyButton;
   [SerializeField]  protected bool canInteractBuyButton;
    protected virtual void Awake()
    {
        itemName_TMP.text = itemName;
        itemPrice_TMP.text = price.ToString();
        BuyButton = GetComponentInChildren<Button>();
        BuyButton.onClick.AddListener(delegate { OnBuyItem(GameManager.Singleton.playerManager); });
    }

    public virtual void OnBuyItem(PlayerManager playerManager)
    {
        if (buttonSFX)
            AudioPoolSystem.Singleton.PlayAudio(buttonSFX, .5f);

        if (!canInteractBuyButton)
        {
            BuyButton.interactable = false;
            var tmp = BuyButton.GetComponentInChildren<TMP_Text>();
            tmp.text = "Purchased";
        }
    }
}
