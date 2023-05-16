using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretProjectile : MonoBehaviour
{
    [SerializeField] public Transform projectileSpawnPosition;
    [SerializeField] public float delayBtwAttacks = 2f;
    [SerializeField] public float damage = 2f;
    public float Damage;
    public float DelayPerShot;
    
    public float nextAttackTime;
    public ObjectPooler pooler;
    public Turret turret;
    public Projectile currentProjectileLoaded;

    private void Start()
    {
        turret = GetComponent<Turret>();
        pooler = GetComponent<ObjectPooler>();

        Damage = damage;
        DelayPerShot = delayBtwAttacks;
        LoadProjectile();
    }

    protected virtual void Update()
    {
        if (IsTurretEmpty())
        {
            LoadProjectile();
        }

        if (Time.time > nextAttackTime)
        {
            if (turret.CurrentEnemyTarget != null && currentProjectileLoaded != null &&
                turret.CurrentEnemyTarget.EnemyHealth.CurrentHealth > 0f)
            {
                currentProjectileLoaded.transform.parent = null;
                currentProjectileLoaded.SetEnemy(turret.CurrentEnemyTarget);
                AudioManager.Instance.PlayerSound(AudioManager.Sound.rocket);
            }

            nextAttackTime = Time.time + DelayPerShot;
        }
    }

    protected virtual  void LoadProjectile()
    {
        GameObject newInstance = pooler.GetInstanceFromPool();
        newInstance.transform.localPosition = projectileSpawnPosition.position;
        newInstance.transform.SetParent(projectileSpawnPosition);

        currentProjectileLoaded = newInstance.GetComponent<Projectile>();
        currentProjectileLoaded.TurretOwner = this;
        currentProjectileLoaded.ResetProjectile();
        currentProjectileLoaded.Damage = Damage;
        newInstance.SetActive(true);
    }

    private bool IsTurretEmpty()
    {
        return currentProjectileLoaded == null;
    }
    
    public void ResetTurretProjectile()
    {
        currentProjectileLoaded = null;
    }
}