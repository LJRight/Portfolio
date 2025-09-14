using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoyStickCheckBoxSetting : MonoBehaviour
{
    [SerializeField] List<GameObject> checkImg;
    [SerializeField] VariableJoystick joystick;
    int curMode = 0;
    void Awake()
    {
        // joystick = GetComponentInParent<VariableJoystick>(true);
    }
    public void ButtonSelected(int mode)
    {
        if (curMode == mode)
            return;
        joystick.SetMode((JoystickType)(mode + 1));
        checkImg[curMode].SetActive(false);
        curMode = mode;
        checkImg[curMode].SetActive(true);
    }
}
