using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ProjectileBase", menuName = "Projectiles/SmallOrangeProjectile", order = 1)]
public class OrangeProjectileSmall : ProjectileBaseSO
{
    public override ProjectileEnum MixedVariant(ProjectileBaseSO collidedProjectile)
    {
        return ProjectileEnum.None;
    }

    public override IEnumerator ProjectileAttack(GameObject bigProjectile)
    {
        yield return null;
    }
}
