using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public static Action<Enemy> OnEnemyKilled;
    public static Action<Enemy> OnEnemyHit;

    [SerializeField] private GameObject healthBarPrefab;
    [SerializeField] private Transform barPosition;
    [SerializeField] private float initialHealth = 10f;
    [SerializeField] private float maxHealth = 10f;

    public float CurrentHealth;
    private Image healthBar;
    private TextMeshProUGUI healthText;
    private Enemy enemy;
    private EnemyFX enemyFX;
    GameObject newBar;
    private void Start()
    {
        initialHealth = (int)(((float)(LevelManager.Instance.CurrentWave / 20f) + 1f) * initialHealth);
        CurrentHealth = initialHealth;
        maxHealth = CurrentHealth;
        CreateHealthBar();
        enemy = GetComponent<Enemy>();
        enemyFX = GetComponent<EnemyFX>();
    }

    private void Update()
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount,CurrentHealth / maxHealth, Time.deltaTime * 10f);
    }

    private void CreateHealthBar()
    {
        newBar = Instantiate(healthBarPrefab, barPosition.position, Quaternion.identity);
        newBar.transform.SetParent(transform);
        EnemyHealthContainer container = newBar.GetComponent<EnemyHealthContainer>();
        healthBar = container.FillAmountImage;
        if (container)
            container.healthText.text = maxHealth.ToString();
    }

    public void DealDamage(float damageReceived)
    {
        CurrentHealth -= damageReceived;
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            Die();
        }
        else
            OnEnemyHit?.Invoke(enemy);
        UpdateTextHealth();
    }

    private void UpdateTextHealth()
    {
        EnemyHealthContainer container = newBar.GetComponent<EnemyHealthContainer>();
        if (container)
            container.healthText.text = CurrentHealth.ToString();
    }
    public void ResetHealth()
    {
        initialHealth = (int)(((float)(LevelManager.Instance.CurrentWave / 20f) + 1f) * initialHealth);
        CurrentHealth = initialHealth;
        maxHealth = CurrentHealth;
        healthBar.fillAmount = 1f;
        UpdateTextHealth();
    }
    
    private void Die()
    {
        OnEnemyKilled?.Invoke(enemy);
    }
}
