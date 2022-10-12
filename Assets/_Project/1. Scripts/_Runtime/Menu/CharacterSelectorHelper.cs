using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CharacterSelectorHelper : MonoBehaviour, IPointerClickHandler
{
    public CharacterDataSO characterDataSO;
    public GameObject selectedArrow;
    CharacterSelector characterSelector;
    private void Start() {
        characterSelector = GetComponentInParent<CharacterSelector>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        characterSelector.OnSelectCharacter(this);
    }
}
