using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectile
{
    float Damage { get; }
    public void Moving();
}
