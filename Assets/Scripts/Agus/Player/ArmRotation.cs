using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmRotation : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRend;
    public Transform firePoint;
    [SerializeField] bool isFacingRight;

    private void Update()
    {
        // AimArmAtMouse();
         var tr = transform.localScale;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            tr = new Vector2(tr.x, tr.y * 1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            tr = new Vector2(tr.x, tr.y * -1);
        }
    }

    void AimArmAtMouse()
    {
        Vector2 mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 armToMouse = mousePosition - (Vector2)transform.position;
        float rotationZ = Vector2.SignedAngle(transform.right, armToMouse);
        transform.Rotate(0f, 0f, rotationZ);
        // FlipArm(Vector2.SignedAngle(transform.right, Vector2.right));

        // if(transform.rotation.z < -90f || transform.rotation.z > 90f)
        if (transform.localEulerAngles.z < 270 && transform.localEulerAngles.z > 90f)
        {
            print("facing left");

            isFacingRight = false;
        }

        else
        {
            print("facing right");
            isFacingRight = true;
        }

        print(transform.localEulerAngles.z);

        FlipTransform();
       

    }


    void FlipArm(float rotation)
    {
        var spriteRendTransform = spriteRend.transform.localScale;
        if (transform.rotation.z < -90f || transform.rotation.z > 90f)
        {
            spriteRendTransform.y = spriteRendTransform.y * -1;
            FlipFirePoint(true);
        }
        else
        {
            spriteRendTransform.y = spriteRendTransform.y * 1;

            FlipFirePoint(false);
        }
    }

    public void FlipTransform()
    {
        var trScale = transform.localScale;
        float multiplier;
        if (isFacingRight)
            multiplier = 1;

        else multiplier = -1;

        trScale = new Vector2(trScale.x, trScale.y * multiplier);
    }

    void FlipFirePoint(bool flip)
    {
        var pos = firePoint.localPosition;
        pos.x = Mathf.Abs(pos.x) * (flip ? -1 : 1);
        firePoint.localPosition = pos;
    }
}
