using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;

public class ProjectilePrefab : MonoBehaviour
{
    public ProjectileBaseSO ProjectileBase => currentProjectile;

    public bool IsMixing = false;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animatorController;
    [Tooltip("After the projectile is spawned, how long does it take to enable mixing")]
    [SerializeField] private float mixingDelay;
    [Space(5)]
    [SerializeField] private List<ProjectileBaseSO> mixedProjectiles;

    private ProjectileBaseSO currentProjectile;

    void Update()
    {
        transform.position += currentProjectile.Speed * Time.deltaTime * transform.forward;
    }

    public void SetCurrentProjectile(ProjectileBaseSO newProjectile, bool canEnableMixing)
    {
        currentProjectile = newProjectile;

        IsMixing = false;
        spriteRenderer.sprite = currentProjectile.ProjectileSprite;
        animatorController.runtimeAnimatorController = currentProjectile.ProjectileAnimator;

        transform.localScale = new(currentProjectile.Size, currentProjectile.Size, 1);

        if (!canEnableMixing) return;
        
        StartCoroutine(EnableMixing());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DamagePlayer();
        }
        else if (!IsMixing && other.CompareTag("Projectile"))
        {
            ProjectilePrefab projectilePrefab = other.GetComponent<ProjectilePrefab>();

            if (projectilePrefab.IsMixing) return;

            MixProjectile(projectilePrefab);
        }
    }

    private void DamagePlayer()
    {
        PlayerHealth.OnDamagePlayer?.Invoke(currentProjectile.Damage);

        currentProjectile = null;
        ProjectilePooling.Instance.ReleaseProjectile(gameObject);
    }

    private void MixProjectile(ProjectilePrefab collidedProjectile)
    {
        if (currentProjectile == null || collidedProjectile.currentProjectile == null) return;

        ProjectileEnum mixedVariant = currentProjectile.MixedVariant(collidedProjectile.currentProjectile);

        if (mixedVariant == ProjectileEnum.None) return;

        IsMixing = true;
        collidedProjectile.IsMixing = true;

        currentProjectile = mixedProjectiles.Find(x => x.ProjectileType == mixedVariant);
        SetCurrentProjectile(currentProjectile, true);

        ProjectilePooling.Instance.ReleaseProjectile(collidedProjectile.gameObject);
    }

    private IEnumerator EnableMixing()
    {
        yield return new WaitForSeconds(mixingDelay);
        currentProjectile.CanBeMixed = true;
    }
}
