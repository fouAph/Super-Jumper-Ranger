using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour
{
    public CharacterSelectorHelper currentSelectedCharacter;
    public CharacterDataSO currentCharDataSO;
    public Image charStatsImage;
    CharacterSelectorHelper prevChar;
    MenuManager menuManager;
    GameManager gameManager;
    private void Start()
    {
        menuManager = MenuManager.Singleton;
        gameManager = GameManager.Singleton;
        OnSelectCharacter(currentSelectedCharacter);
    }

    public void OnSelectCharacter(CharacterSelectorHelper selectedChar)
    {
        prevChar = currentSelectedCharacter;

        currentSelectedCharacter = selectedChar;
        currentCharDataSO = selectedChar.characterDataSO;
        charStatsImage.sprite = currentCharDataSO.statsSprite;
        selectedChar.selectedArrow.SetActive(true);
        gameManager.currentCharacter = currentCharDataSO;
        menuManager.OnClickMenu();
        if (prevChar != currentSelectedCharacter)
        {
            prevChar.selectedArrow.SetActive(false);
        }
    }
}
