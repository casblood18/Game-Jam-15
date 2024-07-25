using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileBase", menuName = "Projectiles/YellowProjectile", order = 1)]
public class YellowProjectile : ProjectileBaseSO
{
    // let's just ignore the yellow, so it doesn't spawn two projectiles when yellow and red collides at the same time
    public override ProjectileEnum MixedVariant(ProjectileBaseSO collidedProjectile)
    {
        if (!CanBeMixed || !collidedProjectile.CanBeMixed) return ProjectileEnum.None;
        return ProjectileEnum.None;
    }
}
