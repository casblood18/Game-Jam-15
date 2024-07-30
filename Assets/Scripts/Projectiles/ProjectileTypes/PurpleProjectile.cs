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
        Vector2 playerPosition = Player.Instance.transform.position;

        Vector2 directionToPlayer = playerPosition - (Vector2)bigProjectile.transform.position;

        float angleToPlayer = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        bigProjectile.transform.rotation = Quaternion.Euler(0, 0, angleToPlayer);

        GameObject smallProjectile = ProjectilePooling.Instance.GetProjectile(bigProjectile.transform.position, Quaternion.Euler(0, 0, angleToPlayer + 180f));
        smallProjectile.GetComponent<ProjectilePrefab>().SetCurrentProjectile(purpleBeamPrefab, false);

        ProjectilePooling.Instance.ReleaseProjectile(bigProjectile);
        yield return null;
    }

}