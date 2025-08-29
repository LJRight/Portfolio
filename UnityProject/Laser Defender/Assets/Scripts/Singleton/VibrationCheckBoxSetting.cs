using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrationCheckBoxSetting : MonoBehaviour
{
    [SerializeField] GameObject img;
    bool isSelected = true;
    public void ButtonSelected()
    {
        isSelected = !isSelected;
        img.SetActive(isSelected);
        GameController.Instance.ChangeVibrateMode(isSelected);
    }
}
