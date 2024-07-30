using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileBase", menuName = "Projectiles/RedProjectile", order = 1)]
public class RedProjectile : ProjectileBaseSO
{
    public override ProjectileEnum MixedVariant(ProjectileBaseSO collidedProjectile)
    {
        if (!CanBeMixed || !collidedProjectile.CanBeMixed) return ProjectileEnum.None;

        if (collidedProjectile is YellowProjectile)
            return ProjectileEnum.Orange;
        else if (collidedProjectile is BlueProjectile)
            return ProjectileEnum.Purple;

        return ProjectileEnum.None;
    }

    public override IEnumerator ProjectileAttack(GameObject bigProjectile)
    {
        yield return null;
    }
}
