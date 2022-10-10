using UnityEngine;
using UnityEngine.UI;

public class CountDownHelper : MonoBehaviour
{
    public Image image;
    public Sprite _3, _2, _1, _go;
    public AudioClip countdownClip;
    public void SetSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }
}
