using UnityEngine;

public class MobileController : MonoBehaviour
{
    // public GameObject mobileControllerCanvas;

    [Header("Mobile Settings")]
    public MobileControlScheme controlScheme;
    [HideInInspector] public bool use360Aim;
    public float direction;
    [HideInInspector] public bool jumpPressed;
    [HideInInspector] public bool isFacingRight;
    [HideInInspector] public bool isFiring;
    [HideInInspector] public bool useJoystickToMove;
    public MyJoystick movementJoystick;
    public MyJoystick shootJoystick;
    public float shootThreshold = .8f;
    public float jumpThreshold = .7f;
    public GameObject[] buttonObjects;
    public GameObject openShopButtonObject;
    public void SetMobileControllScheme(GameManager gm)
    {
        switch (controlScheme)
        {
            case MobileControlScheme.Analog:
                gm.flipMode = CharacterFlipMode.ByMousePosition;
                AnalogControll();

                break;
            case MobileControlScheme.Button:
                gm.flipMode = CharacterFlipMode.ByMoveDirection;
                ButtonControll();

                break;
            case MobileControlScheme.Hybrid:
                gm.flipMode = CharacterFlipMode.ByMousePosition;
                HybridControll();

                break;
        }
    }

    // private void Start()
    // {
    //     mobileControllerCanvas.SetActive(false);
    // }

    public void ChangeControllButton()
    {
        int curControll = (int)controlScheme;
        if (curControll < 2)
        {
            controlScheme++;
        }
        else
            controlScheme = 0;
    }

    private void AnalogControll()
    {
        useJoystickToMove = true;

        shootJoystick.gameObject.SetActive(true);
        movementJoystick.gameObject.SetActive(true);
        for (int i = 0; i < buttonObjects.Length; i++)
        {
            buttonObjects[i].SetActive(false);
            if (buttonObjects[i].name == "Pause_Button")
            {
                buttonObjects[i].SetActive(true);
            }
        }

    }

    private void ButtonControll()
    {
        // use360Aim = false;
        useJoystickToMove = false;

        shootJoystick.gameObject.SetActive(false);
        movementJoystick.gameObject.SetActive(false);
        for (int i = 0; i < buttonObjects.Length; i++)
        {
            if (buttonObjects[i].name == "Hybrid_Jump_Button")
                buttonObjects[i].SetActive(false);
            else
                buttonObjects[i].SetActive(true);

            if (buttonObjects[i].name == "Pause_Button")
            {
                buttonObjects[i].SetActive(true);
            }
        }
    }

    private void HybridControll()
    {
        // use360Aim = true;
        useJoystickToMove = false;

        shootJoystick.gameObject.SetActive(true);
        movementJoystick.gameObject.SetActive(false);
        foreach (var item in buttonObjects)
        {
            if (item.gameObject.name == "MoveLeft_Button" || item.gameObject.name == "MoveRight_Button" || item.gameObject.name == "Hybrid_Jump_Button" || item.gameObject.name == "Pause_Button")
            {
                item.SetActive(true);
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