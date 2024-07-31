using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;

public class ProjectilePrefab : MonoBehaviour
{
    public ProjectileBaseSO ProjectileBase => currentProjectile;

    public bool IsMixing = false;
    public bool CanBackshotMix = false;

    public ushort SignedNumber => signedNumber;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animatorController;
    [Tooltip("After the projectile is spawned, how long does it take to enable mixing")]
    [SerializeField] private float mixingDelay;
    [Space(5)]
    [SerializeField] private List<ProjectileBaseSO> mixedProjectiles;

    [Space(5)]
    [SerializeField] private Collider2D col;
    [SerializeField] private GameObject disableGO;

    private ProjectileBaseSO currentProjectile;
    private bool isPathMovementEnabled = false;
    private bool canBeDestroyed = true;

    private Path currentPath;
    private ushort currentPathIndex;
    private float customProjectileSpeed = 0.0f;
    private ushort signedNumber = 0;

    private List<Vector3> savedPathPoints;

    void OnEnable()
    {
        disableGO.SetActive(false);
        col.enabled = true;
    }

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

        CheckIfOutOfView();
    }

    public void SetCurrentProjectile(ProjectileBaseSO newProjectile, bool canEnableMixing, Path path = null, float customProjectileSpeed = 1f, bool canBeDestroyed = true, ushort signedNumber = 0)
    {
        currentProjectile = newProjectile;

        this.signedNumber = signedNumber;
        this.customProjectileSpeed = customProjectileSpeed;
        this.canBeDestroyed = canBeDestroyed;

        IsMixing = true;

        animatorController.runtimeAnimatorController = currentProjectile.ProjectileAnimator;

        transform.localScale = new Vector3(currentProjectile.Size / 4, currentProjectile.Size / 4, 1);

        if (path != null)
        {
            SavePathPoints(path);
            isPathMovementEnabled = true;
        }
        else
        {
            isPathMovementEnabled = false;
        }

        if (isPathMovementEnabled)
        {
            currentPath = path;
            currentPathIndex = 0;
        }

        if (!canEnableMixing) return;

        StartCoroutine(EnableMixing());
    }

    private void SavePathPoints(Path path)
    {
        savedPathPoints = new List<Vector3>();

        foreach (var point in path.PathPoints)
        {
            savedPathPoints.Add(point.transform.position);
        }

        currentPathIndex = 0;
    }

    #region  Animation methods
    //Called in the mixed projectiles animations!
    public void MixedProjectileAttack()
    {
        StartCoroutine(currentProjectile.ProjectileAttack(this.gameObject));
    }

    public void ReleaseProjectile()
    {
        ProjectilePooling.Instance.ReleaseProjectile(gameObject);
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SoundManager.Instance.PlaySoundOnce(Audio.bossAttack);
            DamagePlayer();
        }
        else if (!IsMixing && other.CompareTag("Projectile"))
        {
            ProjectilePrefab projectilePrefab = other.GetComponent<ProjectilePrefab>();

            if (projectilePrefab.IsMixing) return;

            MixProjectile(projectilePrefab);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!IsMixing && other.CompareTag("Projectile"))
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
        IsMixing = false;
    }

    private void MoveAlongPath()
    {
        if (currentPathIndex >= savedPathPoints.Count) return;

        Vector3 targetPoint = savedPathPoints[currentPathIndex];
        float step = (customProjectileSpeed > 0 ? customProjectileSpeed : currentProjectile.Speed) * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, step);

        Vector3 direction = targetPoint - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 180;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (Vector3.Distance(transform.position, targetPoint) <= 0.1f)
        {
            currentPathIndex++;
        }

        bool isLastPoint = currentPathIndex == savedPathPoints.Count - 1;

        if (!canBeDestroyed && isLastPoint)
        {
            CanBackshotMix = true;
        }
        else if (canBeDestroyed && isLastPoint)
        {
            isPathMovementEnabled = false;
            ProjectilePooling.Instance.ReleaseProjectile(gameObject);
        }
    }

    private void CheckIfOutOfView()
    {
        if (Camera.main == null) return;
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);

        if (viewportPosition.x < 0 || viewportPosition.x > 1 || viewportPosition.y < 0 || viewportPosition.y > 1)
        {
            ProjectilePooling.Instance.ReleaseProjectile(gameObject);
        }
    }
}
