using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileBase", menuName = "Projectiles/PurpleProjectile", order = 1)]
public class PurpleProjectile : ProjectileBaseSO
{
    [SerializeField] private PurpleBeam purpleBeamPrefab;

    public override ProjectileEnum MixedVariant(ProjectileBaseSO collidedProjectile)
    {
        return ProjectileEnum.None;
    }

    public override IEnumerator ProjectileAttack(GameObject bigProjectile)
    {
        Vector2 forwardDirection = bigProjectile.transform.up;
        float baseAngle = Mathf.Atan2(forwardDirection.y, forwardDirection.x) * Mathf.Rad2Deg;
        GameObject smallProjectile = ProjectilePooling.Instance.GetProjectile(bigProjectile.transform.position, Quaternion.Euler(0, 0, baseAngle + 180f));

        smallProjectile.GetComponent<ProjectilePrefab>().SetCurrentProjectile(purpleBeamPrefab, false);

        ProjectilePooling.Instance.ReleaseProjectile(bigProjectile);
        yield return null;
    }
}