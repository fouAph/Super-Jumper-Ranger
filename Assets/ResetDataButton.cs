using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetDataButton : MonoBehaviour
{
    UnityEngine.UI.Button button;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<UnityEngine.UI.Button>();
        if (button)
            button.onClick.AddListener(delegate { GameManager.Singleton.ResetData(); });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
