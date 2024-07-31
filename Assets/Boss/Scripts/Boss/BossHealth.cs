using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public static Action<float> OnDamageBoss;

    [SerializeField] private float maxHealth;

    [Space(10)]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color normalColor;
    [SerializeField] private Color damageColor;

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
        HUD.Instance.ActivateBossHealth(maxHealth);
    }

    private void TakeDamage(float amount)
    {
        
        currentHealth -= amount;
        SoundManager.Instance.PlaySoundOnce(Audio.enemyDamage);
        HUD.Instance.UpdateBossHealth(currentHealth);
        Debug.Log("boss health: " + currentHealth);
        StartCoroutine(DamageSprite());

        if (currentHealth > 0f)
        {
            BossAI.OnCheckStage?.Invoke(currentHealth);
            return;
        }
        Death();
    }

    private void Death()
    {
        Debug.Log("boss dead");
        HUD.Instance.DeactivateBossHealth();
        Destroy(this.gameObject);
    }


    private IEnumerator DamageSprite()
    {
        spriteRenderer.color = damageColor;
        yield return new WaitForSeconds(0.07f);
        spriteRenderer.color = normalColor;
        yield return new WaitForSeconds(0.07f);
        spriteRenderer.color = damageColor;
        yield return new WaitForSeconds(0.07f);
        spriteRenderer.color = normalColor;
    }
}
