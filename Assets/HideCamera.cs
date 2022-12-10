using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideCamera : MonoBehaviour
{
    public GameObject cameraObject;
    private void Awake() {
        cameraObject.SetActive(false);
    }
}
