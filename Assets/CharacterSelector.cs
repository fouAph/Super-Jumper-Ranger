using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour
{
    public CharacterSelectorHelper currentCharacter;
    public CharacterDataSO currentCharDataSO;
    public Image charStatsImage;
    CharacterSelectorHelper prevChar;
    MenuManager menuManager;
    GameManager gameManager;
    private void Start()
    {
        menuManager = MenuManager.Singleton;
        gameManager = GameManager.Singleton;
        OnSelectCharacter(currentCharacter);
    }

    public void OnSelectCharacter(CharacterSelectorHelper selectedChar)
    {
        prevChar = currentCharacter;

        currentCharacter = selectedChar;
        currentCharDataSO = selectedChar.characterDataSO;
        charStatsImage.sprite = currentCharDataSO.statsSprite;
        selectedChar.selectedArrow.SetActive(true);

        menuManager.OnClickMenu();
        if (prevChar != currentCharacter)
        {
            prevChar.selectedArrow.SetActive(false);
        }
    }

    public void SetPlayerManagerCharacterData()
    {
        gameManager.currentCharacter = currentCharDataSO;
    }
    // public void OnDeselectCharacter()
    // {
    //     currentCharacter.
    // }
}
