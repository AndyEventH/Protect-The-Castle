using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimations : MonoBehaviour
{
    public GameObject deathParticles;
    private Animator animator;
    private Enemy enemy;
    private EnemyHealth enemyHealth;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
        enemy = GetComponent<Enemy>();
        enemyHealth = GetComponent<EnemyHealth>();
    }

    private void PlayHurtAnimation()
    {
        animator.SetTrigger("Hurt");
    }



    private float GetCurrentAnimationLenght()
    {
        float animationLenght = animator.GetCurrentAnimatorStateInfo(0).length;
        return animationLenght;
    }
    
    private IEnumerator PlayHurt()
    {
        enemy.StopMovement();
        PlayHurtAnimation();
        yield return new WaitForSeconds(GetCurrentAnimationLenght() + 0.3f);
        enemy.ResumeMovement();
    }

    private IEnumerator PlayDead()
    {
        enemy.StopMovement();
        Instantiate(deathParticles, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.1f);
        enemy.ResumeMovement();
        enemyHealth.ResetHealth();
        ObjectPooler.ReturnToPool(enemy.gameObject);
    }
    
    private void EnemyHit(Enemy enemy)
    {
        if (this.enemy == enemy)
        {
            StartCoroutine(PlayHurt());
        }
    }

    private void EnemyDead(Enemy enemy)
    {
        if (this.enemy == enemy)
        {
            StartCoroutine(PlayDead());
        }
    }
    
    private void OnEnable()
    {
        EnemyHealth.OnEnemyHit += EnemyHit;
        EnemyHealth.OnEnemyKilled += EnemyDead;
    }

    private void OnDisable()
    {
        EnemyHealth.OnEnemyHit -= EnemyHit;
        EnemyHealth.OnEnemyKilled -= EnemyDead;
    }
}
