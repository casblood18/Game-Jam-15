using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public static Action<float> OnDamageBoss;

    [SerializeField] private float maxHealth;

    private float currentHealth;

    private void OnEnable()
    {
        OnDamageBoss += TakeDamage;
    }

    private void OnDisable()
    {
        OnDamageBoss -= TakeDamage;
    }

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth > 0f)
        {
            BossAI.OnCheckStage?.Invoke(currentHealth);
            return;
        }

        Death();
    }

    private void Death()
    {

    }

}
