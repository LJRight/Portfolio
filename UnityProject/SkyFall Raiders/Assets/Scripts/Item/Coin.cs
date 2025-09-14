using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int[] coinValue = { 5, 40, 250 };
    [SerializeField] private float[] scale = { 1f, 1.25f, 2f };
    private int moneyValue = 10;
    public int Value => moneyValue;
    public void SetMoneyValue(int value)
    {
        for (int i = 0; i < coinValue.Length; i++)
        {
            if (value == coinValue[i])
                transform.localScale *= scale[i];
        }
        moneyValue = value;
    }
}
