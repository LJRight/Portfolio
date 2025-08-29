using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooterManager : MonoBehaviour
{
    public event Action<float> OnPlayerFiringRateChanged;
    public event Action<float> OnPlayerDamageMultiplierChanged;
    [SerializeField] List<Shooter> shooters;
    [SerializeField] float playerFiringRate = 0.5f;
    public float PlayerFiringRate => playerFiringRate;
    [SerializeField] float maxFireRate = 0.075f;
    [SerializeField] GameObject originBullet, superBullet;
    [SerializeField] float damageMultiplier = 1.0f;
    public float DamagePercent => damageMultiplier;
    public void SynchronizingFire()
    {
        foreach (Shooter shooter in shooters)
            shooter.StopFiring();
        ReFire();
    }
    public void AllStop()
    {
        foreach (Shooter shooter in shooters)
        {
            if (shooter.enabled)
                shooter.StopFiring();
        }
    }
    public void ReFire()
    {
        bool isSound = false;
        for (int i = 0; i < shooters.Count; i++)
        {
            if (shooters[i].enabled)
            {
                shooters[i].StartFiring(!isSound);
                isSound = true;
            }
        }
    }
    void Start()
    {
        UpdateFiringRate(0);
    }
    public void ActiveSideShooter()
    {
        shooters[0].enabled = false;
        shooters[1].enabled = true;
        shooters[2].enabled = true;
        UpdateFiringRate(0);
    }
    public void ActiveTripleShooter()
    {
        shooters[0].enabled = true;
        UpdateFiringRate(0);
    }
    public void SideShooterBulletUpgrade()
    {
        shooters[1].SetBullet(superBullet);
        shooters[2].SetBullet(superBullet);
    }
    public void FrontShooterBulletUpgrade()
    {
        shooters[0].SetBullet(superBullet);
    }
    public void UpdateFiringRate(float ratio)
    {
        if (playerFiringRate == maxFireRate)
            return;
        playerFiringRate = playerFiringRate * (1f - ratio / 100f) <= maxFireRate ? maxFireRate : playerFiringRate * (1f - ratio / 100f);
        foreach (Shooter shooter in shooters)
        {
            shooter.FiringRate = playerFiringRate;
        }
        SynchronizingFire();
        OnPlayerFiringRateChanged?.Invoke(playerFiringRate);
    }
    public void ActiveFlowerShooter(int idx)
    {
        shooters[idx].gameObject.SetActive(true);
        shooters[idx].gameObject.GetComponent<FlowerShooterFadeIn>().StartFadeIn();
        shooters[idx].enabled = true;
        UpdateFiringRate(0);
    }
    public void DamageUpgrade(float ratio)
    {
        damageMultiplier += ratio / 100f;
        Debug.Log($"Damage Ratio Updated to {damageMultiplier}");
        foreach (Shooter shooter in shooters)
            shooter.Damage = damageMultiplier;
        OnPlayerDamageMultiplierChanged?.Invoke(damageMultiplier);
    }
    public bool isMaxFiringSpeed()
    {
        return playerFiringRate == maxFireRate;
    }
}
