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
    private bool isPathMovementEnabled;
    private Path currentPath;
    private ushort currentPathIndex;

    void Update()
    {
        if (isPathMovementEnabled)
        {
            MoveAlongPath();
        }
        else
        {
            transform.position += currentProjectile.Speed * Time.deltaTime * transform.forward;
        }

    }

    public void SetCurrentProjectile(ProjectileBaseSO newProjectile, bool canEnableMixing, Path path = null)
    {
        currentProjectile = newProjectile;

        spriteRenderer.sprite = currentProjectile.ProjectileSprite;
        animatorController.runtimeAnimatorController = currentProjectile.ProjectileAnimator;

        transform.localScale = new Vector3(currentProjectile.Size, currentProjectile.Size, 1);

        isPathMovementEnabled = path != null;

        if (isPathMovementEnabled)
        {
            currentPath = path;
            currentPathIndex = 0;
        }

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
        SetCurrentProjectile(currentProjectile, true, currentPath);

        ProjectilePooling.Instance.ReleaseProjectile(collidedProjectile.gameObject);
    }

    private IEnumerator EnableMixing()
    {
        yield return new WaitForSeconds(mixingDelay);
        currentProjectile.CanBeMixed = true;
    }

    private void MoveAlongPath()
    {
        if (currentPath.PathPoints.Count == 0) return;

        Transform targetPoint = currentPath.PathPoints[currentPathIndex].transform;
        float step = currentProjectile.Speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, step);

        Vector3 direction = (targetPoint.position - transform.position);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 180;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (Vector3.Distance(transform.position, targetPoint.position) > 0.1f) return;
        currentPathIndex++;

        if (currentPathIndex < currentPath.PathPoints.Count) return;

        isPathMovementEnabled = false;
        ProjectilePooling.Instance.ReleaseProjectile(gameObject);
    }
}
