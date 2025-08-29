using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    [SerializeField] PlayerShooterManager playerShooterManager;
    int power = 0;
    private float basePower;
    private int baseline = 3;
    private PlayerShooterManager psm;
    void Awake()
    {
        psm = GetComponent<PlayerShooterManager>();
        basePower = CalculatePlayerPower();
        psm.OnPlayerDamageMultiplierChanged += _ => NotifyDPSChanged();
        psm.OnPlayerFiringRateChanged += _ => NotifyDPSChanged();
    }
    private void NotifyDPSChanged()
    {
        UIManager.Instance.SetPlayerDPSText();
    }
    public void PowerUp()
    {
        power++;
        switch (power)
        {
            case 1:
                playerShooterManager.ActiveSideShooter();
                break;
            case 2:
                playerShooterManager.ActiveTripleShooter();
                break;
            case 3:
                playerShooterManager.FrontShooterBulletUpgrade();
                break;
            case 4:
                playerShooterManager.SideShooterBulletUpgrade();
                break;
            case 5:
                playerShooterManager.ActiveFlowerShooter(power - 2);
                break;
            case 6:
                playerShooterManager.ActiveFlowerShooter(power - 2);
                break;
            case 7:
                playerShooterManager.ActiveFlowerShooter(power - 2);
                break;
            case 8:
                playerShooterManager.ActiveFlowerShooter(power - 2);
                break;
            default:
                ScoreKeeper.Instance.AddScore(500);
                break;
        }
        NotifyDPSChanged();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PowerUp"))
        {
            PowerUp();
            AudioPlayer.Instance.PlayPowerUpClip();
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Coin"))
        {
            AudioPlayer.Instance.PlayCoinClip();
            ScoreKeeper.Instance.AddCoin(other.gameObject.GetComponent<Coin>().money);
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Shop"))
        {
            AudioPlayer.Instance.PlayShopItemClip();
            GameController.Instance.StopGameTime();
            UIManager.Instance.ShopPanelActive(true);
            Destroy(other.gameObject);
        }
    }
    private float CalculatePlayerPower()
    {
        int projectilePower = power >= baseline ? power + 2 : power + 1;
        return projectilePower * psm.DamagePercent / psm.PlayerFiringRate;
    }
    public float GetPlayerDPS()
    {
        return CalculatePlayerPower();
    }
    public float GetPlayerPowerMultiplier()
    {
        return CalculatePlayerPower() / basePower;
    }
}

