using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    #region Singleton
    public static ShopManager Singleton;
    public GameObject shopGameobjectHolder;
    public List<ShopItem> shopItems = new List<ShopItem>();
    private void Awake()
    {
        if (Singleton != null)
        {
            Debug.LogWarning("More than one Instance of " + Singleton.GetType());
            Debug.LogWarning("Destroying " + Singleton.name);
            DestroyImmediate(Singleton.gameObject);
        }

        Singleton = this;

        shopGameobjectHolder.SetActive(false);

        // foreach (var item in shopItems)
        // {
        //     item.SetupShop();
        // }
    }

    #endregion

    public void ResetShopItem()
    {
        StartCoroutine(RestShopItem_Routine());
    }

    IEnumerator RestShopItem_Routine()
    {
        shopGameobjectHolder.SetActive(true);
        yield return new WaitForSeconds(.1f);
        shopGameobjectHolder.SetActive(false);

        foreach (var item in shopItems)
        {
            item.SetupShop();
        }

    }

    public void OpenCloseShop()
    {
        showShop = !showShop;
        shopGameobjectHolder.SetActive(showShop);
    }
    public void OpenAndCloseShop(GameManager gm)
    {
        shopItems = GetComponentsInChildren<ShopItem>().ToList();

        showShop = !showShop;
        shopGameobjectHolder.SetActive(showShop);
        if (showShop)
        {
            gm.PauseGame();
            gm.gameState = GameState.InShop;
        }
        else gm.ResumeGame();
    }
    public bool showShop;

}
