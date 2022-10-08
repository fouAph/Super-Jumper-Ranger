using UnityEngine;
public class MenuManager : MonoBehaviour
{
    public static MenuManager Singleton;
    [SerializeField] Menu[] menus;

    public string currentMenu;
    public string previousMenu;
    private void Awake()
    {
        if (Singleton != null)
        {
            Debug.LogWarning("More than one Instance of " + Singleton.GetType());
            Debug.LogWarning("Destroying " + Singleton.name);
            DestroyImmediate(Singleton.gameObject);
        }


        Singleton = this;
    }

    public AudioClip selectMenuClip;
    public AudioClip exitSelectMenuClip;
    public AudioClip clickMenuClip;

    AudioPoolSystem audioPoolSystem;

    private void Start()
    {
        audioPoolSystem = AudioPoolSystem.Singleton;
    }

    public void OnSelectMenu()
    {
        if (audioPoolSystem && selectMenuClip)
        {
            audioPoolSystem.PlayAudio(selectMenuClip, 1f);
        }
    }

    public void OnExitSelectMenu()
    {
        if (audioPoolSystem && exitSelectMenuClip)
        {
            audioPoolSystem.PlayAudio(exitSelectMenuClip, 1f);
        }
    }

    public void OnClickMenu()
    {
        if (audioPoolSystem && clickMenuClip)
        {
            audioPoolSystem.PlayAudio(clickMenuClip, 1f);
        }
    }

    public void OpenMenu(string menuName)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].menuName == menuName)
            {
                previousMenu = currentMenu;
                menus[i].Open();
                currentMenu = menus[i].menuName;
            }
            else
            {
                menus[i].Close();
            }
        }
    }

    public void OnBackButton_Click()
    {
        OpenMenu(previousMenu);
    }
}
