using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoResizeSprite : MonoBehaviour
{
    /// <summary> Do you want the sprite to maintain the aspect ratio? </summary>
    public bool aspectRatio = true;
    public bool widthOnly = false;
    public bool heightOnly = false;
    /// <summary> Do you want it to continually check the screen size and update? </summary>
    // public bool ExecuteOnUpdate = true;
     Vector3 savedScale;
    void Start()
    {
        savedScale = new Vector3(transform.localScale.x, transform.localScale.y);

        Resize(aspectRatio);
    }

    //  void FixedUpdate () {
    //      if (ExecuteOnUpdate)
    //          Resize(aspectRatio);
    //  }

    /// <summary>
    /// Resize the attached sprite according to the camera view
    /// </summary>
    /// <param name="keepAspect">bool : if true, the image aspect ratio will be retained</param>
    void Resize(bool keepAspect = false)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        transform.localScale = new Vector3(1, 1, 1);

        // example of a 640x480 sprite
        float width = sr.sprite.bounds.size.x; // 4.80f
        float height = sr.sprite.bounds.size.y; // 6.40f

        // and a 2D camera at 0,0,-10
        float worldScreenHeight = Camera.main.orthographicSize * 2f; // 10f
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width; // 10f

        Vector3 imgScale = new Vector3(1f, 1f, 1f);
        if (widthOnly)
            imgScale = new Vector3(1, transform.localScale.y);

        else if (heightOnly)
            imgScale = new Vector3(transform.localScale.x, 1);


        // do we scale according to the image, or do we stretch it?
        if (keepAspect)
        {
            Vector2 ratio = new Vector2(width / height, height / width);
            if ((worldScreenWidth / width) > (worldScreenHeight / height))
            {
                // wider than tall
                imgScale.x = worldScreenWidth / width;
                imgScale.y = imgScale.x * ratio.y;
            }
            else
            {
                // taller than wide
                imgScale.y = worldScreenHeight / height;
                imgScale.x = imgScale.y * ratio.x;
            }
        }
        else
        {
            if (widthOnly)
            {
                imgScale.x = worldScreenWidth / width;
                imgScale.y = savedScale.y;
            }
            else if (heightOnly)
            {
                imgScale.y = worldScreenHeight / height;
                imgScale.x = savedScale.x;
            }
            else
            {

                imgScale.x = worldScreenWidth / width;
                imgScale.y = worldScreenHeight / height;
            }
        }

        // apply change
        transform.localScale = imgScale;
    }
}
