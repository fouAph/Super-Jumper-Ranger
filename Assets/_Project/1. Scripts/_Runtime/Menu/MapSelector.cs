using System.Collections;
using UnityEngine;

public class MapSelector : MonoBehaviour
{
    public MapSelectorHelper currentSelectedMap;
    public MapDataSO selectedMapDataSO;
    MapSelectorHelper prevMap;
    MenuManager menuManager;
    GameManager gameManager;
    private void Start()
    {
        menuManager = MenuManager.Singleton;
        gameManager = GameManager.Singleton;
        OnMapSelected(currentSelectedMap);
        StartCoroutine(SelectRoutine());
    }

    IEnumerator SelectRoutine()
    {
        yield return new WaitForSeconds(.2f);
        currentSelectedMap.SetSpriteToSelected();
    }

    public void OnMapSelected(MapSelectorHelper selectedMap)
    {
        prevMap = currentSelectedMap;

        currentSelectedMap = selectedMap;
        selectedMapDataSO = selectedMap.mapDataSO;
        gameManager.currentMap = selectedMapDataSO;
        gameManager.SetMapSettings();
        menuManager.OnClickMenu();
        if (prevMap != currentSelectedMap)
        {
            prevMap.SetSpriteToDefault();
        }
    }
}
