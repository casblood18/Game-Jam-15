using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;


public abstract class ProjectileBaseSO : ScriptableObject
{
    public ProjectileEnum ProjectileType;
    public bool CanBeMixed = false;

    public float Speed = 10f;
    public float Damage = 10f;

    [Space(10)]
    public Sprite ProjectileSprite;
    public AnimatorController ProjectileAnimator;
    public float Size;


    public abstract ProjectileEnum MixedVariant(ProjectileBaseSO collidedProjectile);
}
