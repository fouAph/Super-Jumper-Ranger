using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameStartButton : MonoBehaviour
{
    Button button;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();

        if (button)
        {
            if (GameManager.Singleton)
                button.onClick.AddListener(delegate { GameManager.Singleton.OnPlayGameButton();});
            else Debug.LogWarning("GameManager Script Cannot be found");
        }
    }
}
