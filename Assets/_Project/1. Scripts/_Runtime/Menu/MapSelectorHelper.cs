using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapSelectorHelper : MonoBehaviour, IPointerClickHandler
{
    public MapDataSO mapDataSO;
    MapSelector mapSelector;
    Image buttonImage;
    [SerializeField] Sprite selectedSprite;
    [SerializeField] Sprite lockedSprite;
    Sprite defaultSprite;
    Button button;
    private void Start()
    {
        mapSelector = GetComponentInParent<MapSelector>();
        buttonImage = GetComponent<Image>();
        button = GetComponent<Button>();
        defaultSprite = buttonImage.sprite;

        UpdateButtonInteractable();
    }

    public void UpdateButtonInteractable()
    {
        button.interactable = mapDataSO.unlocked;
        buttonImage.sprite = button.interactable ? defaultSprite : lockedSprite;
    }

    public void SetSpriteToSelected()
    {
        buttonImage.sprite = selectedSprite;
    }

    public void SetSpriteToDefault()
    {
        buttonImage.sprite = defaultSprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!button.interactable) return;
        mapSelector.OnMapSelected(this);
        mapSelector.currentSelectedMap.SetSpriteToSelected();
    }



}

