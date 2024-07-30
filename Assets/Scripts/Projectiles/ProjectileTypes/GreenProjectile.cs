using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileBase", menuName = "Projectiles/GreenProjectile", order = 1)]
public class GreenProjectile : ProjectileBaseSO
{
    [Space(5)]
    [SerializeField] private GreenProjectileSmall smallGreenProjectile;
    [SerializeField] private ushort projectileAmount;

    public override ProjectileEnum MixedVariant(ProjectileBaseSO collidedProjectile)
    {
        return ProjectileEnum.None;
    }
    public override IEnumerator ProjectileAttack(GameObject bigProjectile)
    {
        Vector2 forwardDirection = bigProjectile.transform.up;
        float baseAngle = Mathf.Atan2(forwardDirection.y, forwardDirection.x) * Mathf.Rad2Deg;

        float angleIncrement = 60f;

        for (int i = 0; i < projectileAmount; i++)
        {
            float angle = baseAngle + i * angleIncrement;

            float radianAngle = angle * Mathf.Deg2Rad;

            Vector2 newDirection = new Vector2(Mathf.Cos(radianAngle), Mathf.Sin(radianAngle));
            GameObject smallProjectile = ProjectilePooling.Instance.GetProjectile(bigProjectile.transform.position, Quaternion.Euler(0, 0, angle + 180f));

            smallProjectile.GetComponent<ProjectilePrefab>().SetCurrentProjectile(smallGreenProjectile, false);

            smallProjectile.GetComponent<Rigidbody2D>().velocity = newDirection * smallGreenProjectile.Speed;
        }

        ProjectilePooling.Instance.ReleaseProjectile(bigProjectile);

        yield return null;
    }
}