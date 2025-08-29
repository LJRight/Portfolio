using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightEnergy : MonoBehaviour
{
    public float currentEnergy = 1.0f;
    float maxEnergy = 1.0f;
    float energyRatio = 0.0005f;

    public Slider EnergyBar;
    
    void Start()
    {
        EnergyBar = GetComponent<Slider>();

        currentEnergy = maxEnergy;
        EnergyBar.value = currentEnergy;
    }

    public void ReduceEnerygy()
    {
        EnergyBar.value -= energyRatio * 1.5f;
    }

    public void IncreaseEnergy()
    {
        //if (EnergyBar.value >= 1)
        //    return;

        EnergyBar.value += energyRatio;
    }
}
