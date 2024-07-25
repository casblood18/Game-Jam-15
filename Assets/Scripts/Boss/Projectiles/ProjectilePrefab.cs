using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProjectilePrefab : MonoBehaviour
{
    public ProjectileBaseSO ProjectileBase => currentProjectile;

    public bool IsMixing = false;
    [SerializeField] private SpriteRenderer spriteRenderer;
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
        transform.localScale = new(currentProjectile.Size, currentProjectile.Size, 1);

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
