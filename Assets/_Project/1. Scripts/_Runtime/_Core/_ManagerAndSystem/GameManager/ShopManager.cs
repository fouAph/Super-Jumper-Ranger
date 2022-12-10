using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    #region Singleton
    public static ShopManager Singleton;
    public GameObject shopCanvasObject;
    public List<ShopItem> shopItems = new List<ShopItem>();
    private void Awake()
    {
        if (Singleton != null)
        {
            Debug.LogWarning("More than one Instance of " + Singleton.GetType());
            Debug.LogWarning("Destroying " + Singleton.name);
            DestroyImmediate(Singleton.gameObject);
        }
        shopItems = GetComponentsInChildren<ShopItem>().ToList();

        Singleton = this;

        shopCanvasObject.SetActive(false);
    }
    #endregion

    public bool isOpen;

}
