using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] float damage = 1f;
    public float Damage { get { return damage; } }
    [SerializeField] bool isProjectile = false;
    void Update()
    {
        if (isProjectile)
            CheckCameraBound();
    }
    public void SetDamage(float damageMultiplier)
    {
        damage *= damageMultiplier;
    }
    public void Hit()
    {
        Destroy(gameObject);
    }
    void CheckCameraBound()
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
        if (viewportPos.x < -0.05f || viewportPos.x > 1.05f || viewportPos.y < -0.08f || viewportPos.y > 1.08f)
        {
            Destroy(gameObject);
        }
    }
}
