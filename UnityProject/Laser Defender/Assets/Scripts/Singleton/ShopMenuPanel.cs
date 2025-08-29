using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenuPanel : MonoBehaviour
{
    public enum ItemType { Life, AttackSpeed, Damage };
    [SerializeField] List<GameObject> items;
    public (float value, int price)[] data;
    float baseRatio = 5f;
    int basePrice = 100;
    int lifeAmount = 1, lifePrice = 200;
    private void OnEnable()
    {
        InitData();
        RefreshTexts();
        ButtonListenerSetup();
    }
    private void RefreshTexts()
    {
        bool isMaxFiringSpeed = FindFirstObjectByType<PlayerShooterManager>().isMaxFiringSpeed();
        for (int i = 0; i < items.Count; i++)
        {
            var (val, price) = data[i];
            var tmps = items[i].GetComponentsInChildren<TextMeshProUGUI>();
            switch ((ItemType)i)
            {
                case ItemType.Life:
                    tmps[0].SetText("Life");
                    break;
                case ItemType.AttackSpeed:
                    if (isMaxFiringSpeed)
                        tmps[0].SetText("Max Atk Speed");
                    else
                        tmps[0].SetText($"+{val}% Atk Speed");
                    break;
                case ItemType.Damage:
                    tmps[0].SetText($"+{val}% Damage");
                    break;
            }
            tmps[1].SetText($"{price}");
            if (price > ScoreKeeper.Instance.Coin || ((ItemType)i == ItemType.AttackSpeed && isMaxFiringSpeed))
            {
                tmps[1].color = Color.red;
                items[i].GetComponentInChildren<Button>().interactable = false;
            }
            else
            {
                tmps[1].color = Color.green;
                items[i].GetComponentInChildren<Button>().interactable = true;
            }
        }
    }
    private void ButtonListenerSetup()
    {
        for (int i = 0; i < Enum.GetValues(typeof(ItemType)).Length; i++)
        {
            int btIdx = i;
            Button bt = items[i].GetComponentInChildren<Button>();
            if (bt == null)
            {
                Debug.LogWarning($"{Enum.GetNames(typeof(ItemType))[i]} Item Button not found");
                return;
            }
            bt.onClick.RemoveAllListeners();
            bt.onClick.AddListener(() =>
            {
                GameController.Instance.PlayGameTime();
                AudioPlayer.Instance.PlayButtonSelectClip();
                ItemButtonSelected(btIdx);
                ScoreKeeper.Instance.AddCoin(-data[btIdx].price);
                UIManager.Instance.ShopPanelActive(false);
            });
        }
    }
    private void InitData()
    {
        data = new (float, int)[Enum.GetValues(typeof(ItemType)).Length];
        data[(int)ItemType.Life] = (lifeAmount, lifePrice);
        int r = WeightedRandom.Pick();
        data[(int)ItemType.AttackSpeed] = (r * baseRatio, r * basePrice);
        r = WeightedRandom.Pick();
        data[(int)ItemType.Damage] = (r * baseRatio, r * basePrice);
    }
    private void ItemButtonSelected(int idx)
    {
        switch (idx)
        {
            case 0:
                FindFirstObjectByType<PlayerMovement>()?.gameObject.GetComponent<Health>().GetLife();
                break;
            case 1:
                FindFirstObjectByType<PlayerShooterManager>().UpdateFiringRate(data[idx].value);
                break;
            case 2:
                FindFirstObjectByType<PlayerShooterManager>().DamageUpgrade(data[idx].value);
                break;
        }
    }

    private static class WeightedRandom
    {
        static readonly int[] values = { 1, 2, 3, 4, 5, 6, 7, 8 };
        static readonly int[] weights = { 17, 23, 35, 12, 7, 3, 2, 1 };
        static readonly int total = 100;
        public static int Pick()
        {
            int rand = UnityEngine.Random.Range(0, total);
            int sum = 0;
            for (int i = 0; i < weights.Length; i++)
            {
                sum += weights[i];
                if (rand < sum) return values[i];
            }
            return values[^1];
        }
    }
}