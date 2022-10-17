using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlayerBodyParts : MonoBehaviour
{
    private Transform tr;
    public Transform playerBody;
    public Transform[] transformsToRotate;
    public bool isFacingRight;
    Vector3 difference;
    GameManager gm;
    private void Awake()
    {
        tr = transform;
        gm = GameManager.Singleton;
    }
    private void FixedUpdate()
    {
        if (gm.useMobileControll)
            difference = new Vector3(gm.shootJoystick.xValue, gm.shootJoystick.yValue, 0);
        else
            difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        difference.Normalize();

        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        gm.isFacingRight = isFacingRight = difference.x >= .01f;

        for (int i = 0; i < transformsToRotate.Length; i++)
        {
            transformsToRotate[i].rotation = Quaternion.Euler(0f, 0f, rotationZ);

            if (rotationZ < -90 || rotationZ > 90)
            {
                if (tr.transform.eulerAngles.y == 0)
                {
                    transformsToRotate[i].localRotation = Quaternion.Euler(180, 0, -rotationZ);
                }

                else if (tr.transform.eulerAngles.y == 180)
                {
                    transformsToRotate[i].localRotation = Quaternion.Euler(180, 180, -rotationZ);

                }
            }
        }
        if (playerBody)
        {
            if (isFacingRight)
            {
                playerBody.localRotation = Quaternion.Euler(0, 0, 0);

            }
            else
                playerBody.localRotation = Quaternion.Euler(180, 0, 180);
        }
    }

}
