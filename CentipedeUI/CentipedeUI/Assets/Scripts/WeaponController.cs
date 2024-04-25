using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField, Header("Objects References")]
    Transform muzzlePoint;

    [SerializeField]
    Transform poolParent;

    [SerializeField, Header("Prefab References")]
    Bullet bulletPrefab;

    [SerializeField, Header("Values")]
    float fireDelay;

    [SerializeField]
    int poolAmount;

    List<Bullet> bullets = new List<Bullet>();
    float timer;

    void Start()
    {
        InitializePool();
        timer = 0;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (Input.GetButtonDown("Fire1"))
        {
            if (timer < 0)
            {
                Shoot();
                timer = fireDelay;
            }
        }
    }

    void InitializePool()
    {
        for (int i = 0; i < poolAmount; i++)
        {
            var obj = Instantiate(bulletPrefab, poolParent);
            obj.gameObject.SetActive(false);
            bullets.Add(obj);
        }
    }

    Bullet GetActiveBullet()
    {
        foreach (var b in bullets)
        {
            if (!b.gameObject.activeInHierarchy)
            {
                b.gameObject.SetActive(true);
                return b;
            }
        }

        Bullet extraBullet = Instantiate(bulletPrefab, poolParent);
        bullets.Add(extraBullet);
        extraBullet.gameObject.SetActive(true);
        return extraBullet;
    }

    void Shoot()
    {
        GetActiveBullet().Shoot(muzzlePoint.position);
    }
}