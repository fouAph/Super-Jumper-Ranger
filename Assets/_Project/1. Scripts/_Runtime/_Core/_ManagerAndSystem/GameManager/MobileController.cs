using UnityEngine;

public class MobileController : MonoBehaviour
{
    [Header("Mobile Settings")]
    public MobileControlScheme controlScheme;
    [HideInInspector] public bool use360Aim;
    [HideInInspector] public float direction;
    [HideInInspector] public bool jumpPressed;
    [HideInInspector] public bool isFacingRight;
    [HideInInspector] public bool isFiring;
    [HideInInspector] public bool useJoystickToMove;
    public MyJoystick movementJoystick;
    public MyJoystick shootJoystick;
    public float shootThreshold = .8f;
    public float jumpThreshold = .9f;
    public GameObject[] buttonObjects;

    public void SetMobileControllScheme()
    {
        switch (controlScheme)
        {
            case MobileControlScheme.Analog:
                AnalogControll();

                break;
            case MobileControlScheme.Button:
                ButtonControll();

                break;
            case MobileControlScheme.Hybrid:
                HybridControll();

                break;
        }
    }

    private void AnalogControll()
    {
        use360Aim = true;
        useJoystickToMove = true;

        shootJoystick.gameObject.SetActive(true);
        movementJoystick.gameObject.SetActive(true);
        for (int i = 0; i < buttonObjects.Length; i++)
        {
            buttonObjects[i].SetActive(false);
        }
    }

    private void ButtonControll()
    {
        use360Aim = false;
        useJoystickToMove = false;

        shootJoystick.gameObject.SetActive(false);
        movementJoystick.gameObject.SetActive(false);
        for (int i = 0; i < buttonObjects.Length; i++)
        {
            buttonObjects[i].SetActive(true);
        }
    }

    private void HybridControll()
    {
        use360Aim = true;
        useJoystickToMove = false;

        shootJoystick.gameObject.SetActive(true);
        movementJoystick.gameObject.SetActive(false);
        foreach (var item in buttonObjects)
        {
            if (item.gameObject.name == "MoveLeft_Button" || item.gameObject.name == "MoveRight_Button" || item.gameObject.name == "Jump_Button(2)")
            {
                continue;
            }
            else item.SetActive(false);
        }
    }

    #region Button Touch Methods
    public void SetDirection(float dir)
    {
        direction = dir;
    }

    public void SetJumpPressed(bool state)
    {
        jumpPressed = state;
    }

    public void SetIsFiring(bool state)
    {
        isFiring = state;
    }

    #endregion

    public enum MobileControlScheme { Analog, Button, Hybrid }
}