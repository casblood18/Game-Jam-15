using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HealthBase : MonoBehaviour
{
    abstract protected void TakeDamage(float amount);

    abstract protected void Death();
}
