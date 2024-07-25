using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileBase", menuName = "Projectiles/BlueProjectile", order = 1)]
public class BlueProjectile : ProjectileBaseSO
{
    public override ProjectileEnum MixedVariant(ProjectileBaseSO collidedProjectile)
    {
        if (!CanBeMixed || !collidedProjectile.CanBeMixed) return ProjectileEnum.None;

        if (collidedProjectile is YellowProjectile)
            return ProjectileEnum.Green;
        else if (collidedProjectile is RedProjectile)
            return ProjectileEnum.Purple;

        return ProjectileEnum.None;
    }
}
